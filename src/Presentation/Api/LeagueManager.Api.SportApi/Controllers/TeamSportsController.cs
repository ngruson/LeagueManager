using System.Threading.Tasks;
using LeagueManager.Application.Sports.Queries.GetTeamSports;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeagueManager.Api.SportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamSportsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TeamSportsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var teamSports = await mediator.Send(new GetTeamSportsQuery());
                return Ok(teamSports);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}