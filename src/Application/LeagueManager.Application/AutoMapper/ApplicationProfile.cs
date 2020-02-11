using AutoMapper;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.Application.Match.Commands.AddPlayerToLineup;
using LeagueManager.Application.Player.Commands.CreatePlayer;
using LeagueManager.Application.Player.Dto;
using LeagueManager.Application.TeamCompetitor.Dto;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using Lineup = LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using LeagueManager.Application.TeamLeagues.Dto;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.LeagueTable;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Round;
using LeagueManager.Domain.Score;
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

            CreateMap<TeamLeagueRound, TeamLeagueRoundDto>()
                .ForMember(m => m.TeamLeague, opt => opt.MapFrom(src => src.TeamLeague.Name));
            CreateMap<TeamLeagueTable, TeamLeagueTableDto>();
            CreateMap<TeamLeagueTableItem, TeamLeagueTableItemDto>();
            CreateMap<Team, TeamDto>();
            CreateMap<TeamLeagueMatch, TeamMatchDto>()
                .ForMember(m => m.TeamLeague, opt => opt.MapFrom(src => src.TeamLeagueRound.TeamLeague.Name))
                .ForMember(m => m.TeamLeagueRound, opt => opt.MapFrom(src => src.TeamLeagueRound.Name));
            CreateMap<TeamMatchEntry, TeamMatchEntryDto>()
                .ForMember(m => m.Team, opt => opt.MapFrom(src => src.Team));
            CreateMap<Domain.Match.HomeAway, TeamLeagueMatches.Dto.HomeAway>();
            CreateMap<IntegerScore, IntegerScoreDto>();
            CreateMap<TeamMatchEntryLineupEntry, Lineup.LineupEntryDto>()
                .ForMember(m => m.TeamName, opt => opt.MapFrom(src => src.TeamMatchEntry.Team.Name));
            CreateMap<Domain.Player.Player, PlayerDto>();
            CreateMap<TeamCompetitorPlayer, TeamCompetitorPlayerDto>()
                .ForMember(m => m.Player, opt => opt.MapFrom(src => src.Player));
            
            CreateMap<CreatePlayerCommand, Domain.Player.Player>();

            CreateMap<UpdateTeamLeagueMatchDto, UpdateTeamLeagueMatchCommand>()
                .ForMember(m => m.LeagueName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["leagueName"]))
                .ForMember(m => m.Guid, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["guid"]));

            CreateMap<UpdateScoreDto, UpdateTeamLeagueMatchScoreCommand>()
                .ForMember(m => m.LeagueName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["leagueName"]))
                .ForMember(m => m.Guid, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["guid"]));

            CreateMap<AddPlayerToLineupDto, AddPlayerToLineupCommand>()
                .ForMember(m => m.LeagueName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["leagueName"]))
                .ForMember(m => m.Guid, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["guid"]))
                .ForMember(m => m.Team, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["teamName"]));

            CreateMap<UpdateLineupEntryDto, UpdateTeamLeagueMatchLineupEntryCommand>()
                .ForMember(m => m.LeagueName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["leagueName"]))
                .ForMember(m => m.MatchGuid, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["matchGuid"]))
            .ForMember(m => m.TeamName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["teamName"]))
            .ForMember(m => m.LineupEntryGuid, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["lineupEntryGuid"]));
        }
    }
}