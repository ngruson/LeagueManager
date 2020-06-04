using LeagueManager.Application.Interfaces.Dto;
using System;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class TeamMatchDto : /*IMapFrom<TeamLeagueMatch>,*/ ITeamMatchDto<ITeamMatchEntryWithDetailsDto>
    {
        public Guid Guid { get; set; }
        public string TeamLeagueName { get; set; }
        public string RoundName { get; set; }
        public DateTime? StartTime { get; set; }
        public List<ITeamMatchEntryWithDetailsDto> MatchEntries { get; set; } = new List<ITeamMatchEntryWithDetailsDto>();

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<TeamLeagueMatch, ITeamMatchDto<ITeamMatchEntryWithDetailsDto>>()
        //        .ForMember(m => m.TeamLeagueName, opt => opt.MapFrom(src =>
        //            src.TeamLeagueRound != null && src.TeamLeagueRound.TeamLeague != null ?
        //            src.TeamLeagueRound.TeamLeague.Name : null))
        //        .ForMember(m => m.RoundName, opt => opt.MapFrom(src =>
        //            src.TeamLeagueRound != null ? src.TeamLeagueRound.Name : null))
        //        .As<TeamMatchDto>();
        //}
    }
}