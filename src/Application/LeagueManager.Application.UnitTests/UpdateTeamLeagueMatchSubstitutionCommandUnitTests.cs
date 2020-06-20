using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution;
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
    public class UpdateTeamLeagueMatchSubstitutionCommandUnitTests
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
        public async void Given_SubstitutionDoesExist_When_UpdateTeamLeagueSubstitution_Then_SubstitutionIsUpdated()
        {
            //Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .WithSubstitutions()
                .Build();
            var players = new PlayerBuilder().Build();

            var leagues = Enumerable.Repeat(league, 1);
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var match = league.GetMatch(matchGuid);
            var teamName = "Tottenham Hotspur";
            var matchEntry = match.MatchEntries.SingleOrDefault(me => me.Team.Name == teamName);
            var substitutionGuid = matchEntry.Substitutions.ToList()[0].Guid;
            var contextMock = MockDbContext(
                leagues.AsQueryable(),
                players.AsQueryable()
            );
            var loggerMock = new Mock<ILogger<UpdateTeamLeagueMatchSubstitutionCommandHandler>>();

            var handler = new UpdateTeamLeagueMatchSubstitutionCommandHandler(
                contextMock.Object,
                loggerMock.Object
            );

            //Act
            var command = new UpdateTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = league.Name,
                MatchGuid = matchGuid,
                TeamName = teamName,
                SubstitutionGuid = substitutionGuid,
                Minute = "1",
                PlayerOut = players[0].FullName,
                PlayerIn = players[1].FullName
            };

            var sub = await handler.Handle(command, CancellationToken.None);

            //Assert
            sub.Guid.Should().Be(substitutionGuid);
            sub.TeamMatchEntryTeamName.Should().Be(teamName);
            sub.Minute.Should().Be("1");
            sub.PlayerOut.FullName.Should().Be(players[0].FullName);
            sub.PlayerIn.FullName.Should().Be(players[1].FullName);
        }

        [Fact]
        public void Given_PlayerDoesNotExist_When_UpdateTeamLeagueSubstitution_Then_PlayerNotFoundExceptionIsThrown()
        {
            //Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .WithSubstitutions()
                .Build();
            var players = new PlayerBuilder().Build();
            var player = players[0];

            var leagues = Enumerable.Repeat(league, 1);
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var match = league.GetMatch(matchGuid);
            var matchEntry = match.MatchEntries.SingleOrDefault(me => me.Team.Name == "Tottenham Hotspur");
            var substitutionGuid = matchEntry.Substitutions.ToList()[0].Guid;
            var contextMock = MockDbContext(
                leagues.AsQueryable(),
                players.AsQueryable()
            );
            var loggerMock = new Mock<ILogger<UpdateTeamLeagueMatchSubstitutionCommandHandler>>();

            var handler = new UpdateTeamLeagueMatchSubstitutionCommandHandler(
                contextMock.Object,
                loggerMock.Object
            );

            //Act
            var command = new UpdateTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = league.Name,
                MatchGuid = matchGuid,
                TeamName = "Tottenham Hotspur",
                SubstitutionGuid = substitutionGuid,
                Minute = "1",
                PlayerOut = "DoesNotExist"
            };

            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert

            func.Should().Throw<PlayerNotFoundException>();
        }

        [Fact]
        public void Given_SubstitutionDoesNotExist_When_UpdateTeamLeagueSubstitution_Then_SubstitutionNotExistExceptionIsThrown()
        {
            //Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .WithGoals()
                .Build();
            var players = new PlayerBuilder().Build();

            var leagues = Enumerable.Repeat(league, 1);
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var match = league.GetMatch(matchGuid);
            var matchEntry = match.MatchEntries.SingleOrDefault(me => me.Team.Name == "Tottenham Hotspur");
            var substitutionGuid = matchEntry.Goals.ToList()[0].Guid;
            var contextMock = MockDbContext(
                leagues.AsQueryable(),
                players.AsQueryable()
            );
            var loggerMock = new Mock<ILogger<UpdateTeamLeagueMatchSubstitutionCommandHandler>>();

            var handler = new UpdateTeamLeagueMatchSubstitutionCommandHandler(
                contextMock.Object,
                loggerMock.Object
            );

            //Act
            var command = new UpdateTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = league.Name,
                MatchGuid = matchGuid,
                TeamName = "Tottenham Hotspur",
                SubstitutionGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                Minute = "1",
                PlayerOut = players[0].FullName,
                PlayerIn = players[1].FullName
            };

            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert

            func.Should().Throw<SubstitutionNotFoundException>();
        }
    }
}