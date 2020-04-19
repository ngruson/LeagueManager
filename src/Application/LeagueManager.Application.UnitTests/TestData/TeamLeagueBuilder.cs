using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Round;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeagueManager.Application.UnitTests.TestData
{
    public class TeamLeagueBuilder
    {
        private readonly string name = "Premier League";
        private List<Domain.Competitor.TeamCompetitor> competitors;
        private List<TeamLeagueRound> rounds;
        
        public TeamLeagueBuilder WithCompetitors(List<Team> teams)
        {
            competitors = teams.Select(x =>
            {
                return new Domain.Competitor.TeamCompetitor {  Team = x };
            }).ToList();

            return this;
        }

        public TeamLeagueBuilder WithPlayers(Team team, List<Domain.Player.Player> players)
        {
            var competitor = competitors.SingleOrDefault(c => c.Team == team);

            foreach (var p in players)
            {
                var cp = new TeamCompetitorPlayer
                {
                    Number = (players.IndexOf(p) + 1).ToString(),
                    Player = p
                };
                competitor.Players.Add(cp);
            }
            return this;
        }

        public TeamLeagueBuilder WithRounds()
        {
            rounds = new List<TeamLeagueRound>
            {
                new TeamLeagueRound
                {
                    Name = "Round 1"
                }
            };
            rounds[0].Matches = CreateMatches(rounds[0], competitors);

            return this;
        }

        public TeamLeagueBuilder WithGoals()
        {
            var match = rounds[0].Matches[0];
            var matchEntry = match.MatchEntries.ToList()[0];
            matchEntry.Goals = new List<TeamMatchEntryGoal>
            {
                new TeamMatchEntryGoal
                {
                    TeamMatchEntry = matchEntry,
                    Guid = Guid.NewGuid(),
                    Minute = new Random().Next(1, 45).ToString()
                },
                new TeamMatchEntryGoal
                {
                    TeamMatchEntry = matchEntry,
                    Guid = Guid.NewGuid(),
                    Minute = new Random().Next(46, 90).ToString()
                }
            };

            return this;
        }

        private static List<TeamLeagueMatch> CreateMatches(TeamLeagueRound round, List<Domain.Competitor.TeamCompetitor> competitors)
        {            
            var matchEntries = new List<TeamMatchEntry>();

            var matchEntry = new TeamMatchEntry
            {
                HomeAway = HomeAway.Home,
                Team = competitors[0].Team
            };
            matchEntry.Lineup = CreateLineup(matchEntry, 0);
            matchEntries.Add(matchEntry);

            matchEntry = new TeamMatchEntry
            {
                HomeAway = HomeAway.Away,
                Team = competitors[1].Team
            };
            matchEntry.Lineup = CreateLineup(matchEntry, 11);
            matchEntries.Add(matchEntry);

            return new List<TeamLeagueMatch>
            {
                new TeamLeagueMatch
                {
                    Guid = new Guid("00000000-0000-0000-0000-000000000000"),
                    TeamLeagueRound = round,
                    MatchEntries = matchEntries
                }
            };
        }

        private static List<TeamMatchEntryLineupEntry> CreateLineup(TeamMatchEntry matchEntry, int totalCounter)
        {
            var list = new List<TeamMatchEntryLineupEntry>();
            for (int i = 0; i < 11; i++)
            {
                int counter = totalCounter + i;
                string guidValue = "00000000-0000-0000-0000-000000000000";
                guidValue = guidValue.Substring(0, guidValue.Length - 1 - (counter.ToString().Length - 1)) + counter.ToString();

                list.Add(new TeamMatchEntryLineupEntry
                {
                    Guid = new Guid(guidValue),
                    TeamMatchEntry = matchEntry
                });
            }
            return list;
        }

        public TeamLeague Build()
        {
            var teamLeague = new TeamLeague { Name = name };

            if (competitors != null)
                teamLeague.Competitors.AddRange(competitors);

            if (rounds != null)
            {
                rounds.ForEach(r => r.TeamLeague = teamLeague);
                teamLeague.Rounds.AddRange(rounds);
            }
                

            return teamLeague;
        }
    }

}