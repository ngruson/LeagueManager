using LeagueManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using LeagueManager.Infrastructure.Api;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using System.Linq;
using System;
using System.Net;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueCompetitors;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.WebUI.ViewModels;
using System.IO;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution;

namespace LeagueManager.WebUI.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly ApiSettings apiSettings;
        private readonly ISportApi sportApi;
        private readonly ICountryApi countryApi;
        private readonly ITeamApi teamApi;
        private readonly ICompetitionApi competitionApi;

        public CompetitionController(
            IOptions<ApiSettings> apiSettings,
            ISportApi sportApi,
            ICountryApi countryApi,
            ITeamApi teamApi,
            ICompetitionApi competitionApi
        )
        {
            this.apiSettings = apiSettings.Value;
            this.sportApi = sportApi;
            this.countryApi = countryApi;
            this.teamApi = teamApi;
            this.competitionApi = competitionApi;
        }

        public async Task<IActionResult> Index()
        {
            var competitions = await competitionApi.GetCompetitions(
                new GetCompetitionsQuery()
            );

            return View(competitions);
        }

        [HttpGet("CreateTeamLeague")]
        public async Task<IActionResult> CreateTeamLeague()
        {
            var viewModel = new CreateTeamLeagueViewModel
            {
                AllTeams = await teamApi.GetTeams(),
                TeamSports = await sportApi.GetTeamSports(),
                Countries = await countryApi.GetCountries()
            };

            ViewData["TeamApiUrl"] = apiSettings.TeamApiUrl;
            ViewData["CountryApiUrl"] = apiSettings.CountryApiUrl;

            return View(viewModel);
        }

        [HttpPost("CreateTeamLeague")]
        public async Task<IActionResult> CreateTeamLeague(CreateTeamLeagueViewModel vm)
        {
            using (var memoryStream = new MemoryStream())
            {
                vm.LogoFormFile.CopyTo(memoryStream);
                vm.Logo = memoryStream.ToArray();
            }

            await competitionApi.CreateTeamLeague(vm);
            return RedirectToAction("Index");
        }

        //[Route("[controller]/{leagueName}", Name = "ViewTeamLeague")]
        [Route("[controller]/{leagueName}")]
        public async Task<IActionResult> ViewTeamLeague(string leagueName)
        {
            var vm = await competitionApi.GetTeamLeague(leagueName);
            return View(vm);
        }

        [Route("[controller]/{leagueName}/viewTeamLeagueTable")]
        public async Task<IActionResult> ViewTeamLeagueTable(string leagueName)
        {
            var table = await competitionApi.GetTeamLeagueTable(
                new GetTeamLeagueTableQuery
                {
                    LeagueName = leagueName
                }
            );

            return PartialView(table);
        }

        [Route("[controller]/{leagueName}/matches", Name = "ViewMatches")]
        public async Task<IActionResult> ViewMatches(string leagueName, string round)
        {
            var vm = await competitionApi.GetTeamLeagueRounds(
                new GetTeamLeagueRoundsQuery
                {
                    LeagueName = leagueName
                });

            return PartialView(vm);
        }

        [Route("[controller]/{leagueName}/matches/{guid}")]
        public async Task<IActionResult> ViewMatch(string leagueName, string guid)
        {
            var match = await competitionApi.GetTeamLeagueMatch(
                new GetTeamLeagueMatchQuery
                {
                    LeagueName = leagueName,
                    Guid = new Guid(guid)
                }
            );

            return PartialView(match);
        }

        [Route("[controller]/{leagueName}/editmatch/{guid}")]
        public async Task<IActionResult> EditMatch(string leagueName, string guid)
        {
            var competitors = await competitionApi.GetCompetitors(
                new GetTeamLeagueCompetitorsQuery
                {
                    LeagueName = leagueName
                });
            ViewData["Teams"] = competitors;

            var match = await competitionApi.GetTeamLeagueMatch(
                new GetTeamLeagueMatchQuery
                {
                    LeagueName = leagueName,
                    Guid = new Guid(guid)
                }
            );

            return PartialView(match);
        }

        [HttpPut("[controller]/{leagueName}/matches/{guid}")]
        public async Task<IActionResult> UpdateTeamLeagueMatch(string leagueName, Guid guid, UpdateTeamLeagueMatchDto dto)
        {
            var match = await competitionApi.UpdateTeamLeagueMatch(
                new UpdateTeamLeagueMatchCommand
                {
                    LeagueName = WebUtility.HtmlDecode(leagueName),
                    Guid = guid,
                    HomeTeam = dto.HomeTeam,
                    AwayTeam = dto.AwayTeam,
                    StartTime = dto.StartTime
                });

            return PartialView("ViewMatch", match);
        }

        [HttpPut("[controller]/{leagueName}/matches/{guid}/score")]
        public async Task<IActionResult> UpdateTeamLeagueMatchScore(string leagueName, Guid guid, UpdateTeamLeagueMatchScoreDto dto)
        {
            var match = await competitionApi.UpdateTeamLeagueMatchScore(leagueName, guid, dto);
            return PartialView("ViewMatch", match);
        }

        [HttpGet("[controller]/{leagueName}/matches/{guid}/score")]
        public async Task<IActionResult> SetScore(string leagueName, string guid)
        {
            var match = await competitionApi.GetTeamLeagueMatch(
               new GetTeamLeagueMatchQuery
               {
                   LeagueName = leagueName,
                   Guid = new Guid(guid)
               }
           );

            return PartialView(match);
        }

        [HttpGet("[controller]/{leagueName}/matches/{guid}/details")]
        public async Task<IActionResult> ViewMatchDetails(string leagueName, string guid)
        {
            var match = await competitionApi.GetTeamLeagueMatchDetails(
                new GetTeamLeagueMatchDetailsQuery
                {
                    LeagueName = leagueName,
                    Guid = new Guid(guid)
                }
            );

            ViewData["TeamLeagueApiUrl"] = apiSettings.TeamLeagueApiUrl;
            ViewData["PlayerApiUrl"] = apiSettings.PlayerApiUrl;
            ViewData["GetPlayerBaseUrl"] = $"{apiSettings.TeamLeagueApiUrl}/{leagueName}/competitors";
            ViewData["MatchBaseUrl"] = $"{apiSettings.TeamLeagueApiUrl}/{leagueName}/matches/{guid}";
            return View(match);
        }

        [HttpGet("[controller]/{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/lineup/{lineupEntryGuid}")]
        public async Task<IActionResult> ViewMatchLineupEntry(string leagueName, Guid matchGuid, string teamName, Guid lineupEntryGuid)
        {
            var lineupEntry = await competitionApi.GetTeamLeagueMatchLineupEntry(
                new GetTeamLeagueMatchLineupEntryQuery
                {
                    LeagueName = leagueName,
                    MatchGuid = matchGuid,
                    LineupEntryGuid = lineupEntryGuid
                }
            );

            return PartialView(lineupEntry);
        }

        [HttpGet("[controller]/{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/lineup/{lineupEntryGuid}/edit")]
        public async Task<IActionResult> EditMatchLineupEntry(string leagueName, Guid matchGuid, string teamName, Guid lineupEntryGuid)
        {
            var lineupEntry = await competitionApi.GetTeamLeagueMatchLineupEntry(
                new GetTeamLeagueMatchLineupEntryQuery
                {
                    LeagueName = leagueName,
                    MatchGuid = matchGuid,
                    LineupEntryGuid = lineupEntryGuid
                }
            );

            var players = await competitionApi.GetPlayersForTeamCompetitor(new GetPlayersForTeamCompetitorQuery
            {
                LeagueName = leagueName,
                TeamName = teamName
            });
            ViewData["Players"] = players.Select(p => p.Player);

            return PartialView(lineupEntry);
        }

        [HttpPut("[controller]/{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/lineup/{lineupEntryGuid}")]
        public async Task<IActionResult> UpdateMatchLineupEntry(string leagueName, Guid matchGuid, string teamName, Guid lineupEntryGuid, UpdateTeamLeagueMatchLineupEntryCommand command)
        {
            command.LeagueName = WebUtility.HtmlDecode(leagueName);
            command.MatchGuid = matchGuid;
            command.TeamName = teamName;
            command.LineupEntryGuid = lineupEntryGuid;

            var lineupEntry = await competitionApi.UpdateTeamLeagueMatchLineupEntry(command);

            return PartialView("ViewMatchLineupEntry", lineupEntry);
        }

        [HttpGet("[controller]/{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/events")]
        public async Task<IActionResult> ViewTeamMatchEvents(string leagueName, Guid matchGuid, string teamName)
        {
            var events = await competitionApi.GetTeamLeagueMatchEvents(
                    new GetTeamLeagueMatchEventsQuery
                    {
                        LeagueName = leagueName,
                        MatchGuid = matchGuid,
                        TeamName = teamName
                    }
                );

            return PartialView(events);
        }

        [HttpGet("[controller]/{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/goals/{goalGuid}/edit")]
        public async Task<IActionResult> EditMatchGoal(string leagueName, Guid matchGuid, string teamName, Guid goalGuid)
        {
            var goal = await competitionApi.GetTeamLeagueMatchGoal(
                new GetTeamLeagueMatchGoalQuery
                {
                    LeagueName = leagueName,
                    MatchGuid = matchGuid,
                    GoalGuid = goalGuid
                }
            );

            var players = await competitionApi.GetPlayersForTeamCompetitor(new GetPlayersForTeamCompetitorQuery
            {
                LeagueName = leagueName,
                TeamName = teamName
            });
            ViewData["Players"] = players.Select(p => p.Player);

            return PartialView(goal);
        }

        [HttpPut("[controller]/{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/goals/{goalGuid}")]
        public async Task<IActionResult> UpdateMatchGoal(string leagueName, Guid matchGuid, string teamName, Guid goalGuid, UpdateTeamLeagueMatchGoalCommand command)
        {
            var goal = await competitionApi.UpdateTeamLeagueMatchGoal(
                new UpdateTeamLeagueMatchGoalCommand
                {
                    LeagueName = WebUtility.HtmlDecode(leagueName),
                    MatchGuid = matchGuid,
                    TeamName = teamName,
                    GoalGuid = goalGuid
                });

            return PartialView("ViewGoalMatchEvent", goal);
        }

        [HttpGet("[controller]/{leagueName}/matches/{matchGuid}/goals/{goalGuid}")]
        public async Task<IActionResult> ViewGoalMatchEvent(string leagueName, Guid matchGuid, Guid goalGuid)
        {
            var goal = await competitionApi.GetTeamLeagueMatchGoal(
                new GetTeamLeagueMatchGoalQuery
                {
                    LeagueName = leagueName,
                    MatchGuid = matchGuid,
                    GoalGuid = goalGuid
                }
            );

            return PartialView(goal);
        }

        [HttpGet("[controller]/{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/substitutions/{substitutionGuid}/edit")]
        public async Task<IActionResult> EditMatchSubstitution(string leagueName, Guid matchGuid, string teamName, Guid substitutionGuid)
        {
            var sub = await competitionApi.GetTeamLeagueMatchSubstitution(
                new GetTeamLeagueMatchSubstitutionQuery
                {
                    LeagueName = leagueName,
                    MatchGuid = matchGuid,
                    TeamName = teamName,
                    SubstitutionGuid = substitutionGuid
                }
            );

            var players = await competitionApi.GetPlayersForTeamCompetitor(new GetPlayersForTeamCompetitorQuery
            {
                LeagueName = leagueName,
                TeamName = teamName
            });
            ViewData["Players"] = players.Select(p => p.Player);

            return PartialView(sub);
        }

        [HttpPut("[controller]/{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/substitutions/{substitutionGuid}")]
        public async Task<IActionResult> UpdateMatchSubstitution(string leagueName, Guid matchGuid, string teamName, Guid substitutionGuid, UpdateTeamLeagueMatchSubstitutionDto dto)
        {
            var sub = await competitionApi.UpdateTeamLeagueMatchSubstitution(
                leagueName,
                matchGuid,
                teamName,
                substitutionGuid,
                dto
            );

            return PartialView("ViewSubstitutionMatchEvent", sub);
        }

        [HttpGet("[controller]/{leagueName}/matches/{matchGuid}/matchEntries/{teamName}/substitutions/{substitutionGuid}")]
        public async Task<IActionResult> ViewSubstitutionMatchEvent(string leagueName, Guid matchGuid, string teamName, Guid substitutionGuid)
        {
            var sub = await competitionApi.GetTeamLeagueMatchSubstitution(
                new GetTeamLeagueMatchSubstitutionQuery
                {
                    LeagueName = leagueName,
                    MatchGuid = matchGuid,
                    TeamName = teamName,
                    SubstitutionGuid = substitutionGuid
                }
            );

            return PartialView(sub);
        }
    }
}