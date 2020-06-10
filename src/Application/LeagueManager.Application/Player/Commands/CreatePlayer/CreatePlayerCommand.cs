using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Player.Dto;
using MediatR;

namespace LeagueManager.Application.Player.Commands.CreatePlayer
{
    public class CreatePlayerCommand : IRequest<PlayerDto>, IMapFrom<Domain.Player.Player>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePlayerCommand, Domain.Player.Player>();
        }
    }
}