using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution
{
    public class UpdateTeamLeagueMatchSubstitutionCommand : IRequest<SubstitutionDto>
    {
        public string LeagueName { get; set; }
        public Guid MatchGuid { get; set; }
        public string TeamName { get; set; }
        public Guid SubstitutionGuid { get; set; }
        public string Minute { get; set; }
        public string PlayerOut { get; set; }
        public string PlayerIn { get; set; }
    }
}