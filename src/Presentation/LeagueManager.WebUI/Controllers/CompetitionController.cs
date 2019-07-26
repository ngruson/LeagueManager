using AutoMapper;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Competitions.Commands;
using LeagueManager.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueManager.Domain.Competitor;
using LeagueManager.Infrastructure.Api;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Competitions.Queries.GetTeamLeague;

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

        [Route("{controller}/{leagueName}", Name = "ViewTeamLeague")]
        public async Task<IActionResult> ViewTeamLeague(string leagueName)
        {            
            var teamLeague = await competitionApi.ViewTeamLeague(
                new GetTeamLeagueQuery
                {
                    Name = leagueName
                });

            var viewModel = mapper.Map<ViewTeamLeagueViewModel>(teamLeague);

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
    }
}