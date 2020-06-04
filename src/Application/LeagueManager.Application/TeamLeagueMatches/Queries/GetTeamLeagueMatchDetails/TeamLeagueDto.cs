using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Competition;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class TeamLeagueDto : /*IMapFrom<TeamLeague>,*/ ITeamLeagueDto<RoundDto>
    {
        public string Name { get; set; }        
        public List<RoundDto> Rounds { get; set; }
    }
}