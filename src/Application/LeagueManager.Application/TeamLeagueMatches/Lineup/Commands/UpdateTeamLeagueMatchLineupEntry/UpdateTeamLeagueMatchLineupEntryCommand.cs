using LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Lineup.Commands.UpdateTeamLeagueMatchLineupEntry
{
    public class UpdateTeamLeagueMatchLineupEntryCommand : IRequest<LineupEntryDto>
    {
        public string LeagueName { get; set; }
        public Guid MatchGuid { get; set; }
        public string TeamName { get; set; }
        public Guid LineupEntryGuid { get; set; }
        public string PlayerNumber { get; set; }
        public string PlayerName { get; set; }
    }
}