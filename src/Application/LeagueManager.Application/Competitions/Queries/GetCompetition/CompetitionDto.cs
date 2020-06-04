using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Competition;
using System.Collections.Generic;
using System.Linq;

namespace LeagueManager.Application.Competitions.Queries.GetCompetition
{
    public class CompetitionDto : IMapFrom<TeamLeague>
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public byte[] Logo { get; set; }
        public List<string> Competitors { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamLeague, CompetitionDto>()
                .ForMember(m => m.Country, opt =>
                {
                    opt.Condition(src => src.Country != null);
                    opt.MapFrom(src => src.Country.Name);
                })
                .ForMember(m => m.Competitors, opt => opt.MapFrom(src =>
                    src.Competitors.Select(c => c.Team.Name)));
        }
    }
}