using AutoMapper;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Match;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TeamMatchEntrySubstitution, SubstitutionDto>();

            CreateMap<Domain.Player.Player, IPlayerDto>()
                .As<PlayerDto>();
        }
    }
}