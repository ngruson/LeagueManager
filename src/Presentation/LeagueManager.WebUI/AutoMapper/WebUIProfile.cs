using AutoMapper;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.WebUI.ViewModels;
using System;
using System.IO;
using LeagueManager.Application.Config;
using LeagueManager.Application.Player.Dto;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using LeagueManager.Application.TeamLeagues.Dto;
using LeagueManager.Application.TeamCompetitor.Dto;
using LeagueManager.Application.TeamLeagueMatches.Goals;

namespace LeagueManager.WebUI.AutoMapper
{
    public class WebUIProfile : Profile
    {
        public WebUIProfile()
        {
            CreateMap<GettingStartedViewModel, DbConfig>();

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

            
            CreateMap<CreateTeamLeagueViewModel, CreateTeamLeagueCommand>()
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

            CreateMap<Application.TeamLeagues.Dto.TeamLeagueDto, ViewTeamLeagueViewModel>();
            CreateMap<TeamLeagueTableDto, TeamLeagueTableViewModel>();
            CreateMap<TeamLeagueTableItemDto, TeamLeagueTableItemViewModel>();
            CreateMap<TeamDto, TeamViewModel>()
                .ForMember(m => m.Logo, opt =>
                {
                    opt.Condition(src => src.Logo != null);
                    opt.MapFrom((src, dest) =>
                    {
                        var base64 = Convert.ToBase64String(src.Logo);
                        return $"data:image/gif;base64,{base64}";
                    });
                });
            CreateMap<TeamLeagueRoundDto, TeamLeagueRoundViewModel>();
            CreateMap<TeamMatchDto, TeamMatchViewModel>();
            CreateMap<TeamMatchEntryDto, TeamMatchEntryViewModel>()
                .ForMember(m => m.Events, opt => opt.Ignore());
            CreateMap<GoalDto, TeamMatchEntryGoalEventViewModel>();
            CreateMap<TeamCompetitorPlayerDto, PlayerViewModel>();
            CreateMap<Application.Player.Dto.PlayerDto, PlayerViewModel>();
            CreateMap<Application.TeamLeagueMatches.Lineup.Dto.PlayerDto, PlayerViewModel>();
            CreateMap<LineupEntryDto, TeamMatchEntryLineupEntryViewModel>()
                .ForMember(m => m.Number, opt => opt.MapFrom(src => src.PlayerNumber));
            CreateMap<IntegerScoreDto, IntegerScoreViewModel>();
        }
    }
}