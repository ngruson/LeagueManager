using FluentAssertions;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using MediatR;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using System;
using System.Threading.Tasks;
using LeagueManager.Application.Exceptions;
using LeagueManager.Domain.Sports;
using LeagueManager.Domain.Common;
using LeagueManager.Application.UnitTests.TestData;
using LeagueManager.Application.TeamLeagues.Dto;

namespace LeagueManager.Application.UnitTests
{
    public class CreateTeamLeagueCommandUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(
            IQueryable<TeamSports> teamSports,
            IQueryable<Country> countries,
            IQueryable<TeamLeague> leagues,
            IQueryable<Team> teams)
        {
            var teamSportsDbSet = teamSports.BuildMockDbSet();
            var countriesDbSet = countries.BuildMockDbSet();
            var leaguesDbSet = leagues.BuildMockDbSet();
            var teamsDbSet = teams.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamSports).Returns(teamSportsDbSet.Object);
            mockContext.Setup(c => c.Countries).Returns(countriesDbSet.Object);
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            mockContext.Setup(c => c.Teams).Returns(teamsDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_TeamLeagueDoesNotExist_When_CreateTeamLeague_Then_TeamLeagueIsCreated()
        {
            // Arrange
            var teamSports = new List<TeamSports> { new TeamSports {
                Name = "Soccer",
                Options = new TeamSportsOptions { AmountOfPlayers = 11 }
            }};
            var countries = new List<Country>();
            var leagues = new List<TeamLeague>();
            var teams = new TeamsBuilder().Build();

            var contextMock = MockDbContext(
                teamSports.AsQueryable(),
                countries.AsQueryable(),
                leagues.AsQueryable(), 
                teams.AsQueryable());
            var handler = new CreateTeamLeagueCommandHandler(contextMock.Object, Mapper.CreateMapper());

            //Act
            var request = new CreateTeamLeagueCommand {
                Sports = "Soccer",
                Name = "Premier League",
                Teams = teams.Select(t => t.Name).ToList()
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().BeOfType<TeamLeagueDto>();
            result.Rounds.Count().Should().Be((teams.Count - 1) * 2);
            result.Rounds.ToList().ForEach(r => {
                r.Matches.Count().Should().Be(teams.Count / 2);
                r.Matches.ToList().ForEach(m => {
                    m.MatchEntries.Count().Should().Be(2);
                    m.MatchEntries.ForEach(me => me.Lineup.Count().Should().Be(teamSports[0].Options.AmountOfPlayers));
                });
            });
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public void Given_TeamLeagueAlreadyExist_When_CreateTeamLeague_Then_Exception()
        {
            // Arrange
            var teamSports = new List<TeamSports>();
            var countries = new List<Country>();
            var leagues = new List<TeamLeague> {
                new TeamLeague { Name = "Premier League"}
            };
            var teams = new TeamsBuilder().Build();

            var contextMock = MockDbContext(
                teamSports.AsQueryable(),
                countries.AsQueryable(),
                leagues.AsQueryable(),
                teams.AsQueryable());
            var handler = new CreateTeamLeagueCommandHandler(contextMock.Object, Mapper.CreateMapper());

            //Act
            var request = new CreateTeamLeagueCommand
            {
                Name = "Premier League",
                Teams = teams.Select(t => t.Name).ToList()
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<CompetitionAlreadyExistsException>();
        }

        [Fact]
        public void Given_SportsDoesNotExist_When_CreateTeamLeague_Then_Exception()
        {
            // Arrange
            var sports = new List<TeamSports> { new TeamSports { Name = "Soccer" } };
            var countries = new List<Country>();
            var leagues = new List<TeamLeague>();
            var teams = new TeamsBuilder().Build();

            var contextMock = MockDbContext(
                sports.AsQueryable(),
                countries.AsQueryable(),
                leagues.AsQueryable(),
                teams.AsQueryable()); ;
            var handler = new CreateTeamLeagueCommandHandler(contextMock.Object, Mapper.CreateMapper());

            //Act
            var request = new CreateTeamLeagueCommand
            {
                Sports = "DoesNotExist",
                Name = "Premier League",
                Teams = teams.Select(t => t.Name).ToList()
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<SportsNotFoundException>();
        }

        [Fact]
        public void Given_CountryDoesNotExist_When_CreateTeamLeague_Then_Exception()
        {
            // Arrange
            var sports = new List<TeamSports> { new TeamSports { Name = "Soccer" } };
            var countries = new List<Country>();
            var leagues = new List<TeamLeague>();
            var teams = new TeamsBuilder().Build();

            var contextMock = MockDbContext(
                sports.AsQueryable(),
                countries.AsQueryable(),
                leagues.AsQueryable(),
                teams.AsQueryable()); ;
            var handler = new CreateTeamLeagueCommandHandler(contextMock.Object, Mapper.CreateMapper());

            //Act
            var request = new CreateTeamLeagueCommand
            {
                Sports = "Soccer",
                Country = "DoesNotExist",
                Name = "Premier League",
                Teams = teams.Select(t => t.Name).ToList()
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<CountryNotFoundException>();
        }

        [Fact]
        public void Given_TeamDoesNotExist_When_CreateTeamLeague_Then_Exception()
        {
            // Arrange
            var sports = new List<TeamSports> { new TeamSports { Name = "Soccer" } };
            var countries = new List<Country> { new Country { Name = "England" } };
            var leagues = new List<TeamLeague>();
            var teams = new List<Team>();

            var contextMock = MockDbContext(
                sports.AsQueryable(),
                countries.AsQueryable(),
                leagues.AsQueryable(),
                teams.AsQueryable()); ;
            var handler = new CreateTeamLeagueCommandHandler(contextMock.Object, Mapper.CreateMapper());

            //Act
            var request = new CreateTeamLeagueCommand
            {
                Sports = "Soccer",
                Country = "England",
                Name = "Premier League",
                Teams = new TeamsBuilder().Build().Select(t => t.Name).ToList()
            };
            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            //Assert
            func.Should().Throw<TeamNotFoundException>();
        }
    }
}