using AutoMapper;
using FluentAssertions;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile<MappingProfile>();
            });

            return config.CreateMapper();
        }

        private List<Domain.Competitor.TeamCompetitor> CreateCompetitors()
        {
            return new List<Domain.Competitor.TeamCompetitor>
            {
                new Domain.Competitor.TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Liverpool"
                    }
                },
                new Domain.Competitor.TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Manchester City"
                    }
                }
            };
        }

        [Fact]
        public async void Given_TeamLeagueExist_When_GetCompetition_Then_TeamLeagueIsReturned()
        {
            // Arrange
            var leagues = new List<TeamLeague>
            {
                new TeamLeague
                {
                    Name = "Premier League",
                    Country = new Country { Name = "England" },
                    Competitors = CreateCompetitors()
                }
            };
            var contextMock = MockDbContext(leagues.AsQueryable());
            var loggerMock = new Mock<ILogger<GetCompetitionQueryHandler>>();
            var handler = new GetCompetitionQueryHandler(
                contextMock.Object, 
                CreateMapper(),
                loggerMock.Object);

            //Act
            var result = await handler.Handle(
                new GetCompetitionQuery { Name = "Premier League" }, 
                CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Country.Should().Be("England");
            result.Competitors.Count().Should().Be(2);
            result.Competitors[0].Should().Be("Liverpool");
            result.Competitors[1].Should().Be("Manchester City");
        }

        [Fact]
        public async void Given_TeamLeagueDoesNotExist_When_GetCompetition_Then_NullIsReturned()
        {
            // Arrange
            var leagues = new List<TeamLeague>();
            var contextMock = MockDbContext(leagues.AsQueryable());
            var loggerMock = new Mock<ILogger<GetCompetitionQueryHandler>>();
            var handler = new GetCompetitionQueryHandler(
                contextMock.Object, 
                CreateMapper(),
                loggerMock.Object
           );

            //Act
            var result = await handler.Handle(
                new GetCompetitionQuery { Name = "Premier League" },
                CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }
    }
}