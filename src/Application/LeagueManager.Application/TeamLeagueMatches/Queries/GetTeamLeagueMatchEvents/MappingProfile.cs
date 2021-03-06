﻿using AutoMapper;
using Dto = LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Round;
using LeagueManager.Domain.Competitor;
using System;
using LeagueManager.Domain.Score;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TeamLeague, TeamLeagueDto>();
            CreateMap<TeamLeagueRound, Dto.IRoundDto<TeamMatchDto>>()
                .As<RoundDto>();

            CreateMap<TeamLeagueMatch, Dto.ITeamMatchDto<Dto.ITeamMatchEntryEventsDto>>()
                .ForMember(m => m.TeamLeagueName, opt => opt.MapFrom(src =>
                    src.TeamLeagueRound != null && src.TeamLeagueRound.TeamLeague != null ?
                    src.TeamLeagueRound.TeamLeague.Name : null))
                .ForMember(m => m.RoundName, opt => opt.MapFrom(src =>
                    src.TeamLeagueRound != null ? src.TeamLeagueRound.Name : null))
                .As<TeamMatchDto>();

            CreateMap<TeamMatchEntry, Dto.ITeamMatchEntryEventsDto>()
                .ForMember(m => m.TeamLeagueMatchGuid, opt => opt.MapFrom(src => src.TeamLeagueMatch.Guid))
                .ForMember(m => m.Team, opt => opt.MapFrom(src => src.Team))
                .ForMember(m => m.HomeAway, opt => opt.MapFrom(src => src.HomeAway == Domain.Match.HomeAway.Home ?
                    Dto.HomeAway.Home : Dto.HomeAway.Away))
                .As<TeamMatchEntryDto>();

            CreateMap<Team, Dto.ITeamDto>()
                .ForMember(m => m.Logo, opt => opt.MapFrom(src => src.Logo != null ?
                    $"data:image/gif;base64,{Convert.ToBase64String(src.Logo)}" : null))
                .As<TeamDto>();

            CreateMap<Domain.Player.Player, Dto.IPlayerDto>()
                .As<PlayerDto>();

            CreateMap<TeamMatchEntryGoal, Dto.IGoalDto>()
                .As<GoalDto>();

            CreateMap<IntegerScore, Dto.IIntegerScoreDto>()
                .As<IntegerScoreDto>();

            CreateMap<TeamMatchEntrySubstitution, Dto.ISubstitutionDto>()
                .As<SubstitutionDto>();
        }
    }
}