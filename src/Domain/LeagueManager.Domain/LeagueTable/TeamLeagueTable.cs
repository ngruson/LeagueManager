﻿using System.Collections.Generic;
using System.Linq;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Points;
using LeagueManager.Domain.Round;

namespace LeagueManager.Domain.LeagueTable
{
    public class TeamLeagueTable : ILeagueTable
    {
        public virtual List<TeamLeagueTableItem> Items { get; set; } = new List<TeamLeagueTableItem>();

        public void CalculateTable(
            List<TeamCompetitor> teams, 
            List<TeamLeagueRound> rounds,
            PointSystem pointSystem)
        {
            Items.Clear();

            teams.Select(t => t.Team)
                .ToList()
                .ForEach(t =>
                    {
                        var matches = GetMatchesForTeam(t, rounds);
                        var item = new TeamLeagueTableItem
                        {
                            Team = t,
                            GamesPlayed = matches.Count,
                            GamesWon = matches.Count(m => m.Winner == t),
                            GamesLost = matches.Count(m => m.Loser == t),
                            GamesDrawn = matches.Count(m => m.IsDraw),
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
            Items.ForEach(i =>
            {
                position++;
                i.Position = position;
            });
        }

        private List<TeamLeagueMatch> GetMatchesForTeam(
            Team team,
            List<TeamLeagueRound> rounds)
        {
            return rounds.SelectMany(r =>
                r.Matches.Where(m =>
                    m.MatchEntries.Any(me => 
                        me.Team != null && 
                        me.Team.Name == team.Name &&
                        me.Score != null)))
                .ToList();
        }
    }
}