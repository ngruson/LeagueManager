using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Match.Commands.AddPlayerToLineup;
using LeagueManager.Domain.Competition;
using Moq;
using MockQueryable.Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using LeagueManager.Domain.Competitor;
using System;
using LeagueManager.Application.UnitTests.TestData;
using System.Threading;
using MediatR;
using FluentAssertions;
using System.Threading.Tasks;
using LeagueManager.Application.Exceptions;

namespace LeagueManager.Application.UnitTests
{
    public class AddPlayerToLineupUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamLeague> teamLeagues,
            IQueryable<Team> teams,
            IQueryable<Domain.Player.Player> players)
        {          
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.Players).Returns(players.BuildMockDbSet().Object);
            mockContext.Setup(c => c.TeamLeagues).Returns(teamLeagues.BuildMockDbSet().Object);
            mockContext.Setup(c => c.Teams).Returns(teams.BuildMockDbSet().Object);

            return mockContext;
        }

        [Fact]
        public async void Given_AllConditionsPass_When_AddPlayerToLineup_Then_Success()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] { new Domain.Player.Player { FirstName = "John", LastName = "Doe" } };
            
            var contextMock = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(), 
                teamLeague.Competitors.Select(c => c.Team).AsQueryable(),
                players.AsQueryable());
            var handler = new AddPlayerToLineupCommandHandler(contextMock.Object);

            //Act
            var command = new AddPlayerToLineupCommand {
                LeagueName = "Premier League",
                Guid = new Guid("00000000-0000-0000-0000-000000000000"),
                Team = "Tottenham Hotspur",
                Number = "1",
                Player = "John Doe" 
            };
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().Be(Unit.Value);
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }       

        [Fact]
        public void Given_MatchEntryDoesNotExist_When_AddPlayerToLineup_Then_Exception()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] { new Domain.Player.Player { FirstName = "John", LastName = "Doe" } };

            var contextMock = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable(),
                players.AsQueryable());
            var handler = new AddPlayerToLineupCommandHandler(contextMock.Object);

            //Act
            var command = new AddPlayerToLineupCommand
            {
                LeagueName = "Premier League",
                Guid = new Guid("00000000-0000-0000-0000-000000000000"),
                Team = "DoesNotExist",
                Number = "1",
                Player = "John Doe"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<MatchEntryNotFoundException>();
        }

        [Fact]
        public void Given_PlayerDoesNotExist_When_AddPlayerToLineup_Then_Exception()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] { new Domain.Player.Player { FirstName = "John", LastName = "Doe" } };

            var contextMock = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable(),
                players.AsQueryable());
            var handler = new AddPlayerToLineupCommandHandler(contextMock.Object);

            //Act
            var command = new AddPlayerToLineupCommand
            {
                LeagueName = "Premier League",
                Guid = new Guid("00000000-0000-0000-0000-000000000000"),
                Team = "Tottenham Hotspur",
                Number = "1",
                Player = "Jane Doe"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<PlayerNotFoundException>();
        }
    }
}