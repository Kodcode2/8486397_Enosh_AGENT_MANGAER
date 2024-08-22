using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Kerberos;
using MossadAgentsRest.Controllers;
using MossadAgentsRest.Data;
using MossadAgentsRest.Models;
using System.Collections.Generic;

namespace MossadAgentsRest.Service
{
    public class MissionService(ApplicationDbContext context ,
        IAgentService agentService ,ITargetService targetService) : IMissionService
    {

        private readonly ApplicationDbContext _context = context;


        public async Task<MissionModel?> DoMission()
        {
            var agent = await agentService.GetAllAgentAsync();
            var targets = await targetService.GetAllTargetAsync();
            var newMission = await CreateNewMission(agent, targets);
            return newMission;
        }
        public async Task<MissionModel?> CreateNewMission(List<AgentModel> agents, List<TargetModel> targets)
        {
            List<AgentModel> allAgents = await _context.AgentModel.ToListAsync();
            List<TargetModel> allTargets = await _context.TargetModel.ToListAsync();
            MissionModel newMission = new MissionModel() ;  

           
            foreach (var agent in allAgents)
            {
                if (agent.Status == AgentModel.StatusAgent.Active )
                {
                    continue;
                }
                foreach (var target in allTargets)
                {
                    var ChekId2 = await GetAgentIdIfIsInMission(agent.Id ,agent.Id);
                    if (ChekId2)
                    {
                        continue;
                    }
                    if (await CalculateTime(agent ,target) <= 200)
                    {
                        newMission.AgentId = agent.Id;
                        newMission.TargetId = target.Id;
                        newMission.Agent = agent;
                        newMission.Target = target;
                        newMission.status = MissionModel.MissionStatus.Suggest;
                        newMission.LeftTime = await CalculateTime(agent, target)/5;
                        ;
                    }

                }
            }
            await _context.MissionModel.AddAsync(newMission);
            await _context.SaveChangesAsync();
            return newMission;
        }


        public async Task<double>CalculateTime(AgentModel agent ,TargetModel target)
        {
            double time = Math.Sqrt(Math.Pow(agent.Location_X - target.Location_X, 2)
                            + Math.Pow(agent.Location_Y - target.Location_Y, 2));
            return time ;
        }

        public async Task<List<MissionModel>> GetAllmission()=> 
            await _context.MissionModel.ToListAsync();

        public async Task<bool> GetAgentIdIfIsInMission(int AgentId ,int targetId)
        {
            var kk = await GetAllmission();
            List<MissionModel?> a =  kk.Where(a => a.AgentId == AgentId).ToList();
            List<MissionModel?> b =  kk.Where(a => a.TargetId == targetId).ToList();
            if (a.Count > 0 && b.Count > 0)
            {  return true; }
            return false;
        }

        public Task ActiveAgent(MissionModel mission)
        {
            throw new NotImplementedException();
        }
    }
}
