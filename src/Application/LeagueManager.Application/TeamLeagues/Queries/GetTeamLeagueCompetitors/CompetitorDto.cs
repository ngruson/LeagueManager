using LeagueManager.Application.Common.Mappings;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueCompetitors
{
    public class CompetitorDto : IMapFrom<Domain.Competitor.TeamCompetitor>
    {
        public string TeamName { get; set; }
    }
}