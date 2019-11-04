using AutoMapper;
using LeagueManager.Api.CompetitionApi.Dto;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;

namespace LeagueManager.Api.CompetitionApi.AutoMapper
{
    public class CompetitionApiProfile : Profile
    {
        public CompetitionApiProfile()
        {
            CreateMap<UpdateTeamLeagueMatchDto, UpdateTeamLeagueMatchCommand>()
                .ForMember(m => m.LeagueName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["leagueName"]))
                .ForMember(m => m.Guid, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["guid"]));

            CreateMap<UpdateScoreDto, UpdateTeamLeagueMatchScoreCommand>()
                .ForMember(m => m.LeagueName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["leagueName"]))
                .ForMember(m => m.Guid, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["guid"]));

        }
    }
}