using FluentAssertions;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Application.UnitTests.TestData;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Competition;
using MockQueryable.Moq;
using LeagueManager.Application.Exceptions;

namespace LeagueManager.Application.UnitTests
{
    public class UpdateTeamLeagueMatchLineupEntryCommandUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(
            IQueryable<TeamLeague> leagues,
            IQueryable<Domain.Player.Player> players)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();
            var playersDbSet = players.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            mockContext.Setup(c => c.Players).Returns(playersDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_EntryDoesExist_When_UpdateTeamLeagueMatchLineupEntry_Then_EntryIsUpdated()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new PlayerBuilder().Build();

            var contextMock = MockDbContext(
                Enumerable.Repeat(teamLeague, 1).AsQueryable(),
                players.AsQueryable()
            );

            var handler = new UpdateTeamLeagueMatchLineupEntryCommandHandler(
                contextMock.Object,
                Mapper.MapperConfig()
            );

            //Act
            var request = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                LineupEntryGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                PlayerName = "John Doe",
                PlayerNumber = "1"
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().BeOfType<LineupEntryDto>();
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
            result.Player.FirstName.Should().Be("John");
            result.Player.LastName.Should().Be("Doe");
            result.PlayerNumber.Should().Be("1");
        }

        [Fact]
        public void Given_LeagueDoesNotExist_When_UpdateTeamLeagueMatchLineupEntry_Then_LineupEntryNotFoundExceptionIsThrown()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new PlayerBuilder().Build();

            var contextMock = MockDbContext(
                Enumerable.Repeat(teamLeague, 1).AsQueryable(),
                players.AsQueryable()
            );

            var handler = new UpdateTeamLeagueMatchLineupEntryCommandHandler(
                contextMock.Object,
                Mapper.MapperConfig()
            );

            //Act
            var request = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "DoesNotExist",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                LineupEntryGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                PlayerName = "John Doe",
                PlayerNumber = "1"
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<LineupEntryNotFoundException>();
        }

        [Fact]
        public void Given_MatchDoesNotExist_When_UpdateTeamLeagueMatchLineupEntry_Then_LineupEntryNotFoundExceptionIsThrown()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new PlayerBuilder().Build();

            var contextMock = MockDbContext(
                Enumerable.Repeat(teamLeague, 1).AsQueryable(),
                players.AsQueryable()
            );

            var handler = new UpdateTeamLeagueMatchLineupEntryCommandHandler(
                contextMock.Object,
                Mapper.MapperConfig()
            );

            //Act
            var request = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("10000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                LineupEntryGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                PlayerName = "John Doe",
                PlayerNumber = "1"
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<LineupEntryNotFoundException>();
        }

        [Fact]
        public void Given_TeamDoesNotExist_When_UpdateTeamLeagueMatchLineupEntry_Then_LineupEntryNotFoundExceptionIsThrown()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new PlayerBuilder().Build();

            var contextMock = MockDbContext(
                Enumerable.Repeat(teamLeague, 1).AsQueryable(),
                players.AsQueryable()
            );

            var handler = new UpdateTeamLeagueMatchLineupEntryCommandHandler(
                contextMock.Object,
                Mapper.MapperConfig()
            );

            //Act
            var request = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "DoesNotExist",
                LineupEntryGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                PlayerName = "John Doe",
                PlayerNumber = "1"
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<LineupEntryNotFoundException>();
        }

        [Fact]
        public void Given_EntryDoesNotExist_When_UpdateTeamLeagueMatchLineupEntry_Then_LineupEntryNotFoundExceptionIsThrown()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new PlayerBuilder().Build();

            var contextMock = MockDbContext(
                Enumerable.Repeat(teamLeague, 1).AsQueryable(),
                players.AsQueryable()
            );

            var handler = new UpdateTeamLeagueMatchLineupEntryCommandHandler(
                contextMock.Object,
                Mapper.MapperConfig()
            );

            //Act
            var request = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                LineupEntryGuid = new Guid("10000000-0000-0000-0000-000000000000"),
                PlayerName = "John Doe",
                PlayerNumber = "1"
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<LineupEntryNotFoundException>();
        }

        [Fact]
        public void Given_PlayerDoesNotExist_When_UpdateTeamLeagueMatchLineupEntry_Then_PlayerNotFoundExceptionIsThrown()
        {
            // Arrange
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds();

            var teamLeague = builder.Build();
            var players = new PlayerBuilder().Build();

            var contextMock = MockDbContext(
                Enumerable.Repeat(teamLeague, 1).AsQueryable(),
                players.AsQueryable()
            );

            var handler = new UpdateTeamLeagueMatchLineupEntryCommandHandler(
                contextMock.Object,
                Mapper.MapperConfig()
            );

            //Act
            var request = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "Premier League",
                MatchGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                TeamName = "Tottenham Hotspur",
                LineupEntryGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                PlayerName = "DoesNotExist",
                PlayerNumber = "1"
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<PlayerNotFoundException>();
        }
    }
}