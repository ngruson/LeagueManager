using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeague
{
    public class GetTeamLeagueVm
    {
        public string LeagueName { get; set; }
        public GetTeamLeagueTableVm Table { get; set; }
        public GetTeamLeagueRoundsVm Rounds { get; set; }
    }
}