using System;
using System.Collections.Generic;
using System.Linq;
using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.LeagueTable;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Points;
using LeagueManager.Domain.Round;

namespace LeagueManager.Domain.Competition
{
    public class TeamLeague : ITeamLeague
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public byte[] Logo { get; set; }
        public PointSystem PointSystem { get; set; } = new PointSystem();
        public List<TeamCompetitor> Competitors { get; set; } = new List<TeamCompetitor>();
        public List<TeamLeagueRound> Rounds { get; set; } = new List<TeamLeagueRound>();
        public TeamLeagueTable Table { get; private set; } = new TeamLeagueTable();

        public void CalculateTable()
        {
            Table.CalculateTable(Competitors, Rounds, PointSystem);
        }

        public void CreateRounds()
        {
            int amountOfRounds = (Competitors.Count - 1) * 2;
            for (int r = 1; r <= amountOfRounds; r++)
            {
                var round = new TeamLeagueRound
                {
                    Name = $"Round {r}",
                };
                for (int m = 0; m < Competitors.Count / 2; m++)
                {
                    var match = new TeamMatch { Guid = Guid.NewGuid() };
                    match.MatchEntries.Add(new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home
                    });
                    match.MatchEntries.Add(new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away
                    });

                    round.Matches.Add(match);
                }

                Rounds.Add(round);
            }
        }

        public TeamMatch GetMatch(Guid guid)
        {
            TeamMatch match = null;

            if (Rounds != null)
                foreach (var round in Rounds)
                {
                    match = round.Matches.SingleOrDefault(m => m.Guid == guid);
                    if (match != null)
                        break;
                }

            return match;
        }
    }
}