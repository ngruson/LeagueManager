using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Competition;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeagueManager.Application.Competitions.Queries.GetCompetitions
{
    public class CompetitionDto : IMapFrom<TeamLeague>
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Logo { get; set; }
        public List<string> Competitors { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamLeague, CompetitionDto>()
                .ForMember(m => m.Country, opt =>
                {
                    opt.Condition(src => src.Country != null);
                    opt.MapFrom(src => src.Country.Name);
                })
                .ForMember(m => m.Logo, opt => opt.MapFrom(src => src.Logo != null ?
                    $"data:image/gif;base64,{Convert.ToBase64String(src.Logo)}" : null))
                .ForMember(m => m.Competitors, opt => opt.MapFrom(src =>
                    src.Competitors.Select(c => c.Team.Name)));
        }
    }
}