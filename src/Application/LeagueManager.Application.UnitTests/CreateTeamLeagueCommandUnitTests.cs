using FluentAssertions;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
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
        private List<Team> CreateTeams()
        {
            return new List<Team> {
                new Team {  Name = "Team A"},
                new Team { Name = "Team B"},
                new Team { Name = "Team C"},
                new Team { Name = "Team D"}
            };
        }

        [Fact]
        public async void Given_TeamLeagueDoesNotExist_When_CreateTeamLeague_Then_TeamLeagueIsReturned()
        {
            // Arrange
            var leagues = new List<TeamLeague>();
            var teams = CreateTeams();

            var contextMock = MockDbContext(
                leagues.AsQueryable(), 
                teams.AsQueryable());
            var handler = new CreateTeamLeagueCommandHandler(contextMock.Object);

            //Act
            var request = new CreateTeamLeagueCommand {
                Name = "SomeCompetition",
                Teams = teams.Select(t => t.Name).ToList()
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().Be(Unit.Value);
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        private Mock<ILeagueManagerDbContext> MockDbContext(
            IQueryable<TeamLeague> leagues,
            IQueryable<Team> teams)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();
            var teamsDbSet = teams.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            mockContext.Setup(c => c.Teams).Returns(teamsDbSet.Object);
            return mockContext;
        }
    }
}