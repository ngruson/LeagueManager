using LeagueManager.Application.TeamCompetitor.Dto;
using MediatR;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor
{
    public class GetPlayerForTeamCompetitorQuery : IRequest<TeamCompetitorPlayerDto>
    {
        public string LeagueName { get; set; }
        public string TeamName { get; set; }
        public string PlayerName { get; set; }
    }
}