using FluentAssertions;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Competition;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetCompetitionQueryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamLeague> leagues)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_TeamLeagueExist_When_GetCompetition_Then_TeamLeagueIsReturned()
        {
            // Arrange
            var leagues = new List<TeamLeague>
            {
                new TeamLeague
                {
                    Name = "Premier League"
                }
            };
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetCompetitionQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(
                new GetCompetitionQuery { Name = "Premier League" }, 
                CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void Given_TeamLeagueDoesNotExist_When_GetCompetition_Then_NullIsReturned()
        {
            // Arrange
            var leagues = new List<TeamLeague>();
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetCompetitionQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(
                new GetCompetitionQuery { Name = "Premier League" },
                CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }
    }
}