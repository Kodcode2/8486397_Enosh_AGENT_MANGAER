using MossadAgentsRest.Dto;
using MossadAgentsRest.Models;

namespace MossadAgentsRest.Service
{
    public interface IAgentService
    {
        Task<AgentModel?> CreateAgentAsync(AgentDto agent);
        Task<AgentModel?> UpdatAgentAsync(AgentDto agent,int id);
        Task<AgentModel?> FindAgentByIdAsync(int id);
        Task<AgentModel?> PinAgentByIdAsync(LocationDto pin, int id);
        Task<AgentModel?> NoveAgentByIdAsync(string move, int id);
        Task<List<AgentModel>>GetAllAgentAsync();

    }
}
