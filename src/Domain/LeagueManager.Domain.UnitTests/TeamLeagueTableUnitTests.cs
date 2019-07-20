using FluentAssertions;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.LeagueTable;
using LeagueManager.Domain.Match;
using LeagueManager.Domain.Points;
using LeagueManager.Domain.Round;
using LeagueManager.Domain.Score;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LeagueManager.Domain.UnitTests
{
    public class TeamLeagueTableUnitTests
    {
        [Fact]
        public void Given_GamesPlayed_When_CalculateTable_Then_CorrectTable()
        {
            //Arrange
            var pointSystem = new PointSystem(3, 1, 0);

            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };
            var teamManCity = new Team { Name = "Manchester City" };
            var teamTottenham = new Team { Name = "Tottenham Hotspur" };
            var teams = new List<Team> { teamLiverpool, teamChelsea, teamManCity, teamTottenham };
            teams = teams.OrderBy(t => t.Name).ToList();

            #region Rounds
            var rounds = new List<TeamLeagueRound>
            {
                new TeamLeagueRound
                {
                    Matches = new List<TeamMatch>
                    {
                        new TeamMatch
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
                                    Score = new IntegerScore { Value = 0 }
                                }
                            }
                        },
                        new TeamMatch
                        {
                            MatchEntries = new List<TeamMatchEntry>
                            {
                                new TeamMatchEntry
                                {
                                    HomeAway = HomeAway.Home,
                                    Team = teamManCity,
                                    Score = new IntegerScore { Value = 1 }
                                },
                                new TeamMatchEntry
                                {
                                    HomeAway = HomeAway.Away,
                                    Team = teamTottenham,
                                    Score = new IntegerScore { Value = 0 }
                                }
                            }
                        }
                    }
                },
                new TeamLeagueRound
                {
                    Matches = new List<TeamMatch>
                    {
                        new TeamMatch
                        {
                            MatchEntries = new List<TeamMatchEntry>
                            {
                                new TeamMatchEntry
                                {
                                    HomeAway = HomeAway.Home,
                                    Team = teamManCity,
                                    Score = new IntegerScore { Value = 1 }
                                },
                                new TeamMatchEntry
                                {
                                    HomeAway = HomeAway.Away,
                                    Team = teamLiverpool,
                                    Score = new IntegerScore { Value = 1 }
                                }
                            }
                        },
                        new TeamMatch
                        {
                            MatchEntries = new List<TeamMatchEntry>
                            {
                                new TeamMatchEntry
                                {
                                    HomeAway = HomeAway.Home,
                                    Team = teamTottenham,
                                    Score = new IntegerScore { Value = 2 }
                                },
                                new TeamMatchEntry
                                {
                                    HomeAway = HomeAway.Away,
                                    Team = teamChelsea,
                                    Score = new IntegerScore { Value = 2 }
                                }
                            }
                        }
                    }
                }
            };
            #endregion

            //Act
            var table = new TeamLeagueTable();
            table.CalculateTable(teams, rounds, pointSystem);

            //Assert
            var position1 = table.Items[0];
            position1.Position.Should().Be(1);
            position1.Team.Should().Be(teamLiverpool);
            position1.GamesWon.Should().Be(1);
            position1.GamesDrawed.Should().Be(1);
            position1.GamesLost.Should().Be(0);
            position1.GoalsFor.Should().Be(3);
            position1.GoalsAgainst.Should().Be(1);
            position1.GoalDifference.Should().Be(2);
            position1.Points.Should().Be(4);

            var position2 = table.Items[1];
            position2.Position.Should().Be(2);
            position2.Team.Should().Be(teamManCity);
            position2.GamesWon.Should().Be(1);
            position2.GamesDrawed.Should().Be(1);
            position2.GamesLost.Should().Be(0);
            position2.GoalsFor.Should().Be(2);
            position2.GoalsAgainst.Should().Be(1);
            position2.GoalDifference.Should().Be(1);
            position2.Points.Should().Be(4);

            var position3 = table.Items[2];
            position3.Position.Should().Be(3);
            position3.Team.Should().Be(teamTottenham);
            position3.GamesWon.Should().Be(0);
            position3.GamesDrawed.Should().Be(1);
            position3.GamesLost.Should().Be(1);
            position3.GoalsFor.Should().Be(2);
            position3.GoalsAgainst.Should().Be(3);
            position3.GoalDifference.Should().Be(-1);
            position3.Points.Should().Be(1);

            var position4 = table.Items[3];
            position4.Position.Should().Be(4);
            position4.Team.Should().Be(teamChelsea);
            position4.GamesWon.Should().Be(0);
            position4.GamesDrawed.Should().Be(1);
            position4.GamesLost.Should().Be(1);
            position4.GoalsFor.Should().Be(2);
            position4.GoalsAgainst.Should().Be(4);
            position4.GoalDifference.Should().Be(-2);
            position4.Points.Should().Be(1);
        }

        [Fact]
        public void Given_NoGamesPlayed_When_CalculateTable_Then_AllZeroesTable()
        {
            //Arrange
            var pointSystem = new PointSystem(3, 1, 0);

            var teamLiverpool = new Team { Name = "Liverpool" };
            var teamChelsea = new Team { Name = "Chelsea" };
            var teamManCity = new Team { Name = "Manchester City" };
            var teamTottenham = new Team { Name = "Tottenham Hotspur" };
            var teams = new List<Team> { teamLiverpool, teamChelsea, teamManCity, teamTottenham };
            teams = teams.OrderBy(t => t.Name).ToList();

            var rounds = new List<TeamLeagueRound>();

            //Act
            var table = new TeamLeagueTable();
            table.CalculateTable(teams, rounds, pointSystem);

            //Assert
            table.Items.ForEach(i =>
            {
                i.GamesWon.Should().Be(0);
                i.GamesDrawed.Should().Be(0);
                i.GamesLost.Should().Be(0);
                i.GoalsFor.Should().Be(0);
                i.GoalsAgainst.Should().Be(0);
                i.GoalDifference.Should().Be(0);
                i.Points.Should().Be(0);
            });

            table.Items[0].Team.Should().Be(teamChelsea);
            table.Items[1].Team.Should().Be(teamLiverpool);
            table.Items[2].Team.Should().Be(teamManCity);
            table.Items[3].Team.Should().Be(teamTottenham);
        }
    }
}