using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Competition;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch
{
    public class TeamLeagueDto : IMapFrom<TeamLeague>, ITeamLeagueDto<RoundDto>
    {
        public string Name { get; set; }
        public string Country { get; set; }

        public List<string> Competitors { get; set; }
        public List<RoundDto> Rounds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamLeague, TeamLeagueDto>()
                .ForMember(m => m.Country, opt => opt.MapFrom(src => src.Country != null ? src.Country.Name : null))
                .ForMember(m => m.Competitors, opt => opt.MapFrom(src => src.Competitors));

            profile.CreateMap<Domain.Competitor.TeamCompetitor, string>()
                .ConvertUsing(src => src.Team != null ? src.Team.Name : null);
        }
    }
}