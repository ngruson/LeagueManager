using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Match;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry
{
    public class LineupEntryDto : IMapFrom<TeamMatchEntryLineupEntry>, Interfaces.Dto.ILineupEntryDto
    {
        public Guid Guid { get; set; }
        public string PlayerNumber { get; set; }
        public Interfaces.Dto.IPlayerDto Player { get; set; }
        public string TeamMatchEntryTeamName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamMatchEntryLineupEntry, Interfaces.Dto.ILineupEntryDto>()
                .ForMember(m => m.PlayerNumber, opt => opt.MapFrom(src => src.Number))
                //.ForMember(m => m.TeamMatchEntryTeamName, opt => opt.MapFrom(src =>
                //    src.TeamMatchEntry != null && src.TeamMatchEntry.Team != null ? src.TeamMatchEntry.Team.Name : null))
                .As<LineupEntryDto>();
        }
    }
}