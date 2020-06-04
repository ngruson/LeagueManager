using LeagueManager.Application.Interfaces.Dto;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class LineupEntryDto : /*IMapFrom<Domain.Match.TeamMatchEntryLineupEntry>,*/ ILineupEntryDto
    {
        public Guid Guid { get; set; }
        public string PlayerNumber { get; set; }
        public IPlayerDto Player { get; set; }
        public string TeamMatchEntryTeamName { get; set; }

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<Domain.Match.TeamMatchEntryLineupEntry, ILineupEntryDto>()
        //        .ForMember(m => m.PlayerNumber, opt => opt.MapFrom(src => src.Number))
        //        .ForMember(m => m.TeamName, opt => opt.MapFrom(src =>
        //            src.TeamMatchEntry != null && src.TeamMatchEntry.Team != null ? src.TeamMatchEntry.Team.Name : null))
        //        .As<LineupEntryDto>();
        //}
    }
}