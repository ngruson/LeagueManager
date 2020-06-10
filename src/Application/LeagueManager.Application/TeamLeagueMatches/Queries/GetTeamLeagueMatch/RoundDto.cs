using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Round;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch
{
    public class RoundDto : IMapFrom<TeamLeagueRound>, IRoundDto<TeamMatchDto>
    {
        public string Name { get; set; }
        public List<TeamMatchDto> Matches { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamLeagueRound, IRoundDto<TeamMatchDto>>().As<RoundDto>();
        }
    }    
}