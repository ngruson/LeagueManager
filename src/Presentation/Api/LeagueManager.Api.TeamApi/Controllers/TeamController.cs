using System;
using System.Threading.Tasks;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Teams.Commands.CreateTeam;
using LeagueManager.Application.Teams.Queries.GetTeams;
using LeagueManager.Application.Teams.Queries.GetTeamsByCountry;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeagueManager.Api.TeamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IMediator mediator;

        public TeamController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            try
            {
                var teams = await mediator.Send(new GetTeamsQuery());
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }

        [HttpGet("{country}")]
        public async Task<IActionResult> GetTeamsByCountry(string country)
        {
            try
            {
                var teams = await mediator.Send(new GetTeamsByCountryQuery { Country = country });
                return Ok(teams);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamCommand command)
        {
            try
            {
                await mediator.Send(command);
                return Created($"/Team/{command.Name}", new { command.Name });
            }
            catch (TeamAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}