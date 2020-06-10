using AutoMapper;
using FluentAssertions;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.UnitTests.TestData;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Round;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetTeamLeagueRoundsQueryUnitTests
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
                Competitors = CreateCompetitors(),
                Rounds = new List<TeamLeagueRound>
                {
                    new TeamLeagueRound
                    {
                        Name = "Round 2",
                        Matches = CreateMatches()
                    },
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

        private List<Domain.Competitor.TeamCompetitor> CreateCompetitors()
        {
            return new List<Domain.Competitor.TeamCompetitor>
            {
                new Domain.Competitor.TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Liverpool"
                    }
                },
                new Domain.Competitor.TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Manchester City"
                    }
                },
                new Domain.Competitor.TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Chelsea"
                    }
                },
                new Domain.Competitor.TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Tottenham Hotspur"
                    }
                }
            };
        }

        [Fact]
        public async void Given_NoTeamLeaguesExist_When_GetTeamLeagueRounds_Then_ReturnNull()
        {
            // Arrange
            var leagues = new List<TeamLeague>();
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueRoundsQueryHandler(
                contextMock.Object, CreateMapper());

            //Act
            var result = await handler.Handle(new GetTeamLeagueRoundsQuery { LeagueName = "Premier League" }, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async void Given_TeamLeagueExist_When_GetTeamLeagueRounds_Then_ReturnRounds()
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
            var handler = new GetTeamLeagueRoundsQueryHandler(
                contextMock.Object, CreateMapper());

            //Act
            var result = await handler.Handle(new GetTeamLeagueRoundsQuery { LeagueName = "Premier League" }, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Rounds.Count().Should().Be(1);
            result.Rounds.Should().BeInAscendingOrder(x => x.Name);
        }
    }
}