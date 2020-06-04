using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds
{
    public class GetTeamLeagueRoundsVm
    {
        public string Name { get; set; }
        public IEnumerable<RoundDto> Rounds { get; set; }
        public string SelectedRound { get; set; }
    }
}