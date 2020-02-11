using AutoMapper;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Domain.Match;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Lineup
{
    public class LineupProfile : Profile
    {
        public LineupProfile()
        {
            Guid matchGuid = Guid.Empty;
            Guid lineupEntryGuid = Guid.Empty;

            CreateMap<TeamMatchEntryLineupEntry, LineupEntryDto>()
                .ForMember(m => m.PlayerNumber, opt => opt.MapFrom(src => src.Number))
                .ForMember(m => m.Player, opt => opt.MapFrom(src => src.Player))
                .ForMember(m => m.TeamName, opt => opt.MapFrom(src => src.TeamMatchEntry.Team.Name));

            CreateMap<Domain.Player.Player, PlayerDto>();

            CreateMap<UpdateTeamLeagueMatchLineupEntryCommand, GetTeamLeagueMatchLineupEntryQuery>();
        }
    }
}