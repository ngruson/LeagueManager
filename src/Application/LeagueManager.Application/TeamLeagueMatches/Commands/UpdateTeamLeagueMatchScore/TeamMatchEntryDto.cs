using LeagueManager.Application.Common.Mappings;
using Dto = LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Match;
using AutoMapper;
using LeagueManager.Domain.Score;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore
{
    public class TeamMatchEntryDto : IMapFrom<TeamMatchEntry>, IMapFrom<TeamMatchEntryRequestDto>, Dto.ITeamMatchEntryDto
    {
        public Guid TeamLeagueMatchGuid { get; set; }
        public Dto.ITeamDto Team { get; set; }
        public Dto.HomeAway HomeAway { get; set; }
        public Dto.IIntegerScoreDto Score { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamMatchEntry, TeamMatchEntryDto>();

            profile.CreateMap<TeamMatchEntryRequestDto, TeamMatchEntryDto>()
                .ForMember(m => m.Team, opt => opt.MapFrom(src => new TeamDto { Name = src.Team }))
                .ForMember(m => m.Score, opt => opt.MapFrom(src => new IntegerScore { Value = src.Score }));
        }       
    }
}