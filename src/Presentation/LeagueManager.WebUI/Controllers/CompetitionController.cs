using AutoMapper;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Leagues.Commands;
using LeagueManager.Domain.Entities;
using LeagueManager.Infrastructure;
using LeagueManager.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManager.WebUI.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly ApiSettings apiSettings;
        private readonly ITeamApi teamApi;
        private readonly ILeagueApi leagueApi;
        private readonly IMapper mapper;

        public CompetitionController(
            IOptions<ApiSettings> apiSettings,
            ITeamApi teamApi,
            ILeagueApi leagueApi,
            IMapper mapper)
        {
            this.apiSettings = apiSettings.Value;
            this.teamApi = teamApi;
            this.leagueApi = leagueApi;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var competitions = new List<CompetitionViewModel>();
            return View(competitions);
        }

        public async Task<IActionResult> CreateTeamLeague()
        {
            var teams = await teamApi.GetTeams();

            var viewModel = new TeamLeagueViewModel
            {
                AllTeams = teams,
                SelectedTeams = new List<Team>(),
                TeamApiUrl = apiSettings.TeamApiUrl,
                CountryApiUrl = apiSettings.CountryApiUrl,
                AllTeamsCountries = teams
                    .GroupBy(x => x.Country, (key, c) => c.FirstOrDefault().Country)
                    .OrderBy(x => x)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeamLeague(TeamLeagueViewModel model)
        {
            //Func<Task<byte[]>> logoFile = async () =>
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        await model.Logo.CopyToAsync(memoryStream);
            //        return memoryStream.ToArray();
            //    }
            //};

            var command = mapper.Map<CreateTeamLeagueCommand>(model);
            await leagueApi.CreateTeamLeague(command);
            return RedirectToAction("Index");
        }
    }
}