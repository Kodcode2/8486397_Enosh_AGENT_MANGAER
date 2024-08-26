using MossadAgentsRest.Dto;
using MossadAgentsRest.Models;

namespace MossadAgentsRest.Service
{
    public interface ITargetService
    {
        Task<TargetModel?> CreateTargetAsync(TargetDto target);
        Task<TargetModel?> UpdatTargetAsync(TargetDto target, int id);
        Task<TargetModel?> FindTargetByIdAsync(int id);
        Task<TargetModel?> PinAgentByIdAsync(LocationDto pin, int id);
        Task<TargetModel?> MoveTargetByIdAsync(string move, int id);
        Task<List<TargetModel>> GetAllTargetAsync();
        



    }
}
