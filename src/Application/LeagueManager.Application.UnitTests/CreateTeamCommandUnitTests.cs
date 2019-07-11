using FluentAssertions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Teams.Commands.CreateTeam;
using LeagueManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class CreateTeamCommandUnitTests
    {
        [Fact]
        public async void ReturnSuccessWhenTeamDoesNotYetExist()
        {
            // Arrange
            var teams = new List<Team>();
            var contextMock = MockDbContext(teams.AsQueryable());
            var handler = new CreateTeamCommandHandler(contextMock.Object);

            //Act
            var result =  await handler.Handle(new CreateTeamCommand { Name = "Liverpool" }, CancellationToken.None);

            //Assert
            result.Should().Be(Unit.Value);
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public void ThrowExceptionWhenTeamAlreadyExists()
        {
            // Arrange
            var teams = new List<Team> { new Team { Name = "Liverpool" } };
            var contextMock = MockDbContext(teams.AsQueryable());
            var handler = new CreateTeamCommandHandler(contextMock.Object);

            //Act
            Func<Task> func = async () => await handler.Handle(new CreateTeamCommand { Name = "Liverpool" }, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamAlreadyExistsException>();
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