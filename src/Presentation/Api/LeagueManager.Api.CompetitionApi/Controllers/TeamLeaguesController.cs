using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Match.Commands.AddPlayerToLineup;
using LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeague;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueCompetitors;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LeagueManager.Api.CompetitionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamLeaguesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<TeamLeaguesController> logger;
        private readonly IMapper mapper;

        public TeamLeaguesController(IMediator mediator, ILogger<TeamLeaguesController> logger, IMapper mapper) => 
            (this.mediator, this.logger, this.mapper) = (mediator, logger, mapper);

        private IActionResult LogException(string methodName, Exception ex)
        {
            logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
            return BadRequest(ex.Message);
        }

        private IActionResult LogException(string methodName, Exception ex, string error)
        {
            logger.LogError(ex, $"{methodName}: Exception {ex.GetType().Name} thrown: {ex.Message}");
            return BadRequest(error);
        }

        [HttpGet("{leagueName}")]
        public async Task<IActionResult> GetTeamLeague(string leagueName)
        {
            string methodName = nameof(GetTeamLeague);
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var query = new GetTeamLeagueQuery { LeagueName = leagueName };
                logger.LogInformation("{methodName}: Sending query {query}", methodName, query);
                var teamLeague = await mediator.Send(query);
                logger.LogInformation("{methodName}: Returning team league {teamLeague}", methodName, teamLeague);
                return Ok(teamLeague);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpGet("{leagueName}/competitors")]
        public async Task<IActionResult> GetCompetitors(string leagueName)
        {
            string methodName = nameof(GetCompetitors);
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var query = new GetTeamLeagueCompetitorsQuery
                {
                    LeagueName = leagueName,
                };

                logger.LogInformation("{methodName}: Sending query {query}", methodName, query);                
                var competitors = await mediator.Send(query);
                logger.LogInformation("{methodName}: Returning competitors {competitors}", methodName, competitors);
                return Ok(competitors);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpGet("{leagueName}/rounds")]
        public async Task<IActionResult> GetTeamLeagueRounds(string leagueName)
        {
            string methodName = "GetTeamLeagueRounds";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var query = new GetTeamLeagueRoundsQuery { LeagueName = leagueName };

                logger.LogInformation("{methodName}: Sending query {query}", methodName, query);
                var rounds = await mediator.Send(query);
                logger.LogInformation("{methodName}: Returning rounds {rounds}", methodName, rounds);
                return Ok(rounds);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpGet("{leagueName}/table")]
        public async Task<IActionResult> GetTeamLeagueTable(string leagueName)
        {
            string methodName = nameof(GetTeamLeagueTable);
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var query = new GetTeamLeagueTableQuery { LeagueName = leagueName };

                logger.LogInformation("{methodName}: Sending query {query}", methodName, query);
                var table = await mediator.Send(query);
                logger.LogInformation("{methodName}: Returning table {table}", methodName, table);
                return Ok(table);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpGet("{leagueName}/competitors/{teamName}/players")]
        public async Task<IActionResult> GetPlayersForTeamCompetitor(string leagueName, string teamName)
        {
            string methodName = "GetPlayersForTeamCompetitor";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var query = new GetPlayersForTeamCompetitorQuery
                {
                    LeagueName = leagueName,
                    TeamName = teamName
                };

                logger.LogInformation("{methodName}: Sending query {query}", methodName, query);
                var players = await mediator.Send(query);
                logger.LogInformation("{methodName}: Returning players {players}", methodName, players);
                return Ok(players);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpGet("{leagueName}/competitors/{teamName}/players/{playerName}")]
        public async Task<IActionResult> GetPlayerForTeamCompetitor(string leagueName, string teamName, string playerName)
        {
            string methodName = "GetPlayerForTeamCompetitor";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var query = new GetPlayerForTeamCompetitorQuery
                {
                    LeagueName = leagueName,
                    TeamName = teamName,
                    PlayerName = playerName
                };

                logger.LogInformation("{methodName}: Sending query {query}", methodName, query);
                var player = await mediator.Send(query);
                logger.LogInformation("{methodName}: Returning player {player}", methodName, player);
                return Ok(player);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpPost("{leagueName}/competitors/players")]
        public async Task<IActionResult> AddPlayerToTeamCompetitor(string leagueName, AddPlayerToTeamCompetitorCommand command)
        {
            string methodName = "AddPlayerToTeamCompetitor";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                logger.LogInformation("{methodName}: Sending command {command}", methodName, command);
                await mediator.Send(command);
                logger.LogInformation("{methodName}: Added player '{player}' to team '{team}'", methodName, command.PlayerName, command.TeamName);
                return Created($"/teamLeague/{leagueName}/competitor/{command.TeamName}/player/{command.PlayerName}", command);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpGet("{leagueName}/matches/{guid}")]
        public async Task<IActionResult> GetTeamLeagueMatch(string leagueName, Guid guid)
        {
            string methodName = "GetTeamLeagueMatch";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var query = new GetTeamLeagueMatchQuery
                {
                    LeagueName = leagueName,
                    Guid = guid
                };

                logger.LogInformation("{methodName}: Sending query {query}", methodName, query);
                var match = await mediator.Send(query);
                logger.LogInformation("{methodName}: Returning match {match}", methodName, match);
                return Ok(match);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpPut("{leagueName}/matches/{guid}")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTeamLeagueMatch(string leagueName, Guid guid, [FromBody]UpdateTeamLeagueMatchCommand command)
        {
            string methodName = "UpdateTeamLeagueMatch";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                command.LeagueName = leagueName;
                command.Guid = guid;

                logger.LogInformation("{methodName}: Sending command {command}", methodName, command);
                var match = await mediator.Send(command);
                logger.LogInformation("{methodName}: Returning match {match}", methodName, match);
                return Ok(match);
            }
            catch(LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpPut("{leagueName}/matches/{guid}/score")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTeamLeagueMatchScore(string leagueName, Guid guid, [FromBody]UpdateTeamLeagueMatchScoreDto dto)
        {
            string methodName = "UpdateTeamLeagueMatchScore";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var command = mapper.Map<UpdateTeamLeagueMatchScoreCommand>(dto);
                command.LeagueName = leagueName;
                command.Guid = guid;

                logger.LogInformation("{methodName}: Sending command {command}", methodName, command);
                var match = await mediator.Send(command);
                logger.LogInformation("{methodName}: Returning match {match}", methodName, match);
                return Ok(match);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpGet("{leagueName}/matches/{guid}/details/")]
        public async Task<IActionResult> GetTeamLeagueMatchDetails(string leagueName, Guid guid)
        {
            string methodName = "GetTeamLeagueMatchDetails";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var query = new GetTeamLeagueMatchDetailsQuery
                {
                    LeagueName = leagueName,
                    Guid = guid
                };

                logger.LogInformation("{methodName}: Sending query {query}", methodName, query);
                var match = await mediator.Send(query);
                logger.LogInformation("{methodName}: Returning match {match}", methodName, match);
                return Ok(match);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }        

        [HttpGet("{leagueName}/matches/{matchGuid}/lineup/{lineupPlayerGuid}")]
        public async Task<IActionResult> GetTeamLeagueMatchLineupEntry(string leagueName, Guid matchGuid, Guid lineupPlayerGuid)
        {
            string methodName = "GetTeamLeagueMatchLineupEntry";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var query = new GetTeamLeagueMatchLineupEntryQuery
                {
                    LeagueName = leagueName,
                    MatchGuid = matchGuid,
                    LineupEntryGuid = lineupPlayerGuid
                };

                logger.LogInformation("{methodName}: Sending query {query}", methodName, query);
                var lineupEntry = await mediator.Send(query);
                logger.LogInformation("{methodName}: Returning lineup entry {lineupEntry}", methodName, lineupEntry);
                return Ok(lineupEntry);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpPost("{leagueName}/matches/{guid}/matchEntries/{team}/lineup")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddPlayerToLineup(string leagueName, Guid guid, string team, [FromBody]AddPlayerToLineupCommand command)
        {
            string methodName = "AddPlayerToLineup";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                command.LeagueName = leagueName;
                command.Guid = guid;
                command.Team = team;

                logger.LogInformation("{methodName}: Sending command {command}", methodName, command);
                var result = await mediator.Send(command);
                logger.LogInformation("{methodName}: Added player '{player}'", methodName, command.Player);
                return Ok(result);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpPut("{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/lineup/{lineupEntryGuid}")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTeamLeagueMatchLineupEntry(string leagueName, Guid matchGuid, string teamName, Guid lineupEntryGuid, [FromBody]UpdateTeamLeagueMatchLineupEntryCommand command)
        {
            string methodName = "UpdateTeamLeagueMatchLineupEntry";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                command.LeagueName = leagueName;
                command.MatchGuid = matchGuid;
                command.TeamName = teamName;
                command.LineupEntryGuid = lineupEntryGuid;

                logger.LogInformation("{methodName}: Sending command {command}", methodName, command);
                var lineupEntry = await mediator.Send(command);
                logger.LogInformation("{methodName}: Returning lineup entry {lineupEntry}", methodName, lineupEntry);
                return Ok(lineupEntry);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpPost("{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/goals")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddTeamLeagueMatchGoal(string leagueName, Guid matchGuid, string teamName, [FromBody]AddTeamLeagueMatchGoalCommand command)
        {
            string methodName = nameof(AddTeamLeagueMatchGoal);
            logger.LogInformation($"{methodName}: Request received");

            command.LeagueName = leagueName;
            command.MatchGuid = matchGuid;
            command.TeamName = teamName;

            try
            {
                logger.LogInformation("{methodName}: Sending command {command}", methodName, command);
                var goal = await mediator.Send(command);
                logger.LogInformation("{methodName}: Returning goal {goal}", methodName, goal);
                return Ok(goal);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpGet("{leagueName}/matches/{matchGuid}/goals/{goalGuid}")]
        public async Task<IActionResult> GetTeamLeagueMatchGoal(string leagueName, Guid matchGuid, Guid goalGuid)
        {
            string methodName = "GetTeamLeagueMatchGoal";
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var query = new GetTeamLeagueMatchGoalQuery
                {
                    LeagueName = leagueName,
                    MatchGuid = matchGuid,
                    GoalGuid = goalGuid
                };

                logger.LogInformation("{methodName}: Sending query {query}", methodName, query);
                var goal = await mediator.Send(query);
                logger.LogInformation("{methodName}: Returning goal {goal}", methodName, goal);
                return Ok(goal);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpPut("{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/goals/{goalGuid}")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTeamLeagueMatchGoal(string leagueName, Guid matchGuid, string teamName, Guid goalGuid, [FromBody]UpdateTeamLeagueMatchGoalCommand command)
        {
            string methodName = "UpdateTeamLeagueMatchGoal";
            logger.LogInformation($"{methodName}: Request received");            

            try
            {
                command.LeagueName = leagueName;
                command.MatchGuid = matchGuid;
                command.TeamName = teamName;
                command.GoalGuid = goalGuid;

                logger.LogInformation($"{methodName}: Sending command {command}");
                var goal = await mediator.Send(command);
                logger.LogInformation($"{methodName}: Returning goal {goal}");
                return Ok(goal);
            }
            catch(LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }

        [HttpGet("{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/events")]
        public async Task<IActionResult> GetMatchEvents(string leagueName, Guid matchGuid, string teamName)
        {
            string methodName = nameof(GetMatchEvents);
            logger.LogInformation($"{methodName}: Request received");

            try
            {
                var query = new GetTeamLeagueMatchEventsQuery
                {
                    LeagueName = leagueName,
                    MatchGuid = matchGuid,
                    TeamName = teamName
                };

                logger.LogInformation($"{methodName}: Retrieving match events");
                var events = await mediator.Send(query);
                logger.LogInformation($"{methodName}: Returning events");
                return Ok(events);
            }
            catch (LeagueManagerException ex)
            {
                return LogException(methodName, ex);
            }
            catch (Exception ex)
            {
                return LogException(methodName, ex, "Something went wrong!");
            }
        }
    }
}