using AutoMapper;
using FluentAssertions;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile<MappingProfile>();
            });

            return config.CreateMapper();
        }

        private List<TeamLeague> CreateTeamLeagues()
        {
            return new List<TeamLeague>
            {
                new TeamLeague
                {
                    Name = "Premier League 2018-2019",
                    Country = new Country { Name = "England" },
                    Competitors = CreateCompetitorsForEngland()
                },
                new TeamLeague
                {
                    Name = "Premier League 2019-2020",
                    Country = new Country { Name = "England" },
                    Competitors = CreateCompetitorsForEngland()
                },
                new TeamLeague
                {
                    Name = "Primera Division 2019-2020",
                    Country = new Country { Name = "Spain" },
                    Competitors = CreateCompetitorsForSpain()
                }
            };
        }

        private List<Domain.Competitor.TeamCompetitor> CreateCompetitorsForEngland()
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

        private List<Domain.Competitor.TeamCompetitor> CreateCompetitorsForSpain()
        {
            return new List<Domain.Competitor.TeamCompetitor>
            {
                new Domain.Competitor.TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "FC Barcelona"
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
        public async void Given_TeamLeaguesExist_When_GetCompetitions_Then_TeamLeaguesAreReturned()
        {
            // Arrange
            var leagues = CreateTeamLeagues();
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetCompetitionsQueryHandler(
                contextMock.Object,
                CreateMapper());

            //Act
            var result = await handler.Handle(
                new GetCompetitionsQuery(),
                CancellationToken.None);

            //Assert
            result.Count().Should().Be(3);
        }

        [Fact]
        public async void Given_NoCompetitionsExist_When_GetCompetitions_Then_EmptyListIsReturned()
        {
            // Arrange
            var leagues = new List<TeamLeague>();
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetCompetitionsQueryHandler(
                contextMock.Object,
                CreateMapper());

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
            var leagues = CreateTeamLeagues();
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetCompetitionsQueryHandler(
                contextMock.Object,
                CreateMapper());

            //Act
            var result = await handler.Handle(
                new GetCompetitionsQuery { Country = "England" },
                CancellationToken.None);

            //Assert
            result.Count().Should().Be(2);
            var list = result.ToList();
            list[0].Name.Should().Be("Premier League 2018-2019");
            list[0].Country.Should().Be("England");
            list[0].Competitors.Count().Should().Be(2);

            list[1].Name.Should().Be("Premier League 2019-2020");
            list[1].Country.Should().Be("England");
            list[1].Competitors.Count().Should().Be(2);
        }
    }
}