using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal
{
    public class GetTeamLeagueMatchGoalQuery : IRequest<GoalDto>
    {
        public string LeagueName { get; set; }
        public Guid MatchGuid { get; set; }
        public Guid GoalGuid { get; set; }
    }
}