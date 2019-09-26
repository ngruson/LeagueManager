using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Commands;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Round;
using MediatR;
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
    public class UpdateTeamLeagueMatchCommandUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(
            IQueryable<TeamLeague> teamLeagues,
            IQueryable<Team> teams)
        {
            var teamLeaguesDbSet = teamLeagues.BuildMockDbSet();
            var teamsDbSet = teams.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(teamLeaguesDbSet.Object);
            mockContext.Setup(c => c.Teams).Returns(teamsDbSet.Object);
            return mockContext;
        }

        private List<Team> CreateTeams()
        {
            return new List<Team> {
                new Team {  Name = "Team A"},
                new Team { Name = "Team B"},
                new Team { Name = "Team C"},
                new Team { Name = "Team D"}
            };
        }

        private List<TeamLeague> CreateTeamLeagues(Guid guid, bool addMatch)
        {
            return new List<TeamLeague>()
            {
                new TeamLeague
                {
                    Name = "TeamLeague",
                    Rounds = addMatch ? new List<TeamLeagueRound>
                    {
                        new TeamLeagueRound
                        {
                            Matches = new List<TeamMatch>
                            {
                                new TeamMatch
                                {
                                    Guid = guid,
                                    MatchEntries = new List<TeamMatchEntry>
                                    {
                                        new TeamMatchEntry
                                        {
                                            HomeAway = HomeAway.Home
                                        },
                                        new TeamMatchEntry
                                        {
                                            HomeAway = HomeAway.Away
                                        }
                                    }
                                }
                            }
                        }
                    } : null
                }
            };
        }

        [Fact]
        public async void Given_MatchDoesExist_When_UpdateTeamLeagueMatch_Then_MatchIsUpdated()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var teamLeagues = CreateTeamLeagues(guid, true);
            var teams = CreateTeams();

            var contextMock = MockDbContext(
                teamLeagues.AsQueryable(),
                teams.AsQueryable());
            var handler = new UpdateTeamLeagueMatchCommandHandler(contextMock.Object);

            //Act
            var request = new UpdateTeamLeagueMatchCommand
            {
                LeagueName = "TeamLeague",
                Guid = guid,
                HomeTeam = "Team A",
                AwayTeam = "Team B"
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().Be(Unit.Value);
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public void Given_MatchDoesNotExist_When_UpdateTeamLeagueMatch_Then_ThrowMatchNotFoundException()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var teamLeagues = CreateTeamLeagues(guid, false);
            var teams = CreateTeams();

            var contextMock = MockDbContext(
                teamLeagues.AsQueryable(),
                teams.AsQueryable());
            var handler = new UpdateTeamLeagueMatchCommandHandler(contextMock.Object);

            //Act
            var request = new UpdateTeamLeagueMatchCommand
            {
                LeagueName = "TeamLeague",
                Guid = guid,
                HomeTeam = "Team A",
                AwayTeam = "Team B"
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<MatchNotFoundException>();
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public void Given_HomeTeamDoesNotExist_When_UpdateTeamLeagueMatch_Then_ThrowTeamNotFoundException()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var teamLeagues = CreateTeamLeagues(guid, true);
            var teams = CreateTeams();
            teams.Remove(teams.Single(t => t.Name == "Team A"));

            var contextMock = MockDbContext(
                teamLeagues.AsQueryable(),
                teams.AsQueryable());
            var handler = new UpdateTeamLeagueMatchCommandHandler(contextMock.Object);

            //Act
            var request = new UpdateTeamLeagueMatchCommand
            {
                LeagueName = "TeamLeague",
                Guid = guid,
                HomeTeam = "Team A",
                AwayTeam = "Team B"
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamNotFoundException>();
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public void Given_AwayTeamDoesNotExist_When_UpdateTeamLeagueMatch_Then_ThrowTeamNotFoundException()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var teamLeagues = CreateTeamLeagues(guid, true);
            var teams = CreateTeams();
            teams.Remove(teams.Single(t => t.Name == "Team B"));

            var contextMock = MockDbContext(
                teamLeagues.AsQueryable(),
                teams.AsQueryable());
            var handler = new UpdateTeamLeagueMatchCommandHandler(contextMock.Object);

            //Act
            var request = new UpdateTeamLeagueMatchCommand
            {
                LeagueName = "TeamLeague",
                Guid = guid,
                HomeTeam = "Team A",
                AwayTeam = "Team B"
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamNotFoundException>();
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}