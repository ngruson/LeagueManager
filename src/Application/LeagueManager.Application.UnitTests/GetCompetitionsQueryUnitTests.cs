using FluentAssertions;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Common;
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
    public class GetCompetitionsQueryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamLeague> leagues)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_TeamLeaguesExist_When_GetCompetitions_Then_TeamLeaguesAreReturned()
        {
            // Arrange
            var leagues = new List<TeamLeague>
            {
                new TeamLeague
                {
                    Name = "Premier League"
                },
                new TeamLeague
                {
                    Name = "Primera Division"
                }
            };
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetCompetitionsQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(
                new GetCompetitionsQuery(),
                CancellationToken.None);

            //Assert
            result.Count().Should().Be(2);
        }

        [Fact]
        public async void Given_NoCompetitionsExist_When_GetCompetitions_Then_EmptyListIsReturned()
        {
            // Arrange
            var leagues = new List<TeamLeague>();
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetCompetitionsQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(
                new GetCompetitionsQuery(),
                CancellationToken.None);

            //Assert
            result.Count().Should().Be(0);
        }

        [Fact]
        public async void Given_CompetitionsWithCountryExist_When_GetCompetitionsByCountry_Then_CompetitionsAreReturned()
        {
            // Arrange
            var leagues = new List<TeamLeague>
            {
                new TeamLeague
                {
                    Name = "Premier League 2018-2019",
                    Country = new Country { Name = "England" }
                },
                new TeamLeague
                {
                    Name = "Premier League 2019-2020",
                    Country = new Country { Name = "England" }
                },
                new TeamLeague
                {
                    Name = "Primera Division 2019-2020",
                    Country = new Country { Name = "Spain" }
                },
            };
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetCompetitionsQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(
                new GetCompetitionsQuery {  Country = "England" },
                CancellationToken.None);

            //Assert
            result.Count().Should().Be(2);
        }
    }
}