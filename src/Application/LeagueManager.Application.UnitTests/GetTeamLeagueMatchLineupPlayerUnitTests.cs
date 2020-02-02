using FluentAssertions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.UnitTests.TestData;
using LeagueManager.Domain.Competition;
using MockQueryable.Moq;
using Moq;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetTeamLeagueMatchLineupPlayerUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamLeague> leagues)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_LineupPlayerDoesExist_When_GetTeamLeagueMatchLineupPlayer_Then_ReturnLineupPlayer()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .Build();

            var leagues = Enumerable.Repeat(league, 1);
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueMatchLineupEntryQueryHandler(
                contextMock.Object, Mapper.MapperConfig()
            );
            var match = league.Rounds[0].Matches[0];
            var lineupEntryGuid = match.MatchEntries.First().Lineup.First().Guid;

            //Act
            var request = new GetTeamLeagueMatchLineupEntryQuery
            {
                LeagueName = "Premier League",
                MatchGuid = match.Guid,
                LineupEntryGuid = lineupEntryGuid
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Guid.Should().Be(lineupEntryGuid);
        }

        [Fact]
        public async void Given_LeagueDoesNotExist_When_GetTeamLeagueMatchLineupPlayer_Then_ReturnNull()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .Build();

            var leagues = Enumerable.Repeat(league, 1);
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueMatchLineupEntryQueryHandler(
                contextMock.Object, Mapper.MapperConfig()
            );
            var match = league.Rounds[0].Matches[0];
            var lineupPlayerGuid = match.MatchEntries.First().Lineup.First().Guid;

            //Act
            var request = new GetTeamLeagueMatchLineupEntryQuery
            {
                LeagueName = "DoesNotExist",
                MatchGuid = match.Guid,
                LineupEntryGuid = lineupPlayerGuid
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async void Given_MatchDoesNotExist_When_GetTeamLeagueMatchLineupPlayer_Then_ReturnNull()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .Build();

            var leagues = Enumerable.Repeat(league, 1);
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueMatchLineupEntryQueryHandler(
                contextMock.Object, Mapper.MapperConfig()
            );
            var match = league.Rounds[0].Matches[0];
            var lineupPlayerGuid = match.MatchEntries.First().Lineup.First().Guid;

            //Act
            var request = new GetTeamLeagueMatchLineupEntryQuery
            {
                LeagueName = "Premier League",
                MatchGuid = match.Guid,
                LineupEntryGuid = lineupPlayerGuid
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async void Given_LineupPlayerDoesNotExist_When_GetTeamLeagueMatchLineupPlayer_Then_ReturnNull()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .Build();

            var leagues = Enumerable.Repeat(league, 1);
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueMatchLineupEntryQueryHandler(
                contextMock.Object, Mapper.MapperConfig()
            );
            var matchGuid = league.Rounds[0].Matches[0].Guid;
            var lineupPlayerGuid = Guid.NewGuid();

            //Act
            var request = new GetTeamLeagueMatchLineupEntryQuery
            {
                LeagueName = "Premier League",
                MatchGuid = matchGuid,
                LineupEntryGuid = lineupPlayerGuid
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }
    }
}
