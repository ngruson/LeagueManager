using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal;
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
    public class GetTeamLeagueMatchGoalQueryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamLeague> leagues)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_GoalExist_When_GetTeamLeagueMatchGoal_Then_ReturnGoal()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .WithGoals()
                .Build();

            var leagues = Enumerable.Repeat(league, 1);
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var match = league.GetMatch(matchGuid);
            var matchEntry = match.MatchEntries.SingleOrDefault(me => me.Team.Name == "Tottenham Hotspur");
            var goalGuid = matchEntry.Goals.ToList()[0].Guid;

            var contextMock = MockDbContext(leagues.AsQueryable());
            var loggerMock = new Mock<ILogger<GetTeamLeagueMatchGoalQueryHandler>>();
            var handler = new GetTeamLeagueMatchGoalQueryHandler(
                contextMock.Object, 
                Mapper.MapperConfig(), 
                loggerMock.Object
            );

            // Act
            var request = new GetTeamLeagueMatchGoalQuery
            {
                LeagueName = "Premier League",
                MatchGuid = matchGuid,
                GoalGuid = goalGuid
            };
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Given_GoalDoesNotExist_When_GetTeamLeagueMatchGoal_Then_GoalNotFoundExceptionIsThrown()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .WithGoals()
                .Build();

            var leagues = Enumerable.Repeat(league, 1);
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var match = league.GetMatch(matchGuid);
            var matchEntry = match.MatchEntries.SingleOrDefault(me => me.Team.Name == "Tottenham Hotspur");
            var goalGuid = matchEntry.Goals.ToList()[0].Guid;

            var contextMock = MockDbContext(leagues.AsQueryable());
            var loggerMock = new Mock<ILogger<GetTeamLeagueMatchGoalQueryHandler>>();
            var handler = new GetTeamLeagueMatchGoalQueryHandler(
                contextMock.Object,
                Mapper.MapperConfig(),
                loggerMock.Object
            );

            // Act
            var request = new GetTeamLeagueMatchGoalQuery
            {
                LeagueName = "Premier League",
                MatchGuid = matchGuid,
                GoalGuid = new Guid("00000000-0000-0000-0000-000000000000")
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<GoalNotFoundException>();
        }
    }
}