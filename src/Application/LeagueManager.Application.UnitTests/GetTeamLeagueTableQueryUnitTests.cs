using FluentAssertions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Application.UnitTests.TestData;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Score;
using LeagueManager.Domain.Sports;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetTeamLeagueTableQueryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamLeague> leagues)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_NoTeamLeaguesExist_When_GetTeamLeagueTable_Then_ReturnNull()
        {
            // Arrange
            var leagues = new List<TeamLeague>();
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueTableQueryHandler(
                contextMock.Object, Mapper.CreateMapper());

            //Act
            var result = await handler.Handle(new GetTeamLeagueTableQuery { LeagueName = "Premier League" }, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        private void SetScores(TeamLeague league, List<Tuple<Tuple<Team, int>, Tuple<Team, int>>> scores)
        {
            int counter = 0;
            while (true)
            {
                foreach (var round in league.Rounds)
                {
                    foreach (var match in round.Matches)
                    {
                        var score = scores[counter];
                        match.MatchEntries.ToList()[0].Team = score.Item1.Item1;
                        match.MatchEntries.ToList()[0].Score = new IntegerScore { Value = score.Item1.Item2 };
                        match.MatchEntries.ToList()[1].Team = score.Item2.Item1;
                        match.MatchEntries.ToList()[1].Score = new IntegerScore { Value = score.Item2.Item2 };
                        counter++;
                        if (counter == scores.Count)
                            return;
                    }
                }
            }
        }

        private Tuple<Tuple<Team, int>, Tuple<Team, int>> AddScore(Team team1, Team team2, int score1, int score2)
        {
            return new Tuple<Tuple<Team, int>, Tuple<Team, int>>(
                new Tuple<Team, int>(team1, score1),
                new Tuple<Team, int>(team2, score2)
            );
        }

        [Fact]
        public async void Given_TeamLeagueExist_When_GetTeamLeagueTable_Then_ReturnTable()
        {
            // Arrange
            var sports = new TeamSports
            {
                Options = new TeamSportsOptions
                {
                    AmountOfPlayers = 11
                }
            };
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .Build();
            league.Sports = sports;
            league.CreateRounds();

            var scores = new List<Tuple<Tuple<Team, int>, Tuple<Team, int>>>
            {
                AddScore(teams[0], teams[1], 1, 0),
                AddScore(teams[2], teams[3], 1, 1),
                AddScore(teams[0], teams[2], 2, 1),
                AddScore(teams[3], teams[1], 0, 1)
            };
            SetScores(league, scores);

            var contextMock = MockDbContext(new TeamLeague[] { league }.AsQueryable());
            var handler = new GetTeamLeagueTableQueryHandler(
                contextMock.Object, Mapper.CreateMapper());

            //Act
            var result = await handler.Handle(new GetTeamLeagueTableQuery { LeagueName = "Premier League" }, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Table.Items.Should().NotBeNull();
            result.Table.Items.Count.Should().Be(4);
            result.Table.Items[0].Position.Should().Be(1);
            result.Table.Items[0].GamesPlayed.Should().Be(2);
            result.Table.Items[0].GamesWon.Should().Be(2);
            result.Table.Items[0].GamesDrawn.Should().Be(0);
            result.Table.Items[0].GamesLost.Should().Be(0);
            result.Table.Items[0].GoalsFor.Should().Be(3);
            result.Table.Items[0].GoalsAgainst.Should().Be(1);
            result.Table.Items[0].GoalDifference.Should().Be(2);
            result.Table.Items[0].Points.Should().Be(6);
        }
    }
}