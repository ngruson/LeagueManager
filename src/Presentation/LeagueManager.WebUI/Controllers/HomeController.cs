using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LeagueManager.WebUI.Models;
using LeagueManager.WebUI.Configuration;
using LeagueManager.WebUI.ViewModels;
using LeagueManager.Application.Interfaces;
using AutoMapper;
using LeagueManager.Application.Config;
using LeagueManager.Infrastructure.WritableOptions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace LeagueManager.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWritableOptions<InitSettings> initSettings;
        private readonly IMapper mapper;
        private readonly ICompetitionApi competitionApi;
        private readonly ICountryApi countryApi;
        private readonly ITeamApi teamApi;

        public HomeController(IWritableOptions<InitSettings> initSettings,
            IMapper mapper,
            ICompetitionApi competitionApi,
            ICountryApi countryApi,
            ITeamApi teamApi)
        {
            this.initSettings = initSettings;
            this.mapper = mapper;
            this.competitionApi = competitionApi;
            this.countryApi = countryApi;
            this.teamApi = teamApi;
        }
        public IActionResult Index(bool init)
        {
            if (!init && (!initSettings.Value.Init))
                return RedirectToAction("GettingStarted");
            return View();
        }

        [Authorize]
        public IActionResult GettingStarted()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GettingStarted(GettingStartedViewModel model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var dbConfig = mapper.Map<DbConfig>(model);

            var result = await competitionApi.Configure(dbConfig, accessToken);
            result = result && await countryApi.Configure(dbConfig, accessToken);
            result = result && await teamApi.Configure(dbConfig, accessToken);

            if (result)
            {
                initSettings.Update(opt =>
                {
                    opt.Init = true;
                });
            }            

            return RedirectToAction("Index", new { init = true });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}