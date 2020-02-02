using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor;
using LeagueManager.Application.UnitTests.TestData;
using LeagueManager.Domain.Competition;
using MediatR;
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
    public class AddPlayerToTeamCompetitorCommandUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamLeague> teamLeagues,
            IQueryable<Domain.Player.Player> players)
        {
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.Players).Returns(players.BuildMockDbSet().Object);
            mockContext.Setup(c => c.TeamLeagues).Returns(teamLeagues.BuildMockDbSet().Object);

            return mockContext;
        }

        [Fact]
        public async void Given_AllConditionsPass_When_AddPlayerToTeamCompetitor_Then_Success()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build());

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] { new Domain.Player.Player { FirstName = "John", LastName = "Doe" } };

            var contextMock = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                players.AsQueryable());
            var handler = new AddPlayerToTeamCompetitorCommandHandler(contextMock.Object);

            //Act
            var command = new AddPlayerToTeamCompetitorCommand
            {
                LeagueName = "Premier League",
                TeamName = "Tottenham Hotspur",
                PlayerNumber = "1",
                PlayerName = "John Doe"
            };
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().Be(Unit.Value);
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public void Given_TeamLeagueDoesNotExist_When_AddPlayerToTeamCompetitor_Then_Exception()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build());

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] { new Domain.Player.Player { FirstName = "John", LastName = "Doe" } };

            var contextMock = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                players.AsQueryable());
            var handler = new AddPlayerToTeamCompetitorCommandHandler(contextMock.Object);

            //Act
            var command = new AddPlayerToTeamCompetitorCommand
            {
                LeagueName = "DoesNotExist",
                TeamName = "Tottenham Hotspur",
                PlayerNumber = "1",
                PlayerName = "John Doe"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamLeagueNotFoundException>();
        }

        [Fact]
        public void Given_TeamDoesNotExist_When_AddPlayerToTeamCompetitor_Then_Exception()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build());

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] { new Domain.Player.Player { FirstName = "John", LastName = "Doe" } };

            var contextMock = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                players.AsQueryable());
            var handler = new AddPlayerToTeamCompetitorCommandHandler(contextMock.Object);

            //Act
            var command = new AddPlayerToTeamCompetitorCommand
            {
                LeagueName = "Premier League",
                TeamName = "DoesNotExist",
                PlayerNumber = "1",
                PlayerName = "John Doe"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamNotFoundException>();
        }

        [Fact]
        public void Given_PlayerDoesNotExist_When_AddPlayerToTeamCompetitor_Then_Exception()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build());

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] { new Domain.Player.Player { FirstName = "John", LastName = "Doe" } };

            var contextMock = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                players.AsQueryable());
            var handler = new AddPlayerToTeamCompetitorCommandHandler(contextMock.Object);

            //Act
            var command = new AddPlayerToTeamCompetitorCommand
            {
                LeagueName = "Premier League",
                TeamName = "Tottenham Hotspur",
                PlayerNumber = "1",
                PlayerName = "DoesNotExist"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<PlayerNotFoundException>();
        }
    }
}