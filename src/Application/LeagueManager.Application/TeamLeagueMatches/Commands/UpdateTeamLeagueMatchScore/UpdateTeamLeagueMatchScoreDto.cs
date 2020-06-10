using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore
{
    public class UpdateTeamLeagueMatchScoreDto
    {
        public List<TeamMatchEntryRequestDto> MatchEntries { get; set; }
    }
}