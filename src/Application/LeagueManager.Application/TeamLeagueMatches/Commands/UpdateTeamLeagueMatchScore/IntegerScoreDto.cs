using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Score;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore
{
    public class IntegerScoreDto : IMapFrom<IntegerScore>, IIntegerScoreDto
    {
        public int? Value { get; set; }
    }
}