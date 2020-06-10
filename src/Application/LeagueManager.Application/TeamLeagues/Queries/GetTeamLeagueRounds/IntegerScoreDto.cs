using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Score;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds
{
    public class IntegerScoreDto : IMapFrom<IntegerScore>, IIntegerScoreDto
    {
        public int? Value { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IntegerScore, IIntegerScoreDto>()
                .As<IntegerScoreDto>();
        }
    }
}