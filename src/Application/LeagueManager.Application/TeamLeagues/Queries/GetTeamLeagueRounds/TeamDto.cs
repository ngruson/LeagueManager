using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Competitor;
using System;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds
{
    public class TeamDto : IMapFrom<Team>, ITeamDto
    {
        public string Name { get; set; }
        public string Logo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Team, ITeamDto>()
                .ForMember(m => m.Logo, opt => opt.MapFrom(src => src.Logo != null ?
                    $"data:image/gif;base64,{Convert.ToBase64String(src.Logo)}" : null))
                .As<TeamDto>();
        }
    }
}