﻿using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents;
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
    public class GetTeamLeagueMatchEventsQueryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamLeague> leagues)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_GoalsExist_When_GetTeamLeagueMatchEvents_Then_ReturnGoals()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .WithGoals()
                .Build();

            var leagues = Enumerable.Repeat(league, 1);
            var contextMock = MockDbContext(leagues.AsQueryable());
            var loggerMock = new Mock<ILogger<GetTeamLeagueMatchEventsQueryHandler>>();
            var handler = new GetTeamLeagueMatchEventsQueryHandler(
                contextMock.Object, loggerMock.Object
            );

            // Act
            var guid = new Guid("00000000-0000-0000-0000-000000000000");
            var request = new GetTeamLeagueMatchEventsQuery
            {
                LeagueName = "Premier League",
                MatchGuid = guid,
                TeamName = "Tottenham Hotspur"
            };
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Goals.Count().Should().Be(2);
            result.Goals.ToList().ForEach(e =>
            {
                var goal = e.Should().BeAssignableTo<IGoalDto>().Subject;
                int.Parse(goal.Minute).Should().BeInRange(1, 90);
            });
        }

        [Fact]
        public void Given_LeagueDoesNotExist_When_GetTeamLeagueMatchEvents_Then_TeamLeagueNotFoundExceptionIsThrown()
        {
            // Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .WithGoals()
                .Build();

            var leagues = Enumerable.Repeat(league, 1);
            var contextMock = MockDbContext(leagues.AsQueryable());
            var loggerMock = new Mock<ILogger<GetTeamLeagueMatchEventsQueryHandler>>();
            var handler = new GetTeamLeagueMatchEventsQueryHandler(
                contextMock.Object, loggerMock.Object
            );

            // Act
            var guid = new Guid("00000000-0000-0000-0000-000000000000");
            var request = new GetTeamLeagueMatchEventsQuery
            {
                LeagueName = "DoesNotExist",
                MatchGuid = guid,
                TeamName = "Tottenham Hotspur"
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<TeamLeagueNotFoundException>();
        }
    }
}