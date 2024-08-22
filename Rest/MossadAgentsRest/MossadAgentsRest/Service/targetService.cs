using Microsoft.EntityFrameworkCore;
using MossadAgentsRest.Data;
using MossadAgentsRest.Dto;
using MossadAgentsRest.Models;

namespace MossadAgentsRest.Service
{
    public class TargetService(ApplicationDbContext context) : ITargetService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<TargetModel?> ChengeLocationTargetByIdAsync(LocationDto pin, int id)
        {
            TargetModel? OldTarget = await FindTargetByIdAsync(id);
            if (OldTarget != null)
            {
                OldTarget.Location_X = pin.Location_X;
                OldTarget.Location_Y = pin.Location_Y;
            }
            await _context.SaveChangesAsync();
            return OldTarget;
        }

        public async Task<TargetModel?> CreateTargetAsync(TargetDto target)
        {
            TargetModel newTarget = new TargetModel()
            {
                 Name = target.Name,
                 Gob = target.Position,
                 Location_X = target.Location_X,
                 Location_Y = target.Location_Y,
                 Status = 0
            };
            await _context.TargetModel.AddAsync(newTarget);
            await _context.SaveChangesAsync();
            return newTarget;
        }

  
        public async Task<TargetModel?> FindTargetByIdAsync(int id) =>
           await _context.TargetModel.FindAsync(id) ?? null;

        public async Task<List<TargetModel>> GetAllTargetAsync() =>
            await context.TargetModel.ToListAsync();


        public async Task<TargetModel?> UpdatTargetAsync(TargetDto target, int id)
        {
            TargetModel? OldTarget = await FindTargetByIdAsync(id);
            if (OldTarget != null)
            {
                OldTarget.Name = target.Name;
                OldTarget.Gob = target.Position;
                OldTarget.Location_X = target.Location_X;
                OldTarget.Location_Y = target.Location_Y;
                OldTarget.Status = 0;             
            }
            await _context.SaveChangesAsync();
            return OldTarget;
        }
    }
}
