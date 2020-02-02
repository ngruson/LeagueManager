using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Round;
using LeagueManager.Domain.Sports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeagueManager.Domain.UnitTests.TestData
{
    public class TeamLeagueBuilder
    {
        private readonly string name = "Premier League";
        private List<Domain.Competitor.TeamCompetitor> competitors;
        private List<TeamLeagueRound> rounds;
        private TeamSports sports;

        public TeamLeagueBuilder WithSports(TeamSports sports)
        {
            this.sports = sports;
            return this;
        }

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
            return new List<TeamLeagueMatch>
            {
                new TeamLeagueMatch
                {
                    Guid = new Guid("00000000-0000-0000-0000-000000000000"),
                    TeamLeagueRound = round,
                    MatchEntries = new List<TeamMatchEntry>
                    {
                        new TeamMatchEntry
                        {
                            HomeAway = HomeAway.Home,
                            Team = competitors[0].Team,
                            Lineup = Enumerable.Repeat(new TeamMatchEntryLineupEntry(), 11).ToList()
                        },
                        new TeamMatchEntry
                        {
                            HomeAway = HomeAway.Away,
                            Team = competitors[1].Team,
                            Lineup = Enumerable.Repeat(new TeamMatchEntryLineupEntry(), 11).ToList()
                        }
                    }
                }
            };
        }

        public TeamLeague Build()
        {
            var teamLeague = new TeamLeague { Name = name };
            teamLeague.Sports = sports;

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