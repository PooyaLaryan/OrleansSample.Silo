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
        private readonly IClusterClient _client;

        public RobotController(
            ILogger<RobotController> logger, 
            IClusterClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        [Route("robot/{name}/instruction")]
        public Task<string> Get(string name)
        {
            var grain = this._client.GetGrain<IRobotGrainStateless>(name);
            return grain.GetNextInstruction();
        }

        [HttpPost]
        [Route("robot/{name}/instruction")]
        public async Task<IActionResult> Post(string name, string value)
        {
            var grain = this._client.GetGrain<IRobotGrainStateless>(name);
            await grain.AddInstruction(value);
            return Ok();
        }
    }
}
