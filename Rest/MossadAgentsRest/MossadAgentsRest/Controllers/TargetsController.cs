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
    public class targetsController(ITargetService targetService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TargetModel>>> GetAll() =>
           Ok(await targetService.GetAllTargetAsync());

        [HttpPost]
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

        [HttpPut("updateTarget/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateAgent(TargetDto target, int id)
        {
            if (target == null)
            {
                return NotFound();
            }
            var s = await targetService.UpdatTargetAsync(target, id);
            return Ok("target updated successfully");
        }

        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> PinTarget(LocationDto Location, int id)
        {
            if (Location == null)
            {
                return NotFound();
            }
            var s = await targetService.ChengeLocationTargetByIdAsync(Location, id);
            return Ok("agent updated successfully");
        }

        [HttpPut("{id}/Move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> MoveTarget(LocationDto Location, int id)
        {
            if (Location == null)
            {
                return NotFound();
            }
            var s = await targetService.ChengeLocationTargetByIdAsync(Location, id);
            return Ok("agent updated successfully");
        }
    }
}
