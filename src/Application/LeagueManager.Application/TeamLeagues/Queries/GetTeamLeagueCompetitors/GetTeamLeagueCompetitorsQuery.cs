using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueCompetitors
{
    public class GetTeamLeagueCompetitorsQuery : IRequest<IEnumerable<CompetitorDto>>
    {
        public string LeagueName { get; set; }
    }
}