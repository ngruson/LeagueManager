using System.Threading.Tasks;
using LeagueManager.Application.Leagues.Queries.GetLeague;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeagueManager.Api.LeagueApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly IMediator mediator;

        public LeagueController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET api/league/Premier League 2019-2020
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                var league = await mediator.Send(new GetLeagueQuery { Name = name });
                return Ok(league);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}