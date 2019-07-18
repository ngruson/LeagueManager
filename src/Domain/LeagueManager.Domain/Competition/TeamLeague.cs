using System.Collections.Generic;
using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Round;

namespace LeagueManager.Domain.Competition
{
    public class TeamLeague : ITeamLeague
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public byte[] Logo { get; set; }
        public List<TeamCompetitor> Teams { get; set; } = new List<TeamCompetitor>();
        public List<TeamLeagueRound> Rounds { get; set; } = new List<TeamLeagueRound>();
    }
}