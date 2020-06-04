using AutoMapper;
using LeagueManager.Application.Common.Mappings;
using Dto = LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Match;
using LeagueManager.Application.Interfaces.Dto;
using System;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds
{
    public class TeamMatchEntryDto : IMapFrom<TeamMatchEntry>, ITeamMatchEntryDto
    {
        public Guid TeamLeagueMatchGuid { get; set; }
        public ITeamDto Team { get; set; }
        public Dto.HomeAway HomeAway { get; set; }
        public IIntegerScoreDto Score { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamMatchEntry, ITeamMatchEntryDto>()
                .ForMember(m => m.HomeAway, opt => opt.MapFrom(src =>
                    src.HomeAway == Domain.Match.HomeAway.Home ? Dto.HomeAway.Home : Dto.HomeAway.Away))
                .As<TeamMatchEntryDto>();
        }
    }
}