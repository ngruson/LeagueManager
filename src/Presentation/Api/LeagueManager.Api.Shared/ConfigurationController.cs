using LeagueManager.Application.Config;
using LeagueManager.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Configure(DbConfig dbConfig)
        {
            try
            {
                dbConfigurator.Configure(dbConfig);
            }
            catch
            {
                return BadRequest("Configuration failed!");
            }

            return Ok();
        }
    }
}