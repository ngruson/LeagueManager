using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Match;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal
{
    public class GoalDto : IMapFrom<TeamMatchEntryGoal>
    {
        public string Minute { get; set; }
        public string PlayerName { get; set; }
    }
}