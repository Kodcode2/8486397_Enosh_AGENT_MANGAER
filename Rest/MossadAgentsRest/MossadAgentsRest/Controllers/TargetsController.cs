using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MossadAgentsRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        [HttpGet("shmuel")]
        public ActionResult<string> Shmuel()
        {
            int a = 8;
            return Ok("!!!");
        }
    }
}
