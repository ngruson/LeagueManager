using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Round;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch
{
    public class TeamLeagueRoundDto : IMapFrom<TeamLeagueRound>
    {
        public string Name { get; set; }
        public List<TeamMatchDto> Matches { get; set; }
    }
}