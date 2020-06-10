using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Competition;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor
{
    public class TeamLeagueDto : IMapFrom<TeamLeague>
    {
        public string Name { get; set; }
        public List<TeamCompetitorDto> Competitors { get; set; }
    }
}