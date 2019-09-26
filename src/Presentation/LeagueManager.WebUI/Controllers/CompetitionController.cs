using AutoMapper;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueManager.Domain.Competitor;
using LeagueManager.Infrastructure.Api;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using System.Linq;
using LeagueManager.Application.Competitions.Queries.GetCompetition;

namespace LeagueManager.WebUI.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly ApiSettings apiSettings;
        private readonly ICountryApi countryApi;
        private readonly ITeamApi teamApi;
        private readonly ICompetitionApi competitionApi;
        private readonly IMapper mapper;

        public CompetitionController(
            IOptions<ApiSettings> apiSettings,
            ICountryApi countryApi,
            ITeamApi teamApi,
            ICompetitionApi competitionApi,
            IMapper mapper)
        {
            this.apiSettings = apiSettings.Value;
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
                SelectedTeams = new List<Team>(),
                TeamApiUrl = apiSettings.TeamApiUrl,
                CountryApiUrl = apiSettings.CountryApiUrl,
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
                Rounds = mapper.Map<IEnumerable<TeamLeagueRoundViewModel>>(rounds)
            };

            return View(viewModel);
        }

        [Route("{controller}/{leagueName}/rounds", Name = "EditTeamLeagueRounds")]
        public async Task<IActionResult> EditTeamLeagueRounds(string leagueName, string round)
        {
            var teamLeague = await competitionApi.GetCompetition(
                new GetCompetitionQuery
                {
                    Name = leagueName
                });
            var rounds = await competitionApi.GetTeamLeagueRounds(
                new GetTeamLeagueRoundsQuery
                {
                    LeagueName = leagueName
                });

            var viewModel = new EditTeamLeagueRoundsViewModel
            {
                Name = leagueName,
                Teams = teamLeague.Competitors.Select(c => new TeamViewModel {  Name = c }),
                Rounds = mapper.Map<IEnumerable<TeamLeagueRoundViewModel>>(rounds)
            };
            viewModel.SelectedRound = round ?? viewModel.Rounds.ToList()[0].Name;

            return View(viewModel);
        }
    }
}