using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Round;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable
{
    public class RoundDto : IMapFrom<TeamLeagueRound>
    {
        public string Name { get; set; }
    }
}