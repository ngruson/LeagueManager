using System;
using System.Threading.Tasks;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LeagueManager.Api.CompetitionApi.Dto;
using AutoMapper;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;

namespace LeagueManager.Api.CompetitionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CompetitionController(
            IMediator mediator,
            IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
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

        // GET api/competition/teamleague/Premier League 2019-2020/table
        [HttpGet("teamleague/{name}/table")]
        public async Task<IActionResult> GetTeamLeagueTable(string name)
        {
            try
            {
                var table = await mediator.Send(new GetTeamLeagueTableQuery { LeagueName = name });
                return Ok(table);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        // GET api/competition/teamleague/Premier League 2019-2020/rounds
        [HttpGet("teamleague/{name}/rounds")]
        public async Task<IActionResult> GetTeamLeagueRounds(string name)
        {
            try
            {
                var rounds = await mediator.Send(new GetTeamLeagueRoundsQuery { LeagueName = name });
                return Ok(rounds);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        // GET api/competition/teamleague/Premier League 2019-2020/match/guid
        [HttpGet("teamleague/{name}/match/{guid}")]
        public async Task<IActionResult> GetTeamLeagueMatch(string name, string guid)
        {
            try
            {
                var match = await mediator.Send(new GetTeamLeagueMatchQuery {
                    LeagueName = name,
                    Guid = new Guid(guid)
                });
                return Ok(match);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        // PUT api/competition/teamleague/Premier League 2019-2020/match
        [HttpPut("teamleague/{name}/match/{guid}")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTeamLeagueMatch(string name, string guid, [FromBody]UpdateTeamLeagueMatchDto dto)
        {
            try
            {
                var command = mapper.Map<UpdateTeamLeagueMatchCommand>(dto, opt =>
                    {
                        opt.Items["leagueName"] = name;
                        opt.Items["guid"] = guid;
                    });
                    
                var match = await mediator.Send(command);
                return Ok(match);
            }
            catch (MatchNotFoundException ex)
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

        // PUT api/competition/teamleague/Premier League 2019-2020/match/{guid}/score
        [HttpPut("teamleague/{name}/match/{guid}/score")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTeamLeagueMatchScore(string name, string guid, [FromBody]UpdateScoreDto dto)
        {
            try
            {
                var command = mapper.Map<UpdateTeamLeagueMatchScoreCommand>(dto, opt =>
                {
                    opt.Items["leagueName"] = name;
                    opt.Items["guid"] = guid;
                });

                var match = await mediator.Send(command);
                return Ok(match);
            }
            catch (MatchNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TeamNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}