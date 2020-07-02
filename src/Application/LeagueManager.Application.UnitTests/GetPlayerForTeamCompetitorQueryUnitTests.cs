using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor;
using LeagueManager.Application.UnitTests.TestData;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
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
    public class GetPlayerForTeamCompetitorQueryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(
            IQueryable<TeamLeague> teamLeagues,
            IQueryable<Team> teams)
        {
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(teamLeagues.BuildMockDbSet().Object);
            mockContext.Setup(c => c.Teams).Returns(teams.BuildMockDbSet().Object);

            return mockContext;
        }

        [Fact]
        public async void Given_TeamHasPlayer_When_GetPlayerForTeamCompetitor_Then_ReturnPlayer()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var players = new PlayerBuilder().Build();
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithPlayers(teams[0], players);

            var teamLeague = builder.Build();

            var contextMock = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable()
            );
            var handler = new GetPlayerForTeamCompetitorQueryHandler(
                contextMock.Object,
                Mapper.MapperConfig());

            //Act
            var command = new GetPlayerForTeamCompetitorQuery
            {
                LeagueName = "Premier League",
                TeamName = "Tottenham Hotspur",
                PlayerName = "John Doe"
            };
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeOfType<CompetitorPlayerDto>();
            result.Player.Should().NotBeNull();
            result.Player.FirstName.Should().Be("John");
            result.Player.LastName.Should().Be("Doe");
            result.Number.Should().Be("1");
        }

        [Fact]
        public void Given_LeagueDoesNotExist_When_GetPlayerForTeamCompetitor_Then_ExceptionIsThrown()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var players = new PlayerBuilder().Build();
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithPlayers(teams[0], players);

            var teamLeague = builder.Build();

            var contextMock = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable()
            );
            var handler = new GetPlayerForTeamCompetitorQueryHandler(
                contextMock.Object,
                Mapper.MapperConfig());

            //Act
            var command = new GetPlayerForTeamCompetitorQuery
            {
                LeagueName = "DoesNotExist",
                TeamName = "Tottenham Hotspur",
                PlayerName = "John Doe"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamLeagueNotFoundException>();
        }

        [Fact]
        public void Given_TeamDoesNotExist_When_GetPlayerForTeamCompetitor_Then_ExceptionIsThrown()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var players = new PlayerBuilder().Build();
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithPlayers(teams[0], players);

            var teamLeague = builder.Build();

            var contextMock = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable()
            );
            var handler = new GetPlayerForTeamCompetitorQueryHandler(
                contextMock.Object,
                Mapper.MapperConfig());

            //Act
            var command = new GetPlayerForTeamCompetitorQuery
            {
                LeagueName = "Premier League",
                TeamName = "DoesNotExist",
                PlayerName = "John Doe"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamNotFoundException>();
        }

        [Fact]
        public void Given_PlayerDoesNotExist_When_GetPlayerForTeamCompetitor_Then_ExceptionIsThrown()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var players = new PlayerBuilder().Build();
            var builder = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithPlayers(teams[0], players);

            var teamLeague = builder.Build();

            var contextMock = MockDbContext(
                new List<TeamLeague> { teamLeague }.AsQueryable(),
                teamLeague.Competitors.Select(c => c.Team).AsQueryable()
            );
            var handler = new GetPlayerForTeamCompetitorQueryHandler(
                contextMock.Object,
                Mapper.MapperConfig());

            //Act
            var command = new GetPlayerForTeamCompetitorQuery
            {
                LeagueName = "Premier League",
                TeamName = "Tottenham Hotspur",
                PlayerName = "DoesNotExist"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<PlayerNotFoundException>();
        }
    }
}