using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Entities;
using LeagueManager.Infrastructure;
using LeagueManager.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeagueManager.WebUI.Controllers
{
    public class LeaguesController : Controller
    {
        private readonly ApiSettings apiSettings;
        private readonly ITeamApi teamApi;

        public LeaguesController(IOptions<ApiSettings> apiSettings,
            ITeamApi teamApi)
        {
            this.apiSettings = apiSettings.Value;
            this.teamApi = teamApi;
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new TeamLeagueViewModel();
            viewModel.AllTeams = await teamApi.GetTeams();
            viewModel.SelectedTeams = new List<Team>();
            viewModel.TeamApiUrl = apiSettings.TeamApiUrl;
            viewModel.CountryApiUrl = apiSettings.CountryApiUrl;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(TeamLeagueViewModel model)
        {
            return View();
        }
    }
}