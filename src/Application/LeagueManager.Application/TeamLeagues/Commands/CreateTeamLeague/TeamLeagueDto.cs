using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Competition;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague
{
    public class TeamLeagueDto : IMapFrom<TeamLeague>
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public List<RoundDto> Rounds { get; set; }
    }
}