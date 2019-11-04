using FluentAssertions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Teams.Queries.GetTeams;
using LeagueManager.Domain.Competitor;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetTeamsQueryUnitTests
    {
        [Fact]
        public async void Given_NoTeamsExist_When_GetTeams_Then_ReturnEmptyList()
        {
            // Arrange
            var teams = new List<Team>();
            var contextMock = MockDbContext(teams.AsQueryable());
            var handler = new GetTeamsQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(new GetTeamsQuery(), CancellationToken.None);

            //Assert
            result.Count().Should().Be(0);
        }

        [Fact]
        public async void Given_TeamsExist_When_GetTeams_Then_ReturnOrderedList()
        {
            // Arrange
            var teams = new List<Team> {
                new Team {  Name = "Tottenham Hotspur"},
                new Team {  Name = "Manchester City"},
                new Team {  Name = "Liverpool"}
            };
            var contextMock = MockDbContext(teams.AsQueryable());
            var handler = new GetTeamsQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(new GetTeamsQuery(), CancellationToken.None);

            //Assert
            var orderedList = teams
                .OrderBy(t => t.Name)
                .Select(t => new TeamDto { Name = t.Name });
            result.Count().Should().Be(3);
            result.SequenceEqual(orderedList);
        }

        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<Team> teams)
        {
            var teamsDbSet = teams.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.Teams).Returns(teamsDbSet.Object);
            return mockContext;
        }
    }
}