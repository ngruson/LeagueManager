using System;
using System.Collections.Generic;
using LeagueManager.Application.Common.Mappings;
using MediatR;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore
{
    public class UpdateTeamLeagueMatchScoreCommand : IRequest<TeamMatchDto>, IMapFrom<UpdateTeamLeagueMatchScoreDto>
    {
        public string LeagueName { get; set; }
        public Guid Guid { get; set; }
        public List<TeamMatchEntryRequestDto> MatchEntries { get; set; }
    }
}