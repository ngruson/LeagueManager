using AutoMapper;
using FluentAssertions;
using LeagueManager.Application.AutoMapper;
using LeagueManager.Application.Competitions.Queries.GetTeamLeague;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Round;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetTeamLeagueUnitTests
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
                opts.AddProfile<ApplicationProfile>();
            });

            return config.CreateMapper();
        }

        private List<TeamCompetitor> CreateCompetitors()
        {
            return new List<TeamCompetitor>
            {
                new TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Liverpool"
                    }
                },
                new TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Manchester City"
                    }
                }
            };
        }

        [Fact]
        public async void Given_NoTeamLeaguesExist_When_GetTeamLeague_Then_ReturnNull()
        {
            // Arrange
            var leagues = new List<TeamLeague>();
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueQueryHandler(
                contextMock.Object, CreateMapper());

            //Act
            var result = await handler.Handle(new GetTeamLeagueQuery { Name = "Premier League" }, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async void Given_TeamLeagueExist_When_GetTeamLeague_Then_ReturnTeamLeague()
        {
            // Arrange
            var leagues = new List<TeamLeague> {
                new TeamLeague
                {
                    Name = "Premier League",
                    Competitors = CreateCompetitors(),
                    Rounds = new List<TeamLeagueRound>
                    {
                        new TeamLeagueRound
                        {
                            Name = "Round 1"
                        },
                        new TeamLeagueRound
                        {
                            Name = "Round 2"
                        }
                    }
                },
                new TeamLeague { Name = "Primera Division" }
            };

            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueQueryHandler(
                contextMock.Object, CreateMapper());

            //Act
            var result = await handler.Handle(new GetTeamLeagueQuery { Name = "Premier League" }, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Competitors.Count().Should().Be(2);
            result.Rounds.Count().Should().Be(2);
        }
    }
}