using FluentAssertions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Sports.Queries.GetTeamSports;
using LeagueManager.Domain.Sports;
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
    public class GetTeamSportsQueryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamSports> sports)
        {
            var teamSportsDbSet = sports.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamSports).Returns(teamSportsDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_TeamSportsExist_When_GetTeamSports_Then_ReturnOrderedList()
        {
            // Arrange
            var sports = new List<TeamSports> {
                new TeamSports {  Name = "Soccer", Options = new TeamSportsOptions { AmountOfPlayers = 11 } },
                new TeamSports {  Name = "Baseball",  Options = new TeamSportsOptions { AmountOfPlayers = 9 } },
                new TeamSports {  Name = "Basketball", Options = new TeamSportsOptions { AmountOfPlayers = 5 } }
            };
            var contextMock = MockDbContext(sports.AsQueryable());
            var handler = new GetTeamSportsQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(new GetTeamSportsQuery(), CancellationToken.None);

            //Assert
            var orderedList = sports
                .OrderBy(t => t.Name)
                .Select(t => new TeamSportDto { Name = t.Name });
            result.Count().Should().Be(3);
            result.SequenceEqual(orderedList);
        }

        [Fact]
        public async void Given_TeamSportsDoNotExist_When_GetTeamSports_Then_ReturnNull()
        {
            // Arrange
            var sports = new List<TeamSports>();
            var contextMock = MockDbContext(sports.AsQueryable());
            var handler = new GetTeamSportsQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(new GetTeamSportsQuery(), CancellationToken.None);

            //Assert
            result.Count().Should().Be(0);
        }
    }
}