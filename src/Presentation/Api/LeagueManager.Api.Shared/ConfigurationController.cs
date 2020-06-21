using LeagueManager.Application.Config;
using LeagueManager.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeagueManager.Api.Shared
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConfigurationController : ControllerBase
    {
        private readonly IDbConfigurator dbConfigurator;

        public ConfigurationController(IDbConfigurator dbConfigurator)
        {
            this.dbConfigurator = dbConfigurator;
        }

        [HttpPut]
        public async Task<IActionResult> Configure(DbConfig dbConfig)
        {
            try
            {
                await dbConfigurator.Configure(dbConfig);
            }
            catch
            {
                return BadRequest("Configuration failed!");
            }

            return Ok();
        }
    }
}