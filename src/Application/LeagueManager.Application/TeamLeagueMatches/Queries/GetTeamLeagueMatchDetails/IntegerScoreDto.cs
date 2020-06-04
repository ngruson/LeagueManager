using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Score;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class IntegerScoreDto : /*IMapFrom<Domain.Score.IntegerScore>,*/ IIntegerScoreDto
    {
        public int? Value { get; set; }

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<IntegerScore, IIntegerScoreDto>()
        //        .As<IntegerScoreDto>();
        //}
    }
}