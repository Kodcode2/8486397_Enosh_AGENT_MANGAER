using MossadAgentsRest.Models;

namespace MossadAgentsRest.Service
{
    public interface IMissionService
    {
        Task<MissionModel>CreateNewMission(List<AgentModel>agents,List<TargetModel>targets);
        public Task<MissionModel> DoMission();
        public Task<List<MissionModel>> GetAllmission();
        public Task<double> CalculateTime(AgentModel agent, TargetModel target);
        public Task<bool> GetAgentIdIfIsInMission(int AgentId, int targetId);
        public Task ActiveAgent(MissionModel mission);

    }
}
