using Microsoft.EntityFrameworkCore;
using MossadAgentsRest.Data;
using MossadAgentsRest.Dto;
using MossadAgentsRest.Models;

namespace MossadAgentsRest.Service
{
    public class AgentService(ApplicationDbContext contexts) : IAgentService
    {
        private readonly ApplicationDbContext _contexts = contexts;

        public async Task<AgentModel?> CreateAgentAsync(AgentDto agent)
        {
            AgentModel newAgent = new AgentModel()
            {
               Image = agent.PhotoUrl,
               Name = agent.Nickname,
               Location_X = agent.Location_X,
               Location_Y = agent.Location_Y,
               Status = 0,
            };
            await _contexts.AgentModel.AddAsync(newAgent);
            await _contexts.SaveChangesAsync();
            return newAgent;
        }


        public async Task<AgentModel?>UpdatAgentAsync(AgentDto agent, int id)
        {
            AgentModel? OldAgent = await FindAgentByIdAsync(id);
            if (OldAgent != null)
            {
                OldAgent.Name = agent.Nickname;
                OldAgent.Missions = OldAgent.Missions;
                OldAgent.Location_X = agent.Location_X;
                OldAgent.Image = agent.PhotoUrl;
                OldAgent.Location_Y = agent.Location_Y;
                OldAgent.Status = 0;
            }
            await _contexts.SaveChangesAsync();
            return OldAgent;
        }


        public async Task<AgentModel?> PinAgentByIdAsync(LocationDto pin, int id)
        {
            AgentModel? OldAgent = await FindAgentByIdAsync(id);
            if (OldAgent != null)
            {
                OldAgent.Location_X = pin.x;
                OldAgent.Location_Y= pin.y;
            }
            await _contexts.SaveChangesAsync();
            return OldAgent;
        }


        public async Task<AgentModel?> MoveAgentByIdAsync(string move, int id)
        {
            AgentModel? OldAgent = await FindAgentByIdAsync(id);
            if (OldAgent != null)
            {
                try
                {
                    var test = stepsByDirection[move];
                    var (move_x, move_y) = test;
                    LocationDto location = new LocationDto()
                    { x = move_x + OldAgent.Location_X, y = move_y + OldAgent.Location_Y };
                    await PinAgentByIdAsync(location, id);
                }
                catch (Exception ex) 
                { throw new Exception($"{ex.Message}", ex); }
            }
            await _contexts.SaveChangesAsync();
            return OldAgent;
        }


        public async Task<AgentModel?> FindAgentByIdAsync(int id) =>
          await _contexts.AgentModel.FindAsync(id) ?? null;

        public async Task<List<AgentModel>> GetAllAgentAsync()=>
            await contexts.AgentModel.ToListAsync();


        public async Task DeleteAgentByIdAsync(int id)
        {
            AgentModel agent = await FindAgentByIdAsync(id);
            if (agent != null)
            {
                _contexts?.AgentModel.Remove(agent);
                await _contexts.SaveChangesAsync();
            }
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
