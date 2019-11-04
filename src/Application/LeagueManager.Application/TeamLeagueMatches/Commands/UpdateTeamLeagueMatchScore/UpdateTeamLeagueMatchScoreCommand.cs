using System;
using LeagueManager.Application.TeamLeagues.Queries.Dto;
using MediatR;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore
{
    public class UpdateTeamLeagueMatchScoreCommand : IRequest<TeamMatchDto>
    {
        public string LeagueName { get; set; }
        public Guid Guid { get; set; }
        public TeamMatchEntryDto HomeMatchEntry { get; set; }
        public TeamMatchEntryDto AwayMatchEntry { get; set; }
    }
}