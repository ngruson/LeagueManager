using FluentValidation.TestHelper;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal;
using System;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class AddTeamLeagueMatchGoalCommandValidatorUnitTests
    {
        [Fact]
        public void When_LeagueNameIsEmpty_Then_ValidationError()
        {
            var query = new AddTeamLeagueMatchGoalCommand
            {
                LeagueName = ""
            };

            var validator = new AddTeamLeagueMatchGoalCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.LeagueName, query);
        }

        [Fact]
        public void When_MatchGuidIsEmpty_Then_ValidationError()
        {
            var query = new AddTeamLeagueMatchGoalCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.Empty
            };

            var validator = new AddTeamLeagueMatchGoalCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.MatchGuid, query);
        }

        [Fact]
        public void When_TeamNameIsEmpty_Then_ValidationError()
        {
            var query = new AddTeamLeagueMatchGoalCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = ""
            };

            var validator = new AddTeamLeagueMatchGoalCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.TeamName, query);
        }

        [Fact]
        public void When_MinuteIsEmpty_Then_ValidationError()
        {
            var query = new AddTeamLeagueMatchGoalCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                Minute = ""
            };

            var validator = new AddTeamLeagueMatchGoalCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Minute, query);
        }

        [Fact]
        public void When_PlayerNameIsEmpty_Then_ValidationError()
        {
            var query = new AddTeamLeagueMatchGoalCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                Minute = "1",
                PlayerName = ""
            };

            var validator = new AddTeamLeagueMatchGoalCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.PlayerName, query);
        }

        [Fact]
        public void When_CommandIsOk_Then_NoValidationErrors()
        {
            var query = new AddTeamLeagueMatchGoalCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                Minute = "1",
                PlayerName = "John Doe"
            };

            var validator = new AddTeamLeagueMatchGoalCommandValidator();
            validator.ShouldNotHaveValidationErrorFor(x => x.LeagueName, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.MatchGuid, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.TeamName, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.Minute, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.PlayerName, query);
        }
    }
}