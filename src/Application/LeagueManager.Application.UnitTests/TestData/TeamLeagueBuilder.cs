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

        private static List<TeamLeagueMatch> CreateMatches(TeamLeagueRound round, List<Domain.Competitor.TeamCompetitor> competitors)
        {            
            var matchEntries = new List<TeamMatchEntry>();

            var matchEntry = new TeamMatchEntry
            {
                HomeAway = HomeAway.Home,
                Team = competitors[0].Team
            };
            matchEntry.Lineup = CreateLineup(matchEntry);
            matchEntries.Add(matchEntry);

            matchEntry = new TeamMatchEntry
            {
                HomeAway = HomeAway.Away,
                Team = competitors[1].Team
            };
            matchEntry.Lineup = CreateLineup(matchEntry);
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

        private static List<TeamMatchEntryLineupEntry> CreateLineup(TeamMatchEntry matchEntry)
        {
            var list = new List<TeamMatchEntryLineupEntry>();
            for (int i = 0; i < 11; i++)
            {
                list.Add(new TeamMatchEntryLineupEntry
                {
                    Guid = Guid.NewGuid(),
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