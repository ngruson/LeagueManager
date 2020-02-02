using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.Teams.Queries.GetTeamsByCountry
{
    public class GetTeamsByCountryQuery : IRequest<IEnumerable<TeamDto>>
    {
        public string Country { get; set; }
    }
}