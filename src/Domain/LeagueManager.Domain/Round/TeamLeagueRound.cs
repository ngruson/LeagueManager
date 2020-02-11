using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Match;
using System.Collections.Generic;

namespace LeagueManager.Domain.Round
{
    public class TeamLeagueRound : ITeamLeagueRound
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TeamLeagueMatch> Matches { get; set; } = new List<TeamLeagueMatch>();
        public TeamLeague TeamLeague { get; set; }
    }
}