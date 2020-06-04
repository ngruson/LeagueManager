using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Match;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry
{
    public class LineupEntryDto : IMapFrom<TeamMatchEntryLineupEntry>
    {
        public Guid Guid { get; set; }
        public string PlayerNumber { get; set; }
        public PlayerDto Player { get; set; }
        public string TeamName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamMatchEntryLineupEntry, LineupEntryDto>()
                .ForMember(m => m.TeamName, opt => opt.MapFrom(src => src.TeamMatchEntry.Team.Name));
        }
    }
}