using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution
{
    public class GetTeamLeagueMatchSubstitutionQuery : IRequest<SubstitutionDto>
    {
        public string LeagueName { get; set; }
        public Guid MatchGuid { get; set; }
        public string TeamName { get; set; }
        public Guid SubstitutionGuid { get; set; }
    }
}