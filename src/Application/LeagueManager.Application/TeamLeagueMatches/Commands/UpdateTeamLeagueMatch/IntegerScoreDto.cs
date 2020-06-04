using LeagueManager.Application.Interfaces.Dto;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch
{
    public class IntegerScoreDto : IIntegerScoreDto
    {
        public int? Value { get; set; }
    }
}