using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.UnitTests.TestData;
using LeagueManager.Domain.Competition;
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
    public class UpdateTeamLeagueMatchScoreCommandUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(
            IQueryable<TeamLeague> leagues)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();

            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_MatchDoesExist_When_UpdateTeamLeagueMatchScore_Then_ScoreIsUpdated()
        {
            //Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .Build();

            var contextMock = MockDbContext(Enumerable.Repeat(league, 1).AsQueryable());
            var handler = new UpdateTeamLeagueMatchScoreCommandHandler(
                contextMock.Object, Mapper.CreateMapper()
            );

            //Act
            var request = new UpdateTeamLeagueMatchScoreCommand
            {
                LeagueName = "Premier League",
                Guid = new Guid("00000000-0000-0000-0000-000000000000"),
                MatchEntries = new List<TeamMatchEntryRequestDto>
                {
                    new TeamMatchEntryRequestDto
                    {
                        Team = "Tottenham Hotspur",
                        Score = 1,
                    }
                    ,
                    new TeamMatchEntryRequestDto
                    {
                        Team = "Manchester City",
                        Score = 1
                    }
                }

            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public void Given_LeagueDoesNotExist_When_UpdateTeamLeagueMatchScore_Then_ThrowTeamLeagueNotFoundException()
        {
            //Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .Build();

            var contextMock = MockDbContext(Enumerable.Repeat(league, 1).AsQueryable());
            var handler = new UpdateTeamLeagueMatchScoreCommandHandler(
                contextMock.Object, Mapper.CreateMapper()
            );

            //Act
            var request = new UpdateTeamLeagueMatchScoreCommand
            {
                LeagueName = "DoesNotExist",
                Guid = new Guid("00000000-0000-0000-0000-000000000000"),
                MatchEntries = new List<TeamMatchEntryRequestDto>
                {
                    new TeamMatchEntryRequestDto
                    {
                        Team = "Tottenham Hotspur",
                        Score = 1,
                    }
                    ,
                    new TeamMatchEntryRequestDto
                    {
                        Team = "Manchester City",
                        Score = 1
                    }
                }

            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamLeagueNotFoundException>();
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public void Given_MatchDoesNotExist_When_UpdateTeamLeagueMatchScore_Then_ThrowMatchNotFoundException()
        {
            //Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .Build();

            var contextMock = MockDbContext(Enumerable.Repeat(league, 1).AsQueryable());
            var handler = new UpdateTeamLeagueMatchScoreCommandHandler(
                contextMock.Object, Mapper.CreateMapper()
            );

            //Act
            var request = new UpdateTeamLeagueMatchScoreCommand
            {
                LeagueName = "Premier League",
                Guid = new Guid("10000000-0000-0000-0000-000000000000"),
                MatchEntries = new List<TeamMatchEntryRequestDto>
                {
                    new TeamMatchEntryRequestDto
                    {
                        Team = "Tottenham Hotspur",
                        Score = 1,
                    }
                    ,
                    new TeamMatchEntryRequestDto
                    {
                        Team = "Manchester City",
                        Score = 1
                    }
                }

            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<MatchNotFoundException>();
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public void Given_MatchEntryDoesNotExist_When_UpdateTeamLeagueMatchScore_Then_ThrowMatchEntryNotFoundException()
        {
            //Arrange
            var teams = new TeamsBuilder().Build();
            var league = new TeamLeagueBuilder()
                .WithCompetitors(teams)
                .WithRounds()
                .Build();

            var contextMock = MockDbContext(Enumerable.Repeat(league, 1).AsQueryable());
            var handler = new UpdateTeamLeagueMatchScoreCommandHandler(
                contextMock.Object, Mapper.CreateMapper()
            );

            //Act
            var request = new UpdateTeamLeagueMatchScoreCommand
            {
                LeagueName = "Premier League",
                Guid = new Guid("00000000-0000-0000-0000-000000000000"),
                MatchEntries = new List<TeamMatchEntryRequestDto>
                {
                    new TeamMatchEntryRequestDto
                    {
                        Team = "DoesNotExist",
                        Score = 1,
                    }
                    ,
                    new TeamMatchEntryRequestDto
                    {
                        Team = "Manchester City",
                        Score = 1
                    }
                }

            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<MatchEntryNotFoundException>();
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}