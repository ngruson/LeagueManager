using FluentAssertions;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Points;
using LeagueManager.Domain.Score;
using System.Collections.Generic;
using Xunit;

namespace LeagueManager.Domain.UnitTests
{
    public class TeamMatchUnitTests
    {
        [Fact]
        public void Given_HomeTeamIsWinner_When_Winner_Then_HomeTeamIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 1 }
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 0 }
                    }
                }
            };

            //Assert
            match.Winner.Should().Be(teamLiverpool);
        }

        [Fact]
        public void Given_AwayTeamIsWinner_When_Winner_Then_AwayTeamIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 0 }
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 1 }
                    }
                }
            };

            //Assert
            match.Winner.Should().Be(teamChelsea);
        }

        [Fact]
        public void Given_HomeTeamIsLoser_When_Loser_Then_HomeTeamIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 0 }
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 1 }
                    }
                }
            };

            //Assert
            match.Loser.Should().Be(teamLiverpool);
        }

        [Fact]
        public void Given_AwayTeamIsLoser_When_Loser_Then_AwayTeamIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 1 }
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 0 }
                    }
                }
            };

            //Assert
            match.Loser.Should().Be(teamChelsea);
        }

        [Fact]
        public void Given_GameIsDraw_When_Winner_Then_NullIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 1 }
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 1 }
                    }
                }
            };

            //Assert
            match.Winner.Should().BeNull();
        }

        [Fact]
        public void Given_GameIsDraw_When_Loser_Then_NullIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 1 }
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 1 }
                    }
                }
            };

            //Assert
            match.Loser.Should().BeNull();
        }

        [Fact]
        public void Given_GameIsDraw_When_IsDraw_Then_TrueIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 1 }
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 1 }
                    }
                }
            };

            //Assert
            match.IsDraw.Should().Be(true);
        }

        [Fact]
        public void Given_GameIsNotADraw_When_IsDraw_Then_FalseIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 1 }
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 0 }
                    }
                }
            };

            //Assert
            match.IsDraw.Should().Be(false);
        }

        [Fact]
        public void Given_ScoreForHomeTeamIsSet_When_GetGoalsForHomeTeam_Then_GoalsForHomeTeamAreReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 2 }
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 1 }
                    }
                }
            };

            //Assert
            match.GetGoalsFor(teamLiverpool).Should().Be(2);
        }

        [Fact]
        public void Given_ScoreForAwayTeamIsSet_When_GetGoalsForAwayTeam_Then_GoalsForAwayTeamAreReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 2 }
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 1 }
                    }
                }
            };

            //Assert
            match.GetGoalsFor(teamChelsea).Should().Be(1);
        }

        [Fact]
        public void Given_MatchEntryDoesNotExistForTeam_When_GetGoalsForTeam_Then_NullIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var match = new TeamMatch();

            //Assert
            match.GetGoalsFor(teamLiverpool).Should().Be(0);
        }

        [Fact]
        public void Given_ScoreForOpponentIsSet_When_GetGoalsAgainst_Then_GoalsForOpponentAreReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 2 }
                    },
                    new TeamMatchEntry
                    {
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 1 }
                    }
                }
            };

            //Assert
            match.GetGoalsAgainst(teamLiverpool).Should().Be(1);
        }

        [Fact]
        public void Given_NoMatchEntries_When_GetGoalsAgainst_Then_ZeroIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch();

            //Assert
            match.GetGoalsAgainst(teamLiverpool).Should().Be(0);
        }

        [Fact]
        public void Given_NoScores_When_GetGoalsAgainst_Then_ZeroIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        Team = teamLiverpool
                    },
                    new TeamMatchEntry
                    {
                        Team = teamChelsea
                    }
                }
            };

            //Assert
            match.GetGoalsAgainst(teamLiverpool).Should().Be(0);
        }

        [Fact]
        public void Given_NoScoreValues_When_GetGoalsAgainst_Then_ZeroIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        Team = teamLiverpool,
                        Score = new IntegerScore()
                    },
                    new TeamMatchEntry
                    {
                        Team = teamChelsea,
                        Score = new IntegerScore()
                    }
                }
            };

            //Assert
            match.GetGoalsAgainst(teamLiverpool).Should().Be(0);
        }

        [Fact]
        public void Given_Win_When_GetPointsFor_Then_WinPointsAreReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 1}
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 0 }
                    }
                }
            };

            var pointSystem = new PointSystem(3, 1, 0);

            //Assert
            match.GetPointsFor(teamLiverpool, pointSystem).Should().Be(3);
        }

        [Fact]
        public void Given_Draw_When_GetPointsFor_Then_DrawPointsAreReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 1}
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 1 }
                    }
                }
            };

            var pointSystem = new PointSystem(3, 1, 0);

            //Assert
            match.GetPointsFor(teamLiverpool, pointSystem).Should().Be(1);
        }

        [Fact]
        public void Given_Lost_When_GetPointsFor_Then_LostPointsAreReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };

            var match = new TeamMatch
            {
                MatchEntries = new List<TeamMatchEntry>
                {
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Home,
                        Team = teamLiverpool,
                        Score = new IntegerScore { Value = 1}
                    },
                    new TeamMatchEntry
                    {
                        HomeAway = HomeAway.Away,
                        Team = teamChelsea,
                        Score = new IntegerScore { Value = 2 }
                    }
                }
            };

            var pointSystem = new PointSystem(3, 1, 0);

            //Assert
            match.GetPointsFor(teamLiverpool, pointSystem).Should().Be(0);
        }

        [Fact]
        public void Given_NoMatchEntries_When_GetPointsFor_Then_ZeroIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };            
            var match = new TeamMatch();
            var pointSystem = new PointSystem(3, 1, 0);

            //Assert
            match.GetPointsFor(teamLiverpool, pointSystem).Should().Be(0);
        }

        [Fact]
        public void Given_NoPointSystem_When_GetPointsFor_Then_ZeroIsReturned()
        {
            //Act
            var teamLiverpool = new Team { Name = "Liverpool" };
            var match = new TeamMatch();

            //Assert
            match.GetPointsFor(teamLiverpool, null).Should().Be(0);
        }
    }    
}