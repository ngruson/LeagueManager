using LeagueManager.Application.TeamLeagues.Queries.Dto;

namespace LeagueManager.Api.CompetitionApi.Dto
{
    public class UpdateScoreDto
    {
        public TeamMatchEntryDto HomeMatchEntry { get; set; }
        public TeamMatchEntryDto AwayMatchEntry { get; set; }
    }
}