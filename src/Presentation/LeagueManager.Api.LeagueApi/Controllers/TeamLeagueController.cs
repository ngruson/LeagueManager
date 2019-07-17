using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Leagues.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LeagueManager.Api.LeagueApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamLeagueController :  ControllerBase
    {
        private readonly IMediator mediator;

        public TeamLeagueController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // POST api/teamleague
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateTeamLeague([FromBody] CreateTeamLeagueCommand command)
        {
            try
            {
                await mediator.Send(command);
                return Created($"/League/{command.Name}", new { command.Name });
            }
            catch (CompetitionAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TeamNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}
