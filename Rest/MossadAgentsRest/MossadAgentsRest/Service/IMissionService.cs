using MossadAgentsRest.Models;

namespace MossadAgentsRest.Service
{
    public interface IMissionService
    {
        public Task DoMission();
        public Task<MissionModel>ManageMission();
        public Task<List<MissionModel>> GetAllmissionAsync();
        public Task<List<MissionModel>>GetMissionByStatus();
        public Task<double> CalculateRange(AgentModel agent, TargetModel target);
        public Task<bool> CheckAgentIdIfIsInMissionAndDelete(int AgentId, int targetId);
        public Task MoveAgentToTarget();
        public Task KillTarget(MissionModel missionModel);
        public Task ChangeStatusToActive(int id);
        public Task<MissionModel>GetMissionByIdIncludObjectsAsync(int missionId);
        public Task<AgentModel> GetAgentById(int agentId);
        public Task<TargetModel> GetTargetById(int targetId);
        public Task<bool> DeleteIfOutOfRange(MissionModel mission);
        public Task<List<MissionModel>> GetAllMissionsAsync();
        public Task<DashboardModel> GetDashboardModel();
        public Task StartTimerActiveMission(int idMission);
        public Task<MissionModel?> GetMissionById(int id);
        public Task<List<TargetModel>> GetAllTargetsAsync();
        public Task<List<AgentModel>> GetAllAgentsAsync();
        public Task DeleteMissionIfAgentActive(int agentId);
        public Task DeletMissionIfTargetDeadOrActive(int TargetId);

    }
}
