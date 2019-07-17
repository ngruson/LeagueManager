using FluentAssertions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Leagues.Commands;
using LeagueManager.Domain.Entities;
using MediatR;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class CreateTeamLeagueCommandUnitTests
    {
        [Fact]
        public async void Given_TeamLeagueDoesNotExist_When_CreateTeamLeague_Then_TeamLeagueIsReturned()
        {
            // Arrange
            var competitions = new List<Competition>();
            var teams = new List<Team> {
                new Team {  Name = "Team A"},
                new Team { Name = "Team B"}
            };
            var contextMock = MockDbContext(
                competitions.AsQueryable(), 
                teams.AsQueryable());
            var handler = new CreateTeamLeagueCommandHandler(contextMock.Object);

            //Act
            var request = new CreateTeamLeagueCommand {
                Name = "SomeCompetition",
                Teams = new List<string> {  "Team A", "Team B"}
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().Be(Unit.Value);
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        private Mock<ILeagueManagerDbContext> MockDbContext(
            IQueryable<Competition> competitions,
            IQueryable<Team> teams)
        {
            var competitionsDbSet = competitions.BuildMockDbSet();
            var teamsDbSet = teams.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.Competitions).Returns(competitionsDbSet.Object);
            mockContext.Setup(c => c.Teams).Returns(teamsDbSet.Object);
            return mockContext;
        }
    }
}