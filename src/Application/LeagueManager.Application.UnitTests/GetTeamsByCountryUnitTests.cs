using FluentAssertions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Teams.Queries.GetTeamsByCountry;
using LeagueManager.Domain.Common;
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
    public class GetTeamsByCountryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<Team> teams)
        {
            var teamsDbSet = teams.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.Teams).Returns(teamsDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_NoTeamsExist_When_GetTeamsByCountry_Then_ReturnEmptyList()
        {
            // Arrange
            var teams = new List<Team>();
            var contextMock = MockDbContext(teams.AsQueryable());
            var handler = new GetTeamsByCountryQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(new GetTeamsByCountryQuery(), CancellationToken.None);

            //Assert
            result.Count().Should().Be(0);
        }

        [Fact]
        public async void Given_TeamsExist_When_GetTeamsByCountry_Then_ReturnOrderedList()
        {
            // Arrange
            var country = new Country { Name = "England" };
            var teams = new List<Team> {
                new Team {  Name = "Tottenham Hotspur", Country = country },
                new Team {  Name = "Manchester City", Country = country },
                new Team {  Name = "Liverpool", Country = country }
            };
            var contextMock = MockDbContext(teams.AsQueryable());
            var handler = new GetTeamsByCountryQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(
                new GetTeamsByCountryQuery { Country = "England" }, 
                CancellationToken.None);

            //Assert
            var orderedList = teams
                .OrderBy(t => t.Name)
                .Select(t => new TeamDto { Name = t.Name });
            result.Count().Should().Be(3);
            result.SequenceEqual(orderedList);
        }

        [Fact]
        public async void Given_TeamsExistWithoutCountry_When_GetTeamsByCountry_Then_ReturnOrderedList()
        {
            // Arrange
            var country = new Country { Name = "England" };
            var teams = new List<Team> {
                new Team {  Name = "Tottenham Hotspur", Country = country },
                new Team {  Name = "Manchester City", Country = country },
                new Team {  Name = "Liverpool", Country = country },
                new Team { Name = "England" }
            };
            var contextMock = MockDbContext(teams.AsQueryable());
            var handler = new GetTeamsByCountryQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(
                new GetTeamsByCountryQuery { Country = "England" },
                CancellationToken.None);

            //Assert
            var orderedList = teams
                .OrderBy(t => t.Name)
                .Select(t => new TeamDto { Name = t.Name });
            result.Count().Should().Be(3);
            result.SequenceEqual(orderedList);
        }
    }
}