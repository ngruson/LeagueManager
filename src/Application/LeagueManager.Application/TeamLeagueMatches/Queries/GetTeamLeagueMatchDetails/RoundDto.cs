using LeagueManager.Application.Interfaces.Dto;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class RoundDto : IRoundDto<TeamMatchDto>
    {
        public string Name { get; set; }
        public List<TeamMatchDto> Matches { get; set; }        
    }    
}