using System.Collections.Generic;
using System.Linq;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Points;
using LeagueManager.Domain.Round;

namespace LeagueManager.Domain.LeagueTable
{
    public class TeamLeagueTable : ILeagueTable
    {
        public List<TeamLeagueTableItem> Items { get; set; } = new List<TeamLeagueTableItem>();

        public void CalculateTable(
            List<Team> teams, 
            List<TeamLeagueRound> rounds,
            PointSystem pointSystem)
        {
            Items.Clear();

            teams.ForEach(t =>
            {
                var matches = GetMatchesForTeam(t, rounds);
                var matchEntries = GetMatchEntriesForTeam(t, rounds);
                var item = new TeamLeagueTableItem
                {
                    Team = t,
                    GamesPlayed = matchEntries.Count(),
                    GamesWon = matches.Count(m => m.Winner == t),
                    GamesLost = matches.Count(m => m.Loser == t),
                    GamesDrawed = matches.Count(m => m.IsDraw),
                    GoalsFor = matches.Sum(m => m.GetGoalsFor(t)),
                    GoalsAgainst = matches.Sum(m => m.GetGoalsAgainst(t)),
                    Points = matches.Sum(m => m.GetPointsFor(t, pointSystem)),
                };
                Items.Add(item);
            });

            Items = Items.OrderByDescending(i => i.Points)
                .ThenByDescending(i => i.GoalDifference)
                .ThenByDescending(i => i.GoalsFor)
                .ThenBy(i => i.Team.Name).ToList();

            int position = 0;
            foreach (var item in Items)
            {
                position++;
                item.Position = position;
            }
        }

        private List<TeamMatch> GetMatchesForTeam(
            Team team,
            List<TeamLeagueRound> rounds)
        {
            return rounds.SelectMany(r =>
                r.Matches.Where(m =>
                    m.MatchEntries.Exists(me => me.Team.Name == team.Name))).ToList();
        }

        private List<TeamMatchEntry> GetMatchEntriesForTeam(
            Team team, 
            List<TeamLeagueRound> rounds)
        {
            return rounds.SelectMany(r =>
                r.Matches.SelectMany(m =>
                    m.MatchEntries.Where(me => me.Team.Name == team.Name))).ToList();
        }
    }
}