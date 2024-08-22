using MossadAgentsRest.Dto;
using MossadAgentsRest.Models;

namespace MossadAgentsRest.Service
{
    public interface ITargetService
    {
        Task<TargetModel?> CreateTargetAsync(TargetDto target);
        Task<TargetModel?> UpdatTargetAsync(TargetDto target, int id);
        Task<TargetModel?> FindTargetByIdAsync(int id);
        Task<TargetModel?> ChengeLocationTargetByIdAsync(LocationDto pin, int id);
        Task<List<TargetModel>> GetAllTargetAsync();

    }
}
