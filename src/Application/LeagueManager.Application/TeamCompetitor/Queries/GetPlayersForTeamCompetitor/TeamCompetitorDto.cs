using LeagueManager.Application.Common.Mappings;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor
{
    public class TeamCompetitorDto : IMapFrom<Domain.Competitor.TeamCompetitor>
    {
        public string TeamName { get; set; }
        public List<CompetitorPlayerDto> Players { get; set; }
    }
}