using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Competitor;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch
{
    public class TeamDto : IMapFrom<Team>, ITeamDto
    {
        public string Name { get; set; }
        //public string Country { get; set; }
        public string Logo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Team, TeamDto>()
                //.ForMember(m => m.Country, opt => opt.MapFrom(src => src.Country != null ? src.Country.Name : null))
                .ForMember(m => m.Logo, opt => opt.MapFrom(src => src.Logo != null ?
                    $"data:image/gif;base64,{Convert.ToBase64String(src.Logo)}" : null));
        }
    }
}