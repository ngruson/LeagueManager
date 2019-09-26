using System;
using System.Collections.Generic;
using FluentAssertions;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using Xunit;

namespace LeagueManager.Domain.UnitTests
{
    public class TeamLeagueRoundsUnitTests
    {
        private List<TeamCompetitor> CreateCompetitors()
        {
            return new List<TeamCompetitor>
            {
                new TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Team A"
                    }
                },
                new TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Team B"
                    }
                },
                new TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Team C"
                    }
                },
                new TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Team D"
                    }
                }
            };
        }

        [Fact]
        public void Given_TeamsAreAdded_When_CreateRounds_Then_CreateAllRounds()
        {
            //Arrange
            var league = new TeamLeague
            {
                Competitors = CreateCompetitors()
            };

            //Act
            league.CreateRounds();

            //Assert
            league.Rounds.Count.Should().Be(6);
            league.Rounds.ForEach(r =>
                r.Matches.Count.Should().Be(2)
            );
        }
    }
}