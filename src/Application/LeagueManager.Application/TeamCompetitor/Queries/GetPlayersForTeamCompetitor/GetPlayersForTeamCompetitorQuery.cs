using LeagueManager.Application.TeamCompetitor.Dto;
using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor
{
    public class GetPlayersForTeamCompetitorQuery : IRequest<IEnumerable<TeamCompetitorPlayerDto>>
    {
        public string LeagueName { get; set; }
        public string TeamName { get; set; }
    }
}