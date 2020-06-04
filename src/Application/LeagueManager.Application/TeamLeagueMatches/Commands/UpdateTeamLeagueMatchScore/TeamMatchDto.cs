using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Match;
using System;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore
{
    public class TeamMatchDto : IMapFrom<TeamLeagueMatch>, ITeamMatchDto<ITeamMatchEntryDto>
    {
        public string TeamLeagueName { get; set; }
        public string RoundName { get; set; }
        public Guid Guid { get; set; }
        public DateTime? StartTime { get; set; }
        public List<ITeamMatchEntryDto> MatchEntries { get; set; } = new List<ITeamMatchEntryDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamLeagueMatch, TeamMatchDto>()
                .ForMember(m => m.TeamLeagueName, opt => opt.MapFrom(src =>
                    src.TeamLeagueRound != null && src.TeamLeagueRound.TeamLeague != null ?
                    src.TeamLeagueRound.TeamLeague.Name : null))
                .ForMember(m => m.RoundName, opt => opt.MapFrom(src =>
                    src.TeamLeagueRound != null ? src.TeamLeagueRound.Name : null));
        }
    }
}