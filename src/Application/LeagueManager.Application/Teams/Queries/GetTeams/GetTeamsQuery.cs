using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.Teams.Queries.GetTeams
{
    public class GetTeamsQuery : IRequest<IEnumerable<TeamDto>>
    {
    }
}