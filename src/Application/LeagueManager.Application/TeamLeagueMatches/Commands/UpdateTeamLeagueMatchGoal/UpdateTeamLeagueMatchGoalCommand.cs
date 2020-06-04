using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal
{
    public class UpdateTeamLeagueMatchGoalCommand : IRequest<GoalDto>
    {
        public string LeagueName { get; set; }
        public Guid MatchGuid { get; set; }
        public string TeamName { get; set; }
        public Guid GoalGuid { get; set; }
        public string Minute { get; set; }
        public string PlayerName { get; set; }
    }
}