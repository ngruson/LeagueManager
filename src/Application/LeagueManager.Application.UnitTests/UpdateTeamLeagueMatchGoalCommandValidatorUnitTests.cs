using FluentValidation.TestHelper;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using System;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class UpdateTeamLeagueMatchGoalCommandValidatorUnitTests
    {
        [Fact]
        public void When_LeagueNameIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchGoalCommand
            {
                LeagueName = ""
            };

            var validator = new UpdateTeamLeagueMatchGoalCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.LeagueName, command);
        }

        [Fact]
        public void When_MatchGuidIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchGoalCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.Empty
            };

            var validator = new UpdateTeamLeagueMatchGoalCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.MatchGuid, command);
        }

        [Fact]
        public void When_TeamNameIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchGoalCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = ""
            };

            var validator = new UpdateTeamLeagueMatchGoalCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.TeamName, command);
        }

        [Fact]
        public void When_GoalGuidIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchGoalCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                GoalGuid = Guid.Empty
            };

            var validator = new UpdateTeamLeagueMatchGoalCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.GoalGuid, command);
        }

        [Fact]
        public void When_MinuteIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchGoalCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                GoalGuid = Guid.NewGuid(),
                Minute = ""
            };

            var validator = new UpdateTeamLeagueMatchGoalCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Minute, command);
        }

        [Fact]
        public void When_PlayerNameIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchGoalCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                GoalGuid = Guid.NewGuid(),
                Minute = "1",
                PlayerName = ""
            };

            var validator = new UpdateTeamLeagueMatchGoalCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.PlayerName, command);
        }

        [Fact]
        public void When_CommandIsOk_Then_NoValidationErrors()
        {
            var command = new UpdateTeamLeagueMatchGoalCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                GoalGuid = Guid.NewGuid(),
                Minute = "1",
                PlayerName = "John Doe"
            };

            var validator = new UpdateTeamLeagueMatchGoalCommandValidator();
            validator.ShouldNotHaveValidationErrorFor(x => x.LeagueName, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.MatchGuid, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.TeamName, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.GoalGuid, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.Minute, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.PlayerName, command);
        }
    }
}