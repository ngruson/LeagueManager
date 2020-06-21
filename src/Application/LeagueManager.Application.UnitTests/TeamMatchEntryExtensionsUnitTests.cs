using FluentAssertions;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class TeamMatchEntryExtensionsUnitTests
    {
        [Fact]
        public void Given_SubstitutionAndGoals_When_Events_Then_ReturnSubstitutionAndGoals()
        {
            //Arrange
            var mock = new Mock<ITeamMatchEntryWithDetailsDto>();
            mock.SetupGet(x => x.Goals).Returns(
                new List<IGoalDto>
                {
                    new GoalDto { Minute = "5" },
                    new GoalDto { Minute = "15" }
                }
            );
            mock.SetupGet(x => x.Substitutions).Returns(
                new List<ISubstitutionDto>
                {
                    new SubstitutionDto { Minute = "10" }
                }
            );

            //Act
            var events = mock.Object.Events();

            //Assert
            events.Count.Should().Be(3);
            events[0].Should().BeAssignableTo<IGoalDto>();
            events[1].Should().BeAssignableTo<ISubstitutionDto>();
            events[2].Should().BeAssignableTo<IGoalDto>();
        }

        [Fact]
        public void Given_Goals_When_Events_Then_ReturnGoals()
        {
            //Arrange
            var mock = new Mock<ITeamMatchEntryWithDetailsDto>();
            mock.SetupGet(x => x.Goals).Returns(
                new List<IGoalDto>
                {
                    new GoalDto { Minute = "5", Player = new PlayerDto { FirstName = "Player 1" } },
                    new GoalDto { Minute = "15", Player = new PlayerDto { FirstName = "Player 2" } }
                }
            );

            //Act
            var events = mock.Object.Events();

            //Assert
            events.Count.Should().Be(2);
            var goal = events[0].Should().BeAssignableTo<IGoalDto>().Subject;
            goal.Player.FirstName.Should().Be("Player 1");

            goal = events[1].Should().BeAssignableTo<IGoalDto>().Subject;
            goal.Player.FirstName.Should().Be("Player 2");
        }

        [Fact]
        public void Given_Substitutions_When_Events_Then_ReturnSubstitutions()
        {
            //Arrange
            var mock = new Mock<ITeamMatchEntryWithDetailsDto>();
            mock.SetupGet(x => x.Substitutions).Returns(
                new List<ISubstitutionDto>
                {
                    new SubstitutionDto { Minute = "5", PlayerOut = new PlayerDto { FirstName = "Player 1" } },
                    new SubstitutionDto { Minute = "15", PlayerOut = new PlayerDto { FirstName = "Player 2" } }
                }
            );

            //Act
            var events = mock.Object.Events();

            //Assert
            events.Count.Should().Be(2);
            var sub = events[0].Should().BeAssignableTo<ISubstitutionDto>().Subject;
            sub.PlayerOut.FirstName.Should().Be("Player 1");

            sub = events[1].Should().BeAssignableTo<ISubstitutionDto>().Subject;
            sub.PlayerOut.FirstName.Should().Be("Player 2");
        }

        [Fact]
        public void Given_NoEvents_When_Events_Then_ReturnEmptyList()
        {
            //Arrange
            var mock = new Mock<ITeamMatchEntryWithDetailsDto>();

            //Act
            var events = mock.Object.Events();

            //Assert
            events.Count.Should().Be(0);
        }
    }
}