using System;
using System.Threading.Tasks;
using LeagueManager.Application.Competitions.Commands;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeagueManager.Api.CompetitionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly IMediator mediator;

        public CompetitionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET api/competition?country={country}
        [HttpGet]
        public async Task<IActionResult> GetCompetitions([FromQuery]string country)
        {
            try
            {
                var competitions = await mediator.Send(new GetCompetitionsQuery { Country = country });
                return Ok(competitions);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }

        // GET api/competition/Premier League 2019-2020
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                var league = await mediator.Send(new GetCompetitionQuery { Name = name });
                return Ok(league);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        // POST api/competition/teamleague
        [HttpPost("teamleague")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateTeamLeague([FromBody] CreateTeamLeagueCommand command)
        {
            try
            {
                await mediator.Send(command);
                return Created($"/competition/{command.Name}", new { command.Name });
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