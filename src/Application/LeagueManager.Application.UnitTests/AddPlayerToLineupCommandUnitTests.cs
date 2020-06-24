using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddPlayerToLineup;
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
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using LeagueManager.Application.Player.Commands.CreatePlayer;
using LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;

namespace LeagueManager.Application.UnitTests
{
    public class AddPlayerToLineupCommandUnitTests
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
            var contextMediator = new Mock<IMediator>();
            var contextLogger = new Mock<ILogger<AddPlayerToLineupCommandHandler>>();

            var handler = new AddPlayerToLineupCommandHandler(
                contextMock.Object,
                contextMediator.Object,
                Mapper.MapperConfig(),
                contextLogger.Object
            );

            //Act
            var command = new AddPlayerToLineupCommand {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                Number = "1",
                Player = new TeamLeagueMatches.Commands.AddPlayerToLineup.PlayerDto
                {
                    FirstName = "John",
                    LastName = "Doe"
                }
            };
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().Be(Unit.Value);
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }       

        [Fact]
        public void Given_MatchEntryDoesNotExist_When_AddPlayerToLineup_Then_MatchEntryNotFoundException()
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
            var contextMediator = new Mock<IMediator>();
            var contextLogger = new Mock<ILogger<AddPlayerToLineupCommandHandler>>();

            var handler = new AddPlayerToLineupCommandHandler(
                contextMock.Object,
                contextMediator.Object,
                Mapper.MapperConfig(),
                contextLogger.Object
            );

            //Act
            var command = new AddPlayerToLineupCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "DoesNotExist",
                Number = "1",
                Player = new TeamLeagueMatches.Commands.AddPlayerToLineup.PlayerDto
                {
                    FirstName = "John",
                    LastName = "Doe"
                }
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<MatchEntryNotFoundException>();
        }

        [Fact]
        public async void Given_PlayerDoesNotExist_When_AddPlayerToLineup_Then_CreatePlayer()
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
            var contextMediator = new Mock<IMediator>();
            contextMediator.Setup(x => x.Send(
                It.IsAny<GetPlayersForTeamCompetitorQuery>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<CompetitorPlayerDto>());

            var contextLogger = new Mock<ILogger<AddPlayerToLineupCommandHandler>>();

            var handler = new AddPlayerToLineupCommandHandler(
                contextMock.Object,
                contextMediator.Object,
                Mapper.MapperConfig(),
                contextLogger.Object
            );

            //Act
            var command = new AddPlayerToLineupCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                Number = "1",
                Player = new TeamLeagueMatches.Commands.AddPlayerToLineup.PlayerDto
                {
                    FirstName = "Jane",
                    LastName = "Doe"
                }
            };
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            contextMediator.Verify(x => x.Send(
                It.IsAny<CreatePlayerCommand>(),
                It.IsAny<CancellationToken>() 
            ));
            contextMediator.Verify(x => x.Send(
                It.IsAny<AddPlayerToTeamCompetitorCommand>(),
                It.IsAny<CancellationToken>()
            ));

            contextMock.Verify(mock => mock.SaveChangesAsync(
                It.IsAny<CancellationToken>()
            ));

            result.Should().Be(Unit.Value);
        }
    }
}