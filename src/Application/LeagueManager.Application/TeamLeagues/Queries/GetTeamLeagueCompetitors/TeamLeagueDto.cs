using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Competition;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueCompetitors
{
    public class TeamLeagueDto : IMapFrom<TeamLeague>
    {
        public string Name { get; set; }
        public IList<CompetitorDto> Competitors { get; set; }
    }
}