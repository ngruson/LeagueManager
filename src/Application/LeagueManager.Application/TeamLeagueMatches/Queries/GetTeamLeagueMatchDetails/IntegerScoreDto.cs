using LeagueManager.Application.Interfaces.Dto;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class IntegerScoreDto : IIntegerScoreDto
    {
        public int? Value { get; set; }        
    }
}