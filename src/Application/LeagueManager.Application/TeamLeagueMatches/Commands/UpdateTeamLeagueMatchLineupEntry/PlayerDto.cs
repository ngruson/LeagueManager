using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry
{
    public class PlayerDto : IMapFrom<Domain.Player.Player>, IPlayerDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName => this.FullName();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Player.Player, IPlayerDto>()
                .As<PlayerDto>();
        }
    }
}