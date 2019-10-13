using AutoMapper;
using LeagueManager.Api.CompetitionApi.Dto;
using LeagueManager.Application.TeamLeagueMatches.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}