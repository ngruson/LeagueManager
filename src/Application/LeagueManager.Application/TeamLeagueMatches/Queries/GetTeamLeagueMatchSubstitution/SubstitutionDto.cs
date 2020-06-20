using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Match;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution
{
    public class SubstitutionDto : IMapFrom<TeamMatchEntrySubstitution>, ISubstitutionDto
    {
        public Guid Guid { get; set; }
        public string TeamMatchEntryTeamName { get; set; }
        public string Minute { get; set; }
        public IPlayerDto PlayerIn { get; set; }
        public IPlayerDto PlayerOut { get; set; }
    }
}