using LeagueManager.Application.Interfaces.Dto;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class SubstitutionDto : ISubstitutionDto
    {
        public Guid Guid { get; set; }
        public string TeamMatchEntryTeamName { get; set; }
        public string Minute { get; set; }
        public IPlayerDto PlayerIn { get; set; }
        public IPlayerDto PlayerOut { get; set; }
    }
}