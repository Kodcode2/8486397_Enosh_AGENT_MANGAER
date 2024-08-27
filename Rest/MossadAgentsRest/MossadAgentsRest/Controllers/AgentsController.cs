using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MossadAgentsRest.Dto;
using MossadAgentsRest.Models;
using MossadAgentsRest.Service;

namespace MossadAgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class agentsController(IAgentService agentService ,IMissionService missionService
        ) : ControllerBase
    {
        [HttpGet]//קונטרולר שמציג את כל הסוכנים
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AgentModel>>> GetAll() =>
            Ok(await agentService.GetAllAgentAsync());


        [HttpPost]//יוצר סוכן חדש
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgentDto>> CreatAgent(AgentDto agent)
        {
            if (agent == null) 
            {
                return BadRequest("agent is null");
            }
            var res = await agentService.CreateAgentAsync(agent);
            await missionService.DoMission();
            return Created("agent inserted successfully", new { id = res.Id});
        }


        [HttpPut("{id}/update")]//מעדכן סוכן לפי id
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateAgent(AgentDto agent ,int id)
        {
            if (agent == null)
            {
                return NotFound("not found");
            }
            var s = await agentService.UpdatAgentAsync(agent,id);
            await missionService.DoMission();
            return Ok("agent updated successfully");
        }


        [HttpPut("{id}/pin")]//קביעת מיקום רישוני של סוכן
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> PinAgent(LocationDto Location, int id)
        {
            if (Location == null)
            {
                return NotFound("not found");
            }       
            var s = await agentService.PinAgentByIdAsync(Location, id);
            await missionService.DoMission();
            return Ok("agent updated successfully");
        }


        [HttpPut("{id}/move")]//מזיז סוכן לפי אות שמייצגת כיוון
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> MoveAgent(string move, int id)
        {
            if (move == null)
            {
                return NotFound();
            }
            var s = await agentService.MoveAgentByIdAsync(move, id);
            await missionService.DoMission();
            return Ok("agent updated successfully");
        }    
    }
}
