using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Score;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch
{
    public class IntegerScoreDto : IMapFrom<IntegerScore>, IIntegerScoreDto
    {
        public int? Value { get; set; }
    }
}