using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Match;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchSubstitution
{
    public class SubstitutionDto : IMapFrom<TeamMatchEntrySubstitution>
    {
        public string Minute { get; set; }
        public string PlayerOut { get; set; }
        public string PlayerIn { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamMatchEntrySubstitution, SubstitutionDto>()
                .ForMember(m => m.PlayerOut, opt => opt.MapFrom(src => src.PlayerOut.FullName))
                .ForMember(m => m.PlayerIn, opt => opt.MapFrom(src => src.PlayerIn.FullName));
        }
    }
}