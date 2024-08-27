using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MossadAgentsRest.Dto;
using MossadAgentsRest.Models;
using MossadAgentsRest.Service;

namespace MossadAgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class targetsController(ITargetService targetService ,IMissionService missionService) : ControllerBase
    {
        [HttpGet]//מציג את כל היעדים
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TargetModel>>> GetAll() =>
           Ok(await targetService.GetAllTargetAsync());


        [HttpPost]//יוצר יעד חדש
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TargetDto>> CreatAgent(TargetDto target)
        {
            if (target == null)
            {
                return NotFound("target is null");
            }
            var res = await targetService.CreateTargetAsync(target);
            return Created("target inserted successfully", new { id = res.Id });
        }


        [HttpPut("updateTarget/{id}")]//מעדכן יעד לפי מזהה
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateAgent(TargetDto target, int id)
        {
            if (target == null)
            {
                return NotFound("target is null");
            }
            var s = await targetService.UpdatTargetAsync(target, id);
            await missionService.DoMission();
            return Ok("target updated successfully");
        }


        [HttpPut("{id}/pin")]//קביעת מיקום ראשוני 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> PinTarget(LocationDto Location, int id)
        {
            if (Location == null)
            {
                return NotFound("not fuond");
            }
            var s = await targetService.PinAgentByIdAsync(Location, id);
            await missionService.DoMission();
            return Ok("target updated successfully");
        }


        [HttpPut("{id}/move")]//מזיז סוכן לי אות שמיצגת כיוון
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> MoveTarget(string move, int id)
        {
            if (move == null)
            {
                return NotFound("not fuond");
            }
            var s = await targetService.MoveTargetByIdAsync(move, id);
            await missionService.DoMission();
            return Ok("target updated successfully");
        }
    }
}
