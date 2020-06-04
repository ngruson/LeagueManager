using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Match;
using System;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague
{
    public class TeamMatchDto : IMapFrom<TeamLeagueMatch>
    {
        public Guid Guid { get; set; }
        public List<TeamMatchEntryDto> MatchEntries { get; set; }
    }
}