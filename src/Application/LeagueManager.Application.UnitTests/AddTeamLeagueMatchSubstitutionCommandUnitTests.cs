using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchSubstitution;
using LeagueManager.Application.UnitTests.TestData;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using Microsoft.Extensions.Logging;
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
    public class AddTeamLeagueMatchSubstitutionCommandUnitTests
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
        public async void Given_AllConditionsPass_When_AddTeamLeagueMatchSubstitution_Then_Success()
        {
            //Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] {
                new Domain.Player.Player { FirstName = "John", LastName = "Doe" },
                new Domain.Player.Player { FirstName = "Jane", LastName = "Doe" }
            };

            var mockContext = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable(),
                players.AsQueryable());
            var mockLogger = new Mock<ILogger<AddTeamLeagueMatchSubstitutionCommandHandler>>();

            var handler = new AddTeamLeagueMatchSubstitutionCommandHandler(
                mockContext.Object,
                Mapper.MapperConfig(),
                mockLogger.Object
            );

            //Act
            var command = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                Minute = "1",
                PlayerOut = "John Doe",
                PlayerIn = "Jane Doe"
            };
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeAssignableTo<SubstitutionDto>();
            result.PlayerOut.Should().Be("John Doe");
            result.PlayerIn.Should().Be("Jane Doe");
            mockContext.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public void Given_LeagueDoesNotExist_When_AddTeamLeagueMatchSubstitution_Then_TeamLeagueNotFoundExceptionIsThrown()
        {
            //Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] {
                new Domain.Player.Player { FirstName = "John", LastName = "Doe" },
                new Domain.Player.Player { FirstName = "Jane", LastName = "Doe" }
            };

            var mockContext = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable(),
                players.AsQueryable());
            var mockLogger = new Mock<ILogger<AddTeamLeagueMatchSubstitutionCommandHandler>>();

            var handler = new AddTeamLeagueMatchSubstitutionCommandHandler(
                mockContext.Object,
                Mapper.MapperConfig(),
                mockLogger.Object
            );

            //Act
            var command = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "DoesNotExist",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                Minute = "1",
                PlayerOut = "John Doe",
                PlayerIn = "Jane Doe"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamLeagueNotFoundException>();
        }

        [Fact]
        public void Given_MatchDoesNotExist_When_AddTeamLeagueMatchSubstitution_Then_MatchNotFoundExceptionIsThrown()
        {
            //Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] {
                new Domain.Player.Player { FirstName = "John", LastName = "Doe" },
                new Domain.Player.Player { FirstName = "Jane", LastName = "Doe" }
            };

            var mockContext = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable(),
                players.AsQueryable());
            var mockLogger = new Mock<ILogger<AddTeamLeagueMatchSubstitutionCommandHandler>>();

            var handler = new AddTeamLeagueMatchSubstitutionCommandHandler(
                mockContext.Object,
                Mapper.MapperConfig(),
                mockLogger.Object
            );

            //Act
            var command = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("10000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                Minute = "1",
                PlayerOut = "John Doe",
                PlayerIn = "Jane Doe"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<MatchNotFoundException>();
        }

        [Fact]
        public void Given_MatchEntryDoesNotExist_When_AddTeamLeagueMatchSubstitution_Then_MatchEntryNotFoundExceptionIsThrown()
        {
            //Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] {
                new Domain.Player.Player { FirstName = "John", LastName = "Doe" },
                new Domain.Player.Player { FirstName = "Jane", LastName = "Doe" }
            };

            var mockContext = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable(),
                players.AsQueryable());
            var mockLogger = new Mock<ILogger<AddTeamLeagueMatchSubstitutionCommandHandler>>();

            var handler = new AddTeamLeagueMatchSubstitutionCommandHandler(
                mockContext.Object,
                Mapper.MapperConfig(),
                mockLogger.Object
            );

            //Act
            var command = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "DoesNotExist",
                Minute = "1",
                PlayerOut = "John Doe",
                PlayerIn = "Jane Doe"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<MatchEntryNotFoundException>();
        }

        [Fact]
        public void Given_PlayerOutDoesNotExist_When_AddTeamLeagueMatchSubstitution_Then_PlayerNotFoundExceptionIsThrown()
        {
            //Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] {
                new Domain.Player.Player { FirstName = "John", LastName = "Doe" },
                new Domain.Player.Player { FirstName = "Jane", LastName = "Doe" }
            };

            var mockContext = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable(),
                players.AsQueryable());
            var mockLogger = new Mock<ILogger<AddTeamLeagueMatchSubstitutionCommandHandler>>();

            var handler = new AddTeamLeagueMatchSubstitutionCommandHandler(
                mockContext.Object,
                Mapper.MapperConfig(),
                mockLogger.Object
            );

            //Act
            var command = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                Minute = "1",
                PlayerOut = "DoesNotExist",
                PlayerIn = "Jane Doe"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<PlayerNotFoundException>();
        }

        [Fact]
        public void Given_PlayerInDoesNotExist_When_AddTeamLeagueMatchSubstitution_Then_PlayerNotFoundExceptionIsThrown()
        {
            //Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new Domain.Player.Player[] {
                new Domain.Player.Player { FirstName = "John", LastName = "Doe" },
                new Domain.Player.Player { FirstName = "Jane", LastName = "Doe" }
            };

            var mockContext = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable(),
                players.AsQueryable());
            var mockLogger = new Mock<ILogger<AddTeamLeagueMatchSubstitutionCommandHandler>>();

            var handler = new AddTeamLeagueMatchSubstitutionCommandHandler(
                mockContext.Object,
                Mapper.MapperConfig(),
                mockLogger.Object
            );

            //Act
            var command = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                Minute = "1",
                PlayerOut = "John Doe",
                PlayerIn = "DoesNotExist"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<PlayerNotFoundException>();
        }
    }
}