using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansSample.IGrains;

namespace OrleansSample.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RobotController : ControllerBase
    {
        private readonly ILogger<RobotController> _logger;
        private readonly IRobotGrain robotGrain;

        public RobotController(
            ILogger<RobotController> logger, 
            IClusterClient client)
        {
            _logger = logger;
            robotGrain = client.GetGrain<IRobotGrain>("Robot");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(robotGrain.GetPrimaryKeyString());
        }

        [HttpGet("GetInstructionCount")]
        public async Task<IActionResult> GetInstructionCount()
        {
            var result = await robotGrain.GetInstructionCount();
            return Ok(result);
        }
    }
}
