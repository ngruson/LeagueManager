using MediatR;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor
{
    public class GetPlayerForTeamCompetitorQuery : IRequest<CompetitorPlayerDto>
    {
        public string LeagueName { get; set; }
        public string TeamName { get; set; }
        public string PlayerName { get; set; }
    }
}