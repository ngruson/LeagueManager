using System;
using System.Threading.Tasks;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.Match.Commands.AddPlayerToLineup;
using LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal;
using Microsoft.Extensions.Logging;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueCompetitors;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeague;

namespace LeagueManager.Api.CompetitionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ILogger<CompetitionsController> logger;

        public CompetitionsController(
            IMediator mediator,
            IMapper mapper,
            ILogger<CompetitionsController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
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
            catch (Exception)
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

        // GET api/competition/Premier League 2019-2020
        [HttpGet("{name}")]
        public async Task<IActionResult> GetCompetition(string name)
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
    }
}