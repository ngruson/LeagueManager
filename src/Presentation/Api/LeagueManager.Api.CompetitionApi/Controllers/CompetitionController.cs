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
using AutoMapper;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.Match.Commands.AddPlayerToLineup;
using LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Commands;
using Microsoft.Extensions.Logging;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal;

namespace LeagueManager.Api.CompetitionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ILogger<CompetitionController> logger;

        public CompetitionController(
            IMediator mediator,
            IMapper mapper,
            ILogger<CompetitionController> logger)
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
            catch (PlayerNotFoundException ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }

        // GET api/competition/teamleague/Premier League 2019-2020/match/guid
        [HttpGet("teamleague/{name}/match/{guid}")]
        public async Task<IActionResult> GetTeamLeagueMatch(string name, Guid guid)
        {
            try
            {
                var match = await mediator.Send(new GetTeamLeagueMatchQuery
                {
                    LeagueName = name,
                    Guid = guid
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
        public async Task<IActionResult> GetTeamLeagueMatchDetails(string name, Guid guid)
        {
            try
            {
                var match = await mediator.Send(new GetTeamLeagueMatchDetailsQuery
                {
                    LeagueName = name,
                    Guid = guid
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
        public async Task<IActionResult> GetTeamLeagueMatchLineupEntry(string name, Guid matchGuid, Guid lineupPlayerGuid)
        {
            try
            {
                var lineupPlayer = await mediator.Send(new GetTeamLeagueMatchLineupEntryQuery
                {
                    LeagueName = name,
                    MatchGuid = matchGuid,
                    LineupEntryGuid = lineupPlayerGuid
                });
                return Ok(lineupPlayer);
            }
            catch (LineupEntryNotFoundException ex)
            {
                return BadRequest(ex.Message);
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
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        // POST api/competition/teamleague/Premier League 2019-2020/match/{guid}/{team}/lineup
        [HttpPost("teamleague/{name}/match/{guid}/{team}/lineup")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddPlayerToLineup(string name, Guid guid, string team, [FromBody]AddPlayerToLineupDto dto)
        {
            try
            {
                var command = mapper.Map<AddPlayerToLineupCommand>(dto, opt =>
                {
                    opt.Items["leagueName"] = name;
                    opt.Items["guid"] = guid;
                    opt.Items["teamName"] = team;
                });

                var match = await mediator.Send(command);
                return Ok(match);
            }
            catch (TeamLeagueNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MatchNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MatchEntryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PlayerNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }

        // PUT api/competition/teamleague/Premier League 2019-2020/match/{guid}/{team}/lineup
        [HttpPut("teamleague/{leagueName}/match/{matchGuid}/{teamName}/lineup/{lineupEntryGuid}")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTeamLeagueMatchLineupEntry(string leagueName, Guid matchGuid, string teamName, Guid lineupEntryGuid, [FromBody]UpdateLineupEntryDto dto)
        {
            try
            {
                var command = mapper.Map<UpdateTeamLeagueMatchLineupEntryCommand>(dto, opt =>
                {
                    opt.Items["leagueName"] = leagueName;
                    opt.Items["matchGuid"] = matchGuid;
                    opt.Items["teamName"] = teamName;
                    opt.Items["lineupEntryGuid"] = lineupEntryGuid;
                });

                var lineupEntry = await mediator.Send(command);
                return Ok(lineupEntry);
            }
            catch (LineupEntryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        // POST api/competition/teamleague/Premier League 2019-2020/match/{guid}/{team}/goals
        [HttpPost("teamleague/{leagueName}/match/{matchGuid}/{teamName}/goals")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddTeamLeagueMatchGoal(string leagueName, Guid matchGuid, string teamName, [FromBody]AddTeamLeagueMatchGoalDto dto)
        {
            string methodName = "AddTeamLeagueMatchGoal";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var command = mapper.Map<AddTeamLeagueMatchGoalCommand>(dto, opt =>
                {
                    opt.Items["leagueName"] = leagueName;
                    opt.Items["matchGuid"] = matchGuid;
                    opt.Items["teamName"] = teamName;
                });

                logger.LogInformation("{methodName}: Sending command {command}", methodName, command);
                var goal = await mediator.Send(command);
                logger.LogInformation("{methodName}: Returning goal {goal}", methodName, goal);
                return Ok(goal);
            }
            catch (TeamLeagueNotFoundException ex)
            {
                logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (MatchNotFoundException ex)
            {
                logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (MatchEntryNotFoundException ex)
            {
                logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (PlayerNotFoundException ex)
            {
                logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
                return BadRequest("Something went wrong!");
            }
        }

        [HttpGet("teamleague/{leagueName}/match/{matchGuid}/goal/{goalGuid}")]
        public async Task<IActionResult> GetTeamLeagueMatchGoal(string leagueName, Guid matchGuid, Guid goalGuid)
        {
            try
            {
                var goal = await mediator.Send(new GetTeamLeagueMatchGoalQuery
                {
                    LeagueName = leagueName,
                    MatchGuid = matchGuid,
                    GoalGuid = goalGuid
                });
                return Ok(goal);
            }
            catch (GoalNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }

        [HttpPut("teamleague/{leagueName}/match/{matchGuid}/team/{teamName}/goal/{goalGuid}")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTeamLeagueMatchGoal(string leagueName, Guid matchGuid, string teamName, Guid goalGuid, [FromBody]UpdateTeamLeagueMatchGoalDto dto)
        {
            string methodName = "UpdateTeamLeagueMatchGoal";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var command = mapper.Map<UpdateTeamLeagueMatchGoalCommand>(dto, opt =>
                {
                    opt.Items["leagueName"] = leagueName;
                    opt.Items["matchGuid"] = matchGuid;
                    opt.Items["teamName"] = teamName;
                    opt.Items["goalGuid"] = goalGuid;
                });

                logger.LogInformation($"{methodName}: Sending command {command}");
                var goal = await mediator.Send(command);
                logger.LogInformation($"{methodName}: Returning goal {goal}");
                return Ok(goal);
            }
            catch (TeamLeagueNotFoundException ex)
            {
                logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (MatchNotFoundException ex)
            {
                logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (MatchEntryNotFoundException ex)
            {
                logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (PlayerNotFoundException ex)
            {
                logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
                return BadRequest("Something went wrong!");
            }
        }

        [HttpGet("teamleague/{leagueName}/match/{matchGuid}/{teamName}/events")]
        public async Task<IActionResult> GetMatchEvents(string leagueName, Guid matchGuid, string teamName)
        {
            string methodName = nameof(GetMatchEvents);
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                logger.LogInformation($"{methodName}: Retrieving match events");
                var events = await mediator.Send(new GetTeamLeagueMatchEventsQuery
                {
                    LeagueName = leagueName,
                    MatchGuid = matchGuid,
                    TeamName = teamName
                });

                logger.LogInformation($"{methodName}: Returning events");
                return Ok(events);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
                return BadRequest("Something went wrong!");
            }
        }
    }
}