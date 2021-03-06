﻿using System;
using System.Threading.Tasks;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddPlayerToLineup;
using LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor;
using LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry;

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
            catch (LeagueManagerException ex)
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

        [HttpGet("teamleague/{leagueName}/competitor/{teamName}/players")]
        public async Task<IActionResult> GetPlayersForTeamCompetitor(string leagueName, string teamName)
        {
            try
            {
                var request = new GetPlayersForTeamCompetitorQuery
                {
                    LeagueName = leagueName,
                    TeamName = teamName
                };
                var players = await mediator.Send(request);
                return Ok(players);
            }
            catch (TeamLeagueNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TeamNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        [HttpGet("teamleague/{leagueName}/competitor/{teamName}/player/{playerName}")]
        public async Task<IActionResult> GetPlayerForTeamCompetitor(string leagueName, string teamName, string playerName)
        {
            try
            {
                var request = new GetPlayerForTeamCompetitorQuery
                {
                    LeagueName = leagueName,
                    TeamName = teamName,
                    PlayerName = playerName
                };
                var player = await mediator.Send(request);
                return Ok(player);
            }
            catch (TeamLeagueNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TeamNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        [HttpPost("teamleague/{leagueName}/competitor/players")]
        public async Task<IActionResult> AddPlayerToTeamCompetitor(string leagueName, AddPlayerToTeamCompetitorCommand command)
        {
            try
            {
                await mediator.Send(command);
                return Created($"/teamLeague/{leagueName}/competitor/{command.TeamName}/player/{command.PlayerName}", command);
            }
            catch (TeamLeagueNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TeamNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PlayerNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
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
            catch (Exception)
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

        // GET api/competition/teamleague/Premier League 2019-2020/match/guid
        [HttpGet("teamleague/{name}/match/details/{guid}")]
        public async Task<IActionResult> GetTeamLeagueMatchDetails(string name, string guid)
        {
            try
            {
                var match = await mediator.Send(new GetTeamLeagueMatchDetailsQuery
                {
                    LeagueName = name,
                    Guid = new Guid(guid)
                });
                return Ok(match);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong!");
            }
        }

        // GET api/competition/teamleague/Premier League 2019-2020/match/{matchGuid}/lineup/{lineupPlayerGuid}
        [HttpGet("teamleague/{name}/match/{matchGuid}/lineup/{lineupPlayerGuid}")]
        public async Task<IActionResult> GetTeamLeagueMatchLineupPlayer(string name, string matchGuid, string lineupPlayerGuid)
        {
            try
            {
                var lineupPlayer = await mediator.Send(new GetTeamLeagueMatchLineupEntryQuery
                {
                    LeagueName = name,
                    MatchGuid = new Guid(matchGuid),
                    LineupEntryGuid = new Guid(lineupPlayerGuid)
                });
                return Ok(lineupPlayer);
            }
            catch (Exception)
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
            catch (LeagueManagerException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        // PUT api/competition/teamleague/Premier League 2019-2020/match/{guid}/score
        [HttpPut("teamleague/{leagueName}/match/{guid}/score")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTeamLeagueMatchScore(string leagueName, Guid guid, [FromBody]UpdateTeamLeagueMatchScoreCommand command)
        {
            try
            {
                command.LeagueName = leagueName;
                command.Guid = guid;

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
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        // PUT api/competition/teamleague/Premier League 2019-2020/match/{guid}/{team}/lineup
        [HttpPut("teamleague/{name}/match/{guid}/{team}/lineup")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddPlayerToLineup(string name, string guid, string team, [FromBody]AddPlayerToLineupDto dto)
        {
            try
            {
                var command = mapper.Map<AddPlayerToLineupCommand>(dto, opt =>
                {
                    opt.Items["leagueName"] = name;
                    opt.Items["guid"] = guid;
                });

                var match = await mediator.Send(command);
                return Ok(match);
            }
            catch (LeagueManagerException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong!");
            }
        }

        // PUT api/competition/teamleague/Premier League 2019-2020/match/{guid}/{team}/lineup
        [HttpPut("teamleague/{leagueName}/match/{matchGuid}/{teamName}/lineup/{lineupEntryGuid}")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTeamLeagueMatchLineupEntry(string leagueName, Guid matchGuid, string teamName, Guid lineupEntryGuid, [FromBody]UpdateTeamLeagueMatchLineupEntryCommand command)
        {
            try
            {
                var match = await mediator.Send(command);
                return Ok(match);
            }
            catch (LeagueManagerException ex)
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