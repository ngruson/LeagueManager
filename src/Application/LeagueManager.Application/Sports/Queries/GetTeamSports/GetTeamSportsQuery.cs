using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.Sports.Queries.GetTeamSports
{
    public class GetTeamSportsQuery : IRequest<IEnumerable<TeamSportDto>>
    {
    }
}