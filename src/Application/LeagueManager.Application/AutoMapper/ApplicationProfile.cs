using AutoMapper;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.LeagueTable;
using LeagueManager.Domain.Round;
using System.Linq;

namespace LeagueManager.Application.AutoMapper
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<TeamLeague, CompetitionDto>()
                .ForMember(m => m.Country, opt =>
                {
                    opt.Condition(src => src.Country != null);
                    opt.MapFrom(src => src.Country.Name);
                })
                .ForMember(m => m.Competitors, opt => opt.MapFrom(src =>
                    src.Competitors.Select(c => c.Team.Name)));
            CreateMap<TeamLeague, TeamLeagueDto>()
                .ForMember(m => m.Country, opt =>
                {
                    opt.Condition(src => src.Country != null);
                    opt.MapFrom(src => src.Country.Name);
                })
                .ForMember(m => m.Competitors, opt => opt.MapFrom(src =>
                    src.Competitors.Select(c => c.Team.Name)));
            
            CreateMap<TeamLeagueRound, TeamLeagueRoundDto>();
            CreateMap<TeamLeagueTable, TeamLeagueTableDto>();
            CreateMap<TeamLeagueTableItem, TeamLeagueTableItemDto>();
            CreateMap<Team, TeamDto>();
        }
    }
}