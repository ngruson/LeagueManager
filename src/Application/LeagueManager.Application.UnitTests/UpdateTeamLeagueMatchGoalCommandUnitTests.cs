using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using LeagueManager.Application.UnitTests.TestData;
using LeagueManager.Domain.Competition;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class UpdateTeamLeagueMatchGoalCommandUnitTests
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
        public async void Given_GoalDoesExist_When_UpdateTeamLeagueGoal_Then_GoalIsUpdated()
        {
            //Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .WithGoals()
                .Build();
            var players = new PlayerBuilder().Build();
            var player = players[0];

            var leagues = Enumerable.Repeat(league, 1);
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var match = league.GetMatch(matchGuid);
            var matchEntry = match.MatchEntries.SingleOrDefault(me => me.Team.Name == "Tottenham Hotspur");
            var goalGuid = matchEntry.Goals.ToList()[0].Guid;
            var contextMock = MockDbContext(
                leagues.AsQueryable(),
                players.AsQueryable()
            );
            var loggerMock = new Mock<ILogger<UpdateTeamLeagueMatchGoalCommandHandler>>();

            var handler = new UpdateTeamLeagueMatchGoalCommandHandler(
                contextMock.Object,
                Mapper.MapperConfig(),
                loggerMock.Object
            );

            //Act
            var command = new UpdateTeamLeagueMatchGoalCommand
            {
                LeagueName = league.Name,
                MatchGuid = matchGuid,
                TeamName = "Tottenham Hotspur",
                GoalGuid = goalGuid,
                Minute = "1",
                PlayerName = player.FullName
            };

            var goal = await handler.Handle(command, CancellationToken.None);

            //Assert
            goal.Minute.Should().Be("1");
            goal.PlayerFullName.Should().Be(player.FullName);
        }

        [Fact]
        public void Given_PlayerDoesNotExist_When_UpdateTeamLeagueGoal_Then_PlayerNotFoundExceptionIsThrown()
        {
            //Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .WithGoals()
                .Build();
            var players = new PlayerBuilder().Build();
            var player = players[0];

            var leagues = Enumerable.Repeat(league, 1);
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var match = league.GetMatch(matchGuid);
            var matchEntry = match.MatchEntries.SingleOrDefault(me => me.Team.Name == "Tottenham Hotspur");
            var goalGuid = matchEntry.Goals.ToList()[0].Guid;
            var contextMock = MockDbContext(
                leagues.AsQueryable(),
                players.AsQueryable()
            );
            var loggerMock = new Mock<ILogger<UpdateTeamLeagueMatchGoalCommandHandler>>();

            var handler = new UpdateTeamLeagueMatchGoalCommandHandler(
                contextMock.Object,
                Mapper.MapperConfig(),
                loggerMock.Object
            );

            //Act
            var command = new UpdateTeamLeagueMatchGoalCommand
            {
                LeagueName = league.Name,
                MatchGuid = matchGuid,
                TeamName = "Tottenham Hotspur",
                GoalGuid = goalGuid,
                Minute = "1",
                PlayerName = "DoesNotExist"
            };

            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert

            func.Should().Throw<PlayerNotFoundException>();
        }

        [Fact]
        public void Given_GoalDoesNotExist_When_UpdateTeamLeagueGoal_Then_GoalNotExistExceptionIsThrown()
        {
            //Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .WithGoals()
                .Build();
            var players = new PlayerBuilder().Build();
            var player = players[0];

            var leagues = Enumerable.Repeat(league, 1);
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var match = league.GetMatch(matchGuid);
            var matchEntry = match.MatchEntries.SingleOrDefault(me => me.Team.Name == "Tottenham Hotspur");
            var goalGuid = matchEntry.Goals.ToList()[0].Guid;
            var contextMock = MockDbContext(
                leagues.AsQueryable(),
                players.AsQueryable()
            );
            var loggerMock = new Mock<ILogger<UpdateTeamLeagueMatchGoalCommandHandler>>();

            var handler = new UpdateTeamLeagueMatchGoalCommandHandler(
                contextMock.Object,
                Mapper.MapperConfig(),
                loggerMock.Object
            );

            //Act
            var command = new UpdateTeamLeagueMatchGoalCommand
            {
                LeagueName = league.Name,
                MatchGuid = matchGuid,
                TeamName = "Tottenham Hotspur",
                GoalGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                Minute = "1",
                PlayerName = player.FullName
            };

            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert

            func.Should().Throw<GoalNotFoundException>();
        }
    }
}