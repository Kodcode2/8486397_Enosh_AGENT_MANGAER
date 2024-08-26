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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<MissionModel>>> GetAllMissions() =>
            Ok(await missionService.GetAllMissionsAsync());


        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MissionModel>> GetMissionById(int id) =>
            Ok(await missionService.GetMissionById(id));


        [HttpGet("Dashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<DashboardModel>>> GetInfoToDashboard() =>
           Ok(await missionService.GetDashboardModel());


        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateMissions()
        {            
            while (true)
            {
                await missionService.DoMission();
                await Task.Delay(3000);
            }
            int missionId = 0;
            return Ok("request is successfully");
        }


        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update()
        {
            await missionService.MoveAgentToTarget();
            await missionService.DoMission();
            return Ok("request is successfully");
        }


        [HttpPut("active/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> ChangeStatus(int id)
        {    
            await missionService.ChangeStatusToActive(id);
            await missionService.DoMission();
            return Ok("active successfully");    
        }
    }
}
