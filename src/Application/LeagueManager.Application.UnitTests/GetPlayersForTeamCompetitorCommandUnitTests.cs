using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
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
    public class GetPlayersForTeamCompetitorCommandUnitTests
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
        public async void Given_TeamHasPlayers_When_GetPlayersForTeamCompetitor_Then_Success()
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
            var handler = new GetPlayersForTeamCompetitorQueryHandler(
                contextMock.Object,
                Mapper.MapperConfig()
            );

            //Act
            var command = new GetPlayersForTeamCompetitorQuery
            {
                LeagueName = "Premier League",
                TeamName = "Tottenham Hotspur"
            };
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeOfType<List<CompetitorPlayerDto>>();
            result.ToList().Count.Should().Be(2);
        }

        [Fact]
        public void Given_TeamLeagueDoesNotExist_When_GetPlayersForTeamCompetitor_Then_TeamLeagueNotFoundException()
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
            var handler = new GetPlayersForTeamCompetitorQueryHandler(
                contextMock.Object,
                Mapper.MapperConfig()
            );

            //Act
            var command = new GetPlayersForTeamCompetitorQuery
            {
                LeagueName = "DoesNotExist",
                TeamName = "Tottenham Hotspur"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamLeagueNotFoundException>();
        }

        [Fact]
        public void Given_CompetitorDoesNotExist_When_GetPlayersForTeamCompetitor_Then_TeamNotFoundException()
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
            var handler = new GetPlayersForTeamCompetitorQueryHandler(
                contextMock.Object,
                Mapper.MapperConfig()
            );

            //Act
            var command = new GetPlayersForTeamCompetitorQuery
            {
                LeagueName = "Premier League",
                TeamName = "DoesNotExist"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamNotFoundException>();
        }
    }
}