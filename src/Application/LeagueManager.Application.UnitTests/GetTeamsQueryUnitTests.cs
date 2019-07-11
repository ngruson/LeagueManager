using FluentAssertions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Teams.Queries.GetTeams;
using LeagueManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class GetTeamsQueryUnitTests
    {
        [Fact]
        public async void ReturnEmptyListWhenNoTeamsExist()
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
        public async void ReturnOrderedListWhenTeamsExist()
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
            var mockSet = new Mock<DbSet<Team>>();
            mockSet.As<IQueryable<Team>>().Setup(m => m.Provider).Returns(teams.Provider);
            mockSet.As<IQueryable<Team>>().Setup(m => m.Expression).Returns(teams.Expression);
            mockSet.As<IQueryable<Team>>().Setup(m => m.ElementType).Returns(teams.ElementType);
            mockSet.As<IQueryable<Team>>().Setup(m => m.GetEnumerator()).Returns(teams.GetEnumerator());

            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.Teams).Returns(mockSet.Object);

            return mockContext;
        }
    }
}
