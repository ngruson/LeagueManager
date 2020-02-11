using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.UnitTests.TestData;
using LeagueManager.Domain.Competition;
using MockQueryable.Moq;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetTeamLeagueMatchLineupEntryQueryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamLeague> leagues)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            return mockContext;
        }
        
        [Fact]
        public async void Given_LineupEntryDoesExist_When_GetTeamLeagueMatchLineupEntry_Then_ReturnLineupEntry()
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
        public void Given_LeagueDoesNotExist_When_GetTeamLeagueMatchLineupEntry_Then_ReturnNull()
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
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<LineupEntryNotFoundException>();
        }

        [Fact]
        public void Given_MatchDoesNotExist_When_GetTeamLeagueMatchLineupEntry_Then_ThrowLineupEntryNotFoundException()
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

            //Act
            var request = new GetTeamLeagueMatchLineupEntryQuery
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000001"),
                LineupEntryGuid = new Guid("00000000-0000-0000-0000-000000000001")
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<LineupEntryNotFoundException>();
        }

        [Fact]
        public void Given_LineupEntryDoesNotExist_When_GetTeamLeagueMatchLineupEntry_Then_ThrowLineupEntryNotFoundException()
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
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<LineupEntryNotFoundException>();
        }
    }
}
