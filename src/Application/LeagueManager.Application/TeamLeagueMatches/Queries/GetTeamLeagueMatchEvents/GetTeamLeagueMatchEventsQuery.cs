using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class GetTeamLeagueMatchEventsQuery : IRequest<MatchEventsVm>
    {
        public string LeagueName { get; set; }
        public Guid MatchGuid { get; set; }
        public string TeamName { get; set; }
    }
}