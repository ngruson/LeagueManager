using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.Competitions.Queries.GetCompetitions
{
    public class GetCompetitionsQuery : IRequest<IEnumerable<CompetitionDto>>
    {
        public string Country { get; set; }
    }
}