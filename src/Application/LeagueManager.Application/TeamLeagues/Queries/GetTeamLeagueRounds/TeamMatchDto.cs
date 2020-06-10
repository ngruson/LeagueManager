using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Match;
using System;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds
{
    public class TeamMatchDto : IMapFrom<TeamLeagueMatch>, ITeamMatchDto<ITeamMatchEntryDto>
    {
        public string TeamLeagueName { get; set; }
        public string RoundName { get; set; }
        public Guid Guid { get; set; }
        public DateTime? StartTime { get; set; }
        public List<ITeamMatchEntryDto> MatchEntries { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamLeagueMatch, ITeamMatchDto<ITeamMatchEntryDto>>()
                .ForMember(m => m.TeamLeagueName, opt => opt.MapFrom(src => 
                    src.TeamLeagueRound != null && src.TeamLeagueRound.TeamLeague != null ?
                    src.TeamLeagueRound.TeamLeague.Name : null))
                .As<TeamMatchDto>();
        }
    }
}