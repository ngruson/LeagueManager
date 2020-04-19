using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal
{
    public class AddTeamLeagueMatchGoalCommand : IRequest<Unit>
    {
        public string LeagueName { get; set; }
        public Guid MatchGuid { get; set; }
        public string TeamName { get; set; }
        public string Minute { get; set; }
        public string PlayerName { get; set; }
    }
}