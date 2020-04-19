using FluentAssertions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
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
    public class GetTeamLeagueMatchDetailsQueryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamLeague> leagues)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_MatchDoesNotExist_When_GetTeamLeagueMatchDetails_Then_ReturnNull()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .Build();
            var leagues = Enumerable.Repeat(league, 1);
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueMatchDetailsQueryHandler(
                contextMock.Object, Mapper.MapperConfig()
            );

            //Act
            var request = new GetTeamLeagueMatchDetailsQuery
            {
                LeagueName = "Premier League",
                Guid = new Guid("00000000-0000-0000-0000-000000000001")
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async void Given_MatchDoesExist_When_GetTeamLeagueMatchDetails_Then_ReturnMatch()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .Build();
            var leagues = Enumerable.Repeat(league, 1);
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueMatchDetailsQueryHandler(
                contextMock.Object, Mapper.MapperConfig()
            );

            //Act
            var guid = new Guid("00000000-0000-0000-0000-000000000000");
            var request = new GetTeamLeagueMatchDetailsQuery
            {
                LeagueName = "Premier League",
                Guid = guid
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Guid.Should().Be(guid);
            result.MatchEntries[0].Lineup.Count.Should().Be(11);
            result.MatchEntries[1].Lineup.Count.Should().Be(11);
        }
    }
}