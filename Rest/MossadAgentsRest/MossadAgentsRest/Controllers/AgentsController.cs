using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MossadAgentsRest.Dto;
using MossadAgentsRest.Models;
using MossadAgentsRest.Service;

namespace MossadAgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class agentsController(IAgentService agentService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AgentModel>>> GetAll() =>
            Ok(await agentService.GetAllAgentAsync());


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgentDto>> CreatAgent(AgentDto agent)
        {
            if (agent == null) 
            {
                return NotFound("agent is null");
            }
            var res = await agentService.CreateAgentAsync(agent);
            return Created("agent inserted successfully", new { id = res.Id});
        }


        [HttpPut("{id}/update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateAgent(AgentDto agent ,int id)
        {
            if (agent == null)
            {
                return NotFound();
            }
            var s = await agentService.UpdatAgentAsync(agent,id);
            return Ok("agent updated successfully");
        }

        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> PinAgent(LocationDto Location, int id)
        {
            if (Location == null)
            {
                return NotFound();
            }       
            var s = await agentService.PinAgentByIdAsync(Location, id);
            return Ok("agent updated successfully");
        }


        [HttpPut("{id}/Move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> MoveAgent(string move, int id)
        {
            if (move == null)
            {
                return NotFound();
            }
            var s = await agentService.NoveAgentByIdAsync(move, id);
            return Ok("agent updated successfully");
        }      
    }
}
