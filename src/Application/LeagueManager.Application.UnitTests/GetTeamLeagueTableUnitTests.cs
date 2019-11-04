using AutoMapper;
using FluentAssertions;
using LeagueManager.Application.AutoMapper;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Round;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetTeamLeagueTableUnitTests
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
                opts.AddProfile<ApplicationProfile>();
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

        private List<TeamMatch> CreateMatches()
        {
            return new List<TeamMatch>
            {
                new TeamMatch
                {
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
            };
        }

        private List<TeamCompetitor> CreateCompetitors()
        {
            return new List<TeamCompetitor>
            {
                new TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Liverpool"
                    }
                },
                new TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Manchester City"
                    }
                },
                new TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Chelsea"
                    }
                },
                new TeamCompetitor
                {
                    Team = new Team
                    {
                        Name = "Tottenham Hotspur"
                    }
                }
            };
        }

        [Fact]
        public async void Given_NoTeamLeaguesExist_When_GetTeamLeagueTable_Then_ReturnNull()
        {
            // Arrange
            var leagues = new List<TeamLeague>();
            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueTableQueryHandler(
                contextMock.Object, CreateMapper());

            //Act
            var result = await handler.Handle(new GetTeamLeagueTableQuery { LeagueName = "Premier League" }, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async void Given_TeamLeagueExist_When_GetTeamLeagueTable_Then_ReturnTable()
        {
            // Arrange
            var leagues = new List<TeamLeague> {
                CreateTeamLeagueWithRoundsAndMatches("Premier League"),
                CreateTeamLeagueWithRoundsAndMatches("Primera Division")
            };

            var contextMock = MockDbContext(leagues.AsQueryable());
            var handler = new GetTeamLeagueTableQueryHandler(
                contextMock.Object, CreateMapper());

            //Act
            var result = await handler.Handle(new GetTeamLeagueTableQuery { LeagueName = "Premier League" }, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Count.Should().Be(4);
        }
    }
}