using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Commands
{
    public class UpdateTeamLeagueMatchCommand : IRequest
    {
        public string LeagueName { get; set; }
        public Guid Guid { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime? StartTime { get; set; }
    }
}