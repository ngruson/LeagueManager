using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Competition;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds
{
    public class TeamLeagueDto : IMapFrom<TeamLeague>, ITeamLeagueDto<RoundDto>
    {
        public string Name { get; set; }
        public List<RoundDto> Rounds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamLeague, TeamLeagueDto>();
        }
    }
}