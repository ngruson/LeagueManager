using LeagueManager.Application.TeamLeagueMatches.Dto;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore
{
    public class UpdateScoreDto
    {
        public TeamMatchEntryDto HomeMatchEntry { get; set; }
        public TeamMatchEntryDto AwayMatchEntry { get; set; }
    }
}