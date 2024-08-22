using Microsoft.EntityFrameworkCore;
using MossadAgentsRest.Data;
using MossadAgentsRest.Dto;
using MossadAgentsRest.Models;

namespace MossadAgentsRest.Service
{
    public class AgentService(ApplicationDbContext contects) : IAgentService
    {
        private readonly ApplicationDbContext _contects = contects;

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
            await _contects.AgentModel.AddAsync(newAgent);
            await _contects.SaveChangesAsync();
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
                OldAgent.Status = 0;//agent.Status ?? 0;
            }
            await _contects.SaveChangesAsync();
            return OldAgent;

                     
        }


        public async Task<AgentModel?> PinAgentByIdAsync(LocationDto pin, int id)
        {
            AgentModel? OldAgent = await FindAgentByIdAsync(id);
            if (OldAgent != null)
            {
                OldAgent.Location_X = pin.Location_X;
                OldAgent.Location_Y= pin.Location_Y;
            }
            await _contects.SaveChangesAsync();
            return OldAgent;
        }
        public async Task<AgentModel?> FindAgentByIdAsync(int id) =>
          await _contects.AgentModel.FindAsync(id) ?? null;

        public async Task<AgentModel?> NoveAgentByIdAsync(string move, int id)
        {
            AgentModel? OldAgent = await FindAgentByIdAsync(id);
            if (OldAgent != null)
            {
                try
                {
                    var test = stepsByDirection[move];

                    var (move_x, move_y) = test;
                    LocationDto location = new LocationDto()
                    { Location_X = move_x + OldAgent.Location_X, Location_Y = move_y + OldAgent.Location_Y };
                    await PinAgentByIdAsync(location, id);
                }
                catch (Exception ex) 
                { throw new Exception($"{ex.Message}", ex); }
            }
            await _contects.SaveChangesAsync();
            return OldAgent;
        }

        public async Task<List<AgentModel>> GetAllAgentAsync()=>
            await contects.AgentModel.ToListAsync();
       

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
