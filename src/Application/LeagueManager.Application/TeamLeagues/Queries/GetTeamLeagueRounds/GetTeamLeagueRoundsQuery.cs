using LeagueManager.Application.TeamLeagues.Dto;
using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds
{
    public class GetTeamLeagueRoundsQuery : IRequest<IEnumerable<TeamLeagueRoundDto>>
    {
        public string LeagueName { get; set; }
    }
}