using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client.Kerberos;
using MossadAgentsRest.Controllers;
using MossadAgentsRest.Data;
using MossadAgentsRest.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace MossadAgentsRest.Service
{
    public class MissionService(ApplicationDbContext context,
        IAgentService agentService, ITargetService targetService) : IMissionService
    {

        private readonly ApplicationDbContext _context = context;

        //מפעיל את פונקציית הניהול של ההצעות לציוות מפעילים אותה על כל פעולה שקורת בפרוייקט
        public async Task DoMission()
        {          
            await ManageMission();
            return ;
        }
        //פונקצייה שמנהלת את ההצעות לציוות יוצרת חדשים ומוחקת מה שלא רלוונטי
        public async Task<MissionModel?> ManageMission()
        {
            List<AgentModel> allAgents = await _context.AgentModel.ToListAsync();
            List<TargetModel> allTargets = await _context.TargetModel.ToListAsync();
            MissionModel newMission = new MissionModel();
            List<MissionModel> missionsToAdd = [];
            //לולאה מקוננת שמייצרת ומוחקת הצעות לציוות
            foreach (var agent in allAgents)
            {   //אם סוכן פעיל מוחקים את כל שאר ההצעות לציוות שיש עם המזהה שלו
                if (agent.Status == AgentModel.StatusAgent.Active)
                {    
                    await DeleteMissionIfAgentActive(agent.Id);
                    continue;
                }
                foreach (var target in allTargets)
                {    //תנאי שבודק אם יעד מת או בפעולה ומוחקת אותו מהרשימה
                    if (target.Status == TargetModel.StatusTarget.dead ||
                        target.Status == TargetModel.StatusTarget.Active)
                    {
                        await DeletMissionIfTargetDeadOrActive(target.Id);
                    }// ואם הבודק מרחק בין יעד לסוכן ומחזיר אמת אם הם מחוץ לטווח ואם הם יצאו מהטווח מוחקת אותם
                    //ובודק גם שהם לא נמצאים כבר הרשימת ההצעות
                    var ChekId2 = await CheckAgentIdIfIsInMissionAndDelete(agent.Id, target.Id);              
                    if (ChekId2 || target.Status == TargetModel.StatusTarget.dead ||
                        target.Status == TargetModel.StatusTarget.Active)
                    {
                        continue;
                    }//מחשב את הטווח ומייצר חדש במקרה הצורך
                    if (await CalculateRange(agent, target) <= 200)
                    {
                        newMission.AgentId = agent.Id;
                        newMission.TargetId = target.Id;
                        newMission.Agent = agent;
                        newMission.Target = target;
                        newMission.Status = MissionModel.MissionStatus.Suggest;
                        newMission.LeftTime = TimeSpan.FromHours
                            ( await CalculateRange(agent, target) / 5)
                            .ToString(@"hh\:mm\:ss");                        
                    }     //אם התקיימו התנאים מכניס אותם לרשימה               
                    missionsToAdd.Add(newMission);
                }
            }
            await _context.MissionModel.AddRangeAsync(missionsToAdd);
            await _context.SaveChangesAsync();
            return newMission;
        }

        //חישוב מרחק לפי נוסחה
        public async Task<double> CalculateRange(AgentModel agent, TargetModel target)
        {
            double time = Math.Sqrt(Math.Pow(agent.Location_X - target.Location_X, 2)
                            + Math.Pow(agent.Location_Y - target.Location_Y, 2));
            return time;
        }

        //מציג את כל הסוכנים או היעדים או המשימות
        public async Task<List<TargetModel>> GetAllTargetsAsync() =>
            await _context.TargetModel.ToListAsync();
        public async Task<List<MissionModel>> GetAllmissionAsync() =>
            await _context.MissionModel.ToListAsync();
        public async Task<List<AgentModel>> GetAllAgentsAsync() =>
            await _context.AgentModel.ToListAsync();

        //בודק את הטווח ומוחק אם יצאו מהטווח ומחזיר שקר אם קיים כבר הצעה עם אותו סוכן ואותו יעד
        public async Task<bool> CheckAgentIdIfIsInMissionAndDelete(int AgentId, int targetId)
        {
            var OldMissions = await GetAllmissionAsync();
            List<MissionModel> m = OldMissions
                .Where(a => a.AgentId == AgentId && a.TargetId == targetId)
                .ToList();
            if (m.Count > 0)
            {
                var a = await DeleteIfOutOfRange(m.First());
                if (a)
                {
                    _context.MissionModel.Remove(m.First());
                    await _context.SaveChangesAsync();
                }
                return true; 
            }
            return false;
        }

        //מזיז סוכן ליעד
        public async Task MoveAgentToTarget()
        {
            var allMissions = await GetMissionByStatus();
            foreach (var mission in allMissions)
            {
                int agent_x = mission.Agent.Location_X;
                int agent_y = mission.Agent.Location_Y;
                int target_x = mission.Target.Location_X;
                int target_y = mission.Target.Location_Y;

                int xDirection = agent_x < target_x ? 1 : (agent_x > target_x ? -1 : 0);
                int yDirection = agent_y < target_y ? 1 : (agent_y > target_y ? -1 : 0);
                agent_x += xDirection;
                agent_y += yDirection;
                mission.Agent.Location_X = agent_x;
                mission.Agent.Location_Y = agent_y;
                await _context.SaveChangesAsync();
                if (agent_x == target_x && agent_y == target_y)
                {   //הורג את היעד אם הם באותו מיקום
                    await KillTarget(mission);
                    int d = 5;
                    continue;
                }
            }
        }

        //מציג סוכנים לפי סטטוס
        public async Task<List<MissionModel>> GetMissionByStatus()
        {
            var newlist = await _context.MissionModel
                .Where(a => a.Status == MissionModel.MissionStatus.Active)
                .Include(m => m.Agent)
                .Include(t => t.Target)
                .ToListAsync();
            await _context.SaveChangesAsync();

            return newlist;
        }

        //הורג יעד
        public async Task KillTarget(MissionModel missionModel)
        {
            var killtarget = await _context.TargetModel.FindAsync(missionModel.TargetId);
            killtarget.Status = TargetModel.StatusTarget.dead;
            var oldagent = await _context.AgentModel.FindAsync(missionModel.AgentId);
            oldagent.Status = AgentModel.StatusAgent.Sleep;
            oldagent.kills_sum += 1;
            var oldmission = await _context.MissionModel.FindAsync(missionModel.Id);
            oldmission.Status = MissionModel.MissionStatus.finish;
            oldmission.KillTime = DateTime.Now;
            try
            {   //מילון שמחשב את זמן ביצוע הפעולה לפי טיימר
                _stopwatch[oldmission.Id].Stop();
                oldmission.TimeFromStartToEnd = _stopwatch[oldmission.Id].Elapsed.ToString(@"hh\:mm\:ss");
            }
            catch
            {
                oldmission.TimeFromStartToEnd = "0";
            }
            await _context.SaveChangesAsync();
        }

        //הפיכת משימה לפעילה
        public async Task ChangeStatusToActive(int id)
        {
            var m = await GetMissionByIdIncludObjectsAsync(id);
            if (m != null)
            {
                var a = await GetAgentById(m.AgentId);
                var t = await GetTargetById(m.TargetId);
                m.Status = MissionModel.MissionStatus.Active;
                a.Status = AgentModel.StatusAgent.Active;
                t.Status = TargetModel.StatusTarget.Active;
                await StartTimerActiveMission(m.Id);
                await _context.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }

        //מילון ופונקציה שמחשבים את זמן ביצוע הפעולה
        private static Dictionary<int, Stopwatch> _stopwatch = new();
        public async Task StartTimerActiveMission(int idMission)
        {
            try
            {
                MissionModel? mission = await _context.MissionModel
                    .Include(x => x.Agent)
                    .Include(m => m.Target)
                    .FirstOrDefaultAsync(x => x.Id == idMission);
                if (mission == null) { return; }              
                mission.Status = MissionModel.MissionStatus.Active;
                mission.Agent.Status = AgentModel.StatusAgent.Active;
                _stopwatch[mission.Id] = new Stopwatch();
                _stopwatch[mission.Id].Start();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }

        //מביא את כל ההצעות כולל האובייקטים
        public async Task<MissionModel> GetMissionByIdIncludObjectsAsync(int missionId)
        {
            var m = await _context.MissionModel
                .Where(m => m.Id == missionId)
                .Include(a => a.Agent)
                .Include(t => t.Target)
                .FirstOrDefaultAsync();
            return m;
        }
        //מביא את כל היעדים
        public async Task<TargetModel?> GetTargetById(int targetId) =>
            await _context.TargetModel.FindAsync(targetId);
       //מביא את כל הסוכנים לפי מזהה
        public async Task<AgentModel?> GetAgentById(int agentId) =>
            await _context.AgentModel.FindAsync(agentId);
        //מביא את כל ההצעות לפי מזהה
        public async Task<MissionModel?> GetMissionById(int id) =>
            await _context.MissionModel.FindAsync(id);
        //מביא את כל ההצעות
        public async Task<List<MissionModel>> GetAllMissionsAsync() =>
             await _context.MissionModel.ToListAsync();

        //מוחק הצעה אם יצאו מהטווח
        public async Task<bool> DeleteIfOutOfRange(MissionModel mission)
        {
            if (mission.Status == MissionModel.MissionStatus.Active)
            {
                return false;
            } ///מחשב את הטווח
            var b = await CalculateRange(mission.Agent, mission.Target) > 200;
            if (b)
            {
                return true;
            }
            return false;        
        }

        //מציג נתונים כלליים על הפרוייקט
        public async Task<DashboardModel> GetDashboardModel()
        {
            DashboardModel dashboard = new DashboardModel();
            var missionModels = await GetAllMissionsAsync();
            var targetModels = await targetService.GetAllTargetAsync();
            var agentModels = await agentService.GetAllAgentAsync();
            var missionsActiveNum = missionModels
                .Where(a => a.Status == MissionModel.MissionStatus.Active)
                .ToList();
            var agentActiveNum = agentModels
                .Where(t => t.Status == AgentModel.StatusAgent.Active)
                .ToList();
            var targetModeisDeadsNum = targetModels
                .Where(a => a.Status == TargetModel.StatusTarget.dead)
                .ToList();
            dashboard.MissionsActiveNum = missionsActiveNum.Count();
            dashboard.TargetDaedNum = targetModeisDeadsNum.Count();
            dashboard.AgentActiveNum = agentActiveNum.Count();
            dashboard.MissionsNum = missionModels.Count();
            dashboard.AgentNum = agentModels.Count();
            dashboard.TargetNum = targetModels.Count();
            dashboard.RelationAgentsToTargets = agentModels.Count() / targetModels.Count();
            return dashboard;
        }
        //מוחק הצעה אם הסוכן פעיל
        public async Task DeleteMissionIfAgentActive(int agentId)
        {
            var missions = await GetAllmissionAsync();
            List<MissionModel> missionsToDelet = missions
                .Where(a => a.AgentId == agentId)
                .Where(a => a.Status != MissionModel.MissionStatus.Active)
                .Where(a => a.Status != MissionModel.MissionStatus.finish)
                .ToList();
            if (missionsToDelet.Count() > 0 )
            {
                _context.MissionModel.RemoveRange(missionsToDelet);
                await _context.SaveChangesAsync();
            }
        }

        //מוחק הצעה אם היעד פעיל או מת
        public async Task DeletMissionIfTargetDeadOrActive(int TargetId)
        {
            var missions = await GetAllmissionAsync();
            List<MissionModel> missionsToDelet = missions
                .Where(a => a.TargetId == TargetId)
                .Where(a => a.Status != MissionModel.MissionStatus.Active)
                .Where(a => a.Status != MissionModel.MissionStatus.finish)
                .ToList();
            if (missionsToDelet.Count() > 0)
            {
                _context.MissionModel.RemoveRange(missionsToDelet);
                await _context.SaveChangesAsync();
            }
        }

      
    }
}
