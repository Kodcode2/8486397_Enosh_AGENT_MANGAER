using Microsoft.EntityFrameworkCore;
using MossadAgentsRest.Data;
using MossadAgentsRest.Dto;
using MossadAgentsRest.Models;

namespace MossadAgentsRest.Service
{
    public class TargetService(ApplicationDbContext context) : ITargetService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<TargetModel?> PinAgentByIdAsync(LocationDto pin, int id)
        {
            TargetModel? OldTarget = await FindTargetByIdAsync(id);
            if (OldTarget != null)
            {
                OldTarget.Location_X = pin.x;
                OldTarget.Location_Y = pin.y;
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


        public async Task<TargetModel?> MoveTargetByIdAsync(string move, int id)
        {
            TargetModel? OldTarget = await FindTargetByIdAsync(id);
            if (OldTarget != null)
            {
                try
                {
                    var test = stepsByDirection[move];

                    var (move_x, move_y) = test;
                    LocationDto location = new LocationDto()
                    { x = move_x + OldTarget.Location_X, y = move_y + OldTarget.Location_Y };
                    await PinAgentByIdAsync(location, id);
                }
                catch (Exception ex)
                { throw new Exception($"{ex.Message}", ex); }
            }
            await _context.SaveChangesAsync();
            return OldTarget;
        }


        private readonly Dictionary<string, (int x, int y)> stepsByDirection = new()
        {
            {"n", (x: 0,y: 1) },
            {"s", (x: 0,y: -1) },
            {"e", (x: 1,y: 0) },
            {"w", (x: -1,y: 0) },
            {"nw", (x: -1,y: 1) },
            {"ne", (x: 1,y: 1) },
            {"sw", (x: -1,y: -1) },
            {"se", (x: 1,y: -1) }
        };
    }
}
