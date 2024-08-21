using MossadAgentsRest.Models;

namespace MossadAgentsRest.Service
{
    public interface IAgentService
    {
        Task<AgentModel?> CreateAgentAsync(AgentModel agent);

    }
}
