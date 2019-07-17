using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Teams.Commands.CreateTeam;
using LeagueManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
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
        public async void Given_TeamDoesNotExist_When_CreateTeam_Then_TeamIsReturned()
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
        public void Given_TeamDoesExists_When_CreateExistingTeam_Then_ThrowException()
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
            var mockSet = teams.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.Teams).Returns(mockSet.Object);

            return mockContext;
        }
    }
}