using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Player.Commands.CreatePlayer;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeagueManager.Api.PlayerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IMediator mediator;

        public PlayerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerCommand command)
        {
            try
            {
                var player = await mediator.Send(command);
                return Created($"/player/{player.FullName}", new { player });
            }
            catch (LeagueManagerException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}