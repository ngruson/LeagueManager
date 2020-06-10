using AutoMapper;
using FluentAssertions;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.UnitTests.TestData;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Round;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using static LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch.GetTeamLeagueMatchQuery;

namespace LeagueManager.Application.UnitTests
{
    public class GetTeamLeagueMatchQueryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<TeamLeague> leagues)
        {
            var leaguesDbSet = leagues.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.TeamLeagues).Returns(leaguesDbSet.Object);
            return mockContext;
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile<MappingProfile>();
            });

            return config.CreateMapper();
        }

        private TeamLeague CreateTeamLeagueWithRoundsAndMatches(string name)
        {
            return new TeamLeague
            {
                Name = name,
                Rounds = new List<TeamLeagueRound>
                {
                    new TeamLeagueRound
                    {
                        Name = "Round 1",
                        Matches = CreateMatches()
                    }
                }
            };
        }

        private List<TeamLeagueMatch> CreateMatches()
        {
            return new List<TeamLeagueMatch>
            {
                new TeamLeagueMatch
                {
                    Guid = new Guid("77E49557-62F0-4FE5-8A96-52251F108FE3"),
                    MatchEntries = new List<TeamMatchEntry>
                    {
                        new TeamMatchEntry
                        {
                            HomeAway = Domain.Match.HomeAway.Home
                        },
                        new TeamMatchEntry
                        {
                            HomeAway = Domain.Match.HomeAway.Away
                        }
                    }
                }
            };
        }

        [Fact]
        public async void Given_MatchDoesNotExist_When_GetTeamLeagueMatch_Then_ReturnNull()
        {
            // Arrange
            var teamLeague = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds()
                .Build();

            var leagues = new List<TeamLeague> {
                teamLeague
            };
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueMatchQueryHandler(
                contextMock.Object, CreateMapper());

            //Act
            var request = new GetTeamLeagueMatchQuery
            {
                LeagueName = "Premier League",
                Guid = new Guid("77E49557-62F0-4FE5-8A96-52251F108FE4")
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async void Given_MatchDoesExist_When_GetTeamLeagueMatch_Then_ReturnMatch()
        {
            // Arrange
            var teamLeague = new TeamLeagueBuilder()
                .WithCompetitors(new TeamsBuilder().Build())
                .WithRounds()
                .Build();

            var leagues = new List<TeamLeague> {
                teamLeague
            };

            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueMatchQueryHandler(
                contextMock.Object, CreateMapper());

            //Act
            var guid = new Guid("00000000-0000-0000-0000-000000000000");
            var request = new GetTeamLeagueMatchQuery
            {
                LeagueName = "Premier League",
                Guid = guid
            };
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Guid.Should().Be(guid);
        }
    }
}