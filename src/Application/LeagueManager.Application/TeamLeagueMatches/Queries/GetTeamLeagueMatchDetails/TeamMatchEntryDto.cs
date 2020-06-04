using Dto = LeagueManager.Application.Interfaces.Dto;
using System.Collections.Generic;
using System.Linq;
using LeagueManager.Application.Interfaces.Dto;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class TeamMatchEntryDto : /*IMapFrom<TeamMatchEntry>,*/ ITeamMatchEntryWithDetailsDto
    {
        public Guid TeamLeagueMatchGuid { get; set; }
        public ITeamDto Team { get; set; }
        public HomeAway HomeAway { get; set; }
        public IIntegerScoreDto Score { get; set; }
        public List<ILineupEntryDto> Lineup { get; set; }
        public List<IGoalDto> Goals { get; set; }

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<TeamMatchEntry, ITeamMatchEntryWithDetailsDto>()
        //        .ForMember(m => m.TeamLeagueMatchGuid, opt => opt.MapFrom(src => src.TeamLeagueMatch.Guid))
        //        .ForMember(m => m.Team, opt => opt.MapFrom(src => src.Team))
        //        .ForMember(m => m.HomeAway, opt => opt.MapFrom(src => src.HomeAway == Domain.Match.HomeAway.Home ?
        //            Dto.HomeAway.Home : Dto.HomeAway.Away))                
        //        .As<TeamMatchEntryDto>();
        //}
    }
}