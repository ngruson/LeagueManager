using System.Threading.Tasks;
using LeagueManager.Application.Countries.Queries.GetCountries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeagueManager.Api.CountryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IMediator mediator;

        public CountryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var countries = await mediator.Send(new GetCountriesQuery());
                return Ok(countries);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}