using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Match;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal
{
    public class GoalDto : IMapFrom<TeamMatchEntryGoal>
    {
        public string Minute { get; set; }
        public string PlayerFullName { get; set; }
    }
}