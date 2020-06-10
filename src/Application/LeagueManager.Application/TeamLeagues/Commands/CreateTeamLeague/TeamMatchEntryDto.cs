using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Domain.Match;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague
{
    public class TeamMatchEntryDto : IMapFrom<TeamMatchEntry>
    {
        public HomeAway HomeAway { get; set; }
        public List<LineupEntryDto> Lineup { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamMatchEntry, TeamMatchEntryDto>()
                .ForMember(m => m.HomeAway, opt => opt.MapFrom(src =>
                    src.HomeAway == Domain.Match.HomeAway.Home ? HomeAway.Home : HomeAway.Away));
        }
    }
}