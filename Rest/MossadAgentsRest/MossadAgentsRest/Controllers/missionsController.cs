using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MossadAgentsRest.Dto;
using MossadAgentsRest.Models;
using MossadAgentsRest.Service;

namespace MossadAgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class missionsController(IMissionService missionService) : ControllerBase
    {
        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task GetAllMissions()
        {            
            while (true)
            {
                var a = await missionService.DoMission();
                await Task.Delay(5000);
            }
            //return Ok("request is successfully");
        }


        [HttpPost("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task ActiveAgent()
        {
            
            //return Ok("request is successfully");
        }
    }
}
