﻿using AutoMapper;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueManager.Infrastructure.Api;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using System.Linq;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using System;
using System.Net;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupPlayer;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry;

namespace LeagueManager.WebUI.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly ApiSettings apiSettings;
        private readonly ISportApi sportApi;
        private readonly ICountryApi countryApi;
        private readonly ITeamApi teamApi;
        private readonly ICompetitionApi competitionApi;
        private readonly IMapper mapper;

        public CompetitionController(
            IOptions<ApiSettings> apiSettings,
            ISportApi sportApi,
            ICountryApi countryApi,
            ITeamApi teamApi,
            ICompetitionApi competitionApi,
            IMapper mapper)
        {
            this.apiSettings = apiSettings.Value;
            this.sportApi = sportApi;
            this.countryApi = countryApi;
            this.teamApi = teamApi;
            this.competitionApi = competitionApi;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var competitions = await competitionApi.GetCompetitions(
                new GetCompetitionsQuery()
            );
            var viewModel = mapper.Map<IEnumerable<CompetitionViewModel>>(competitions);

            return View(viewModel);
        }

        public async Task<IActionResult> CreateTeamLeague()
        {
            var viewModel = new CreateTeamLeagueViewModel
            {
                AllTeams = await teamApi.GetTeams(),
                SelectedTeams = new List<Application.Teams.Queries.GetTeams.TeamDto>(),
                TeamApiUrl = apiSettings.TeamApiUrl,
                CountryApiUrl = apiSettings.CountryApiUrl,
                TeamSports = await sportApi.GetTeamSports(),
                Countries = await countryApi.GetCountries()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeamLeague(CreateTeamLeagueViewModel model)
        {
            var command = mapper.Map<CreateTeamLeagueCommand>(model);
            await competitionApi.CreateTeamLeague(command);
            return RedirectToAction("Index");
        }

        [Route("{controller}/{leagueName}", Name = "ViewTeamLeague")]
        public async Task<IActionResult> ViewTeamLeague(string leagueName)
        {
            var table = await competitionApi.GetTeamLeagueTable(
                new GetTeamLeagueTableQuery
                {
                    LeagueName = leagueName
                });
            var rounds = await competitionApi.GetTeamLeagueRounds(
                new GetTeamLeagueRoundsQuery
                {
                    LeagueName = leagueName
                });

            var viewModel = new ViewTeamLeagueViewModel
            {
                Name = leagueName,
                Table = mapper.Map<TeamLeagueTableViewModel>(table),
                Rounds = mapper.Map<IEnumerable<TeamLeagueRoundViewModel>>(rounds),
            };

            viewModel.SelectedRound = viewModel.Rounds.ToList()[0].Name;
            return View(viewModel);
        }

        [Route("{controller}/{leagueName}/viewTeamLeagueTable")]
        public async Task<IActionResult> ViewTeamLeagueTable(string leagueName)
        {
            var table = await competitionApi.GetTeamLeagueTable(
                new GetTeamLeagueTableQuery
                {
                    LeagueName = leagueName
                }
            );

            var viewModel = mapper.Map<TeamLeagueTableViewModel>(table);
            return PartialView(viewModel);
        }

        [Route("{controller}/{leagueName}/matches", Name = "ViewMatches")]
        public async Task<IActionResult> ViewMatches(string leagueName, string round)
        {
            var rounds = await competitionApi.GetTeamLeagueRounds(
                new GetTeamLeagueRoundsQuery
                {
                    LeagueName = leagueName
                });

            var viewModel = new ViewTeamLeagueViewModel
            {
                Name = leagueName,
                Rounds = mapper.Map<IEnumerable<TeamLeagueRoundViewModel>>(rounds),
                SelectedRound = round
            };

            return PartialView(viewModel);
        }

        [Route("{controller}/{leagueName}/viewmatch/{guid}")]
        public async Task<IActionResult> ViewMatch(string leagueName, string guid)
        {
            var match = await competitionApi.GetTeamLeagueMatch(
                new GetTeamLeagueMatchQuery
                {
                    LeagueName = leagueName,
                    Guid = new Guid(guid)
                }
            );

            var viewModel = mapper.Map<TeamMatchViewModel>(match);
            return PartialView(viewModel);
        }

        [Route("{controller}/{leagueName}/editmatch/{guid}")]
        public async Task<IActionResult> EditMatch(string leagueName, string guid)
        {
            var teamLeague = await competitionApi.GetCompetition(
                new GetCompetitionQuery
                {
                    Name = leagueName
                });
            ViewData["Teams"] = teamLeague.Competitors
                .Select(c => new TeamViewModel { Name = c })
                .OrderBy(t => t.Name);

            var match = await competitionApi.GetTeamLeagueMatch(
                new GetTeamLeagueMatchQuery
                {
                    LeagueName = leagueName,
                    Guid = new Guid(guid)
                }
            );

            var viewModel = mapper.Map<TeamMatchViewModel>(match);
            return PartialView(viewModel);
        }

        [HttpPut("{controller}/{leagueName}/match/{guid}")]
        public async Task<IActionResult> UpdateTeamLeagueMatch(string leagueName, string guid, UpdateTeamLeagueMatchDto dto)
        {
            var match = await competitionApi.UpdateTeamLeagueMatch(
                new UpdateTeamLeagueMatchCommand
                {
                    LeagueName = WebUtility.HtmlDecode(leagueName),
                    Guid = new Guid(guid),
                    HomeTeam = dto.HomeTeam,
                    AwayTeam = dto.AwayTeam,
                    StartTime = dto.StartTime
                });

            return PartialView("ViewMatch", mapper.Map<TeamMatchViewModel>(match));
        }

        [HttpPut("{controller}/{leagueName}/match/{guid}/score")]
        public async Task<IActionResult> UpdateTeamLeagueMatchScore(string leagueName, string guid, UpdateScoreDto dto)
        {
            var match = await competitionApi.UpdateTeamLeagueMatchScore(
                new UpdateTeamLeagueMatchScoreCommand
                {
                    LeagueName = WebUtility.HtmlDecode(leagueName),
                    Guid = new Guid(guid),
                    HomeMatchEntry = dto.HomeMatchEntry,
                    AwayMatchEntry = dto.AwayMatchEntry
                });

            return PartialView("ViewMatch", mapper.Map<TeamMatchViewModel>(match));
        }

        [HttpGet("{controller}/{leagueName}/match/{guid}/score")]
        public async Task<IActionResult> SetScore(string leagueName, string guid)
        {
            var match = await competitionApi.GetTeamLeagueMatch(
               new GetTeamLeagueMatchQuery
               {
                   LeagueName = leagueName,
                   Guid = new Guid(guid)
               }
           );

            var viewModel = mapper.Map<TeamMatchViewModel>(match);
            return PartialView(viewModel);
        }

        [HttpGet("{controller}/{leagueName}/match/details/{guid}")]
        public async Task<IActionResult> ViewMatchDetails(string leagueName, string guid)
        {
            var match = await competitionApi.GetTeamLeagueMatchDetails(
                new GetTeamLeagueMatchDetailsQuery
                {
                    LeagueName = leagueName,
                    Guid = new Guid(guid)
                }
            );

            var viewModel = mapper.Map<TeamMatchViewModel>(match);
            ViewData["TeamLeagueApiUrl"] = apiSettings.TeamLeagueApiUrl;
            ViewData["PlayerApiUrl"] = apiSettings.PlayerApiUrl;
            ViewData["GetPlayerBaseUrl"] = $"{apiSettings.TeamLeagueApiUrl}/{leagueName}/competitor/";
            ViewData["MatchBaseUrl"] = $"{apiSettings.TeamLeagueApiUrl}/{leagueName}/match/{guid}/";
            return View(viewModel);
        }

        [HttpGet("{controller}/{leagueName}/match/{matchGuid}/lineup/{teamName}/{lineupEntryGuid}/view")]
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

            var viewModel = mapper.Map<TeamMatchEntryLineupEntryViewModel>(lineupEntry);
            return PartialView(viewModel);
        }

        [HttpGet("{controller}/{leagueName}/match/{matchGuid}/lineup/{teamName}/{lineupEntryGuid}/edit")]
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
            ViewData["Players"] = mapper.Map<IEnumerable<PlayerViewModel>>(players.Select(p => p.Player));

            var viewModel = mapper.Map<TeamMatchEntryLineupEntryViewModel>(lineupEntry);
            return PartialView(viewModel);
        }

        [HttpPut("{controller}/{leagueName}/match/{matchGuid}/lineup/{teamName}/{lineupEntryGuid}")]
        public async Task<IActionResult> UpdateMatchLineupEntry(string leagueName, Guid matchGuid, string teamName, Guid lineupEntryGuid, UpdateLineupEntryDto dto)
        {
            var lineupEntry = await competitionApi.UpdateTeamLeagueMatchLineupEntry(
                new UpdateTeamLeagueMatchLineupEntryCommand
                {
                    LeagueName = WebUtility.HtmlDecode(leagueName),
                    MatchGuid = matchGuid,
                    TeamName = teamName,
                    LineupEntryGuid = lineupEntryGuid,
                    PlayerNumber = dto.PlayerNumber,
                    PlayerName = dto.PlayerName
                });

            var viewModel = mapper.Map<TeamMatchEntryLineupEntryViewModel>(lineupEntry);
            return PartialView("ViewMatchLineupEntry", viewModel);
        }
    }
}