using LeagueManager.Application.Player.Dto;
using MediatR;

namespace LeagueManager.Application.Player.Commands.CreatePlayer
{
    public class CreatePlayerCommand : IRequest<PlayerDto>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}