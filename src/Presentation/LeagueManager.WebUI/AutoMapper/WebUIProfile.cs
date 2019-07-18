using AutoMapper;
using LeagueManager.Application.Competitions.Commands;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.WebUI.ViewModels;
using System;
using System.IO;

namespace LeagueManager.WebUI.AutoMapper
{
    public class WebUIProfile : Profile
    {
        public WebUIProfile()
        {
            CreateMap<CompetitionDto, CompetitionViewModel>()
                .ForMember(m => m.Logo, opt =>
                {
                    opt.Condition(src => src.Logo != null);
                    opt.MapFrom((src, dest) =>
                    {
                        var base64 = Convert.ToBase64String(src.Logo);
                        return $"data:image/gif;base64,{base64}";
                    });
                });

            
            CreateMap<TeamLeagueViewModel, CreateTeamLeagueCommand>()
                .ForMember(m => m.Logo, opt => {
                    opt.Condition(src => src.Logo != null);
                    opt.MapFrom((src, dest) =>
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            src.Logo.CopyTo(memoryStream);
                            return memoryStream.ToArray();
                        }
                    });
                })
                .ForMember(m => m.Teams, opt => opt.MapFrom(src => src.SelectedTeamIds));
        }
    }
}