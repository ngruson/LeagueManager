using AutoMapper;
using LeagueManager.Application.Leagues.Commands;
using LeagueManager.WebUI.ViewModels;
using System.IO;

namespace LeagueManager.WebUI.AutoMapper
{
    public class WebUIProfile : Profile
    {
        public WebUIProfile()
        {
            CreateMap<TeamLeagueViewModel, CreateTeamLeagueCommand>()
                .ForMember(m => m.Logo, opt => opt.MapFrom((src, dest) =>
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        src.Logo.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }))
                .ForMember(m => m.Teams, opt => opt.MapFrom(src => src.SelectedTeamIds));
        }
    }
}