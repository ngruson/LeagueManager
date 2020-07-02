using FluentValidation.TestHelper;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry;
using System;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class UpdateTeamLeagueMatchLineupEntryCommandValidatorUnitTests
    {
        [Fact]
        public void When_LeagueNameIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = ""
            };

            var validator = new UpdateTeamLeagueMatchLineupEntryCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.LeagueName, command);
        }

        [Fact]
        public void When_MatchGuidIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.Empty
            };

            var validator = new UpdateTeamLeagueMatchLineupEntryCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.MatchGuid, command);
        }

        [Fact]
        public void When_TeamNameIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = ""
            };

            var validator = new UpdateTeamLeagueMatchLineupEntryCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.TeamName, command);
        }

        [Fact]
        public void When_LineupEntryGuidIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                LineupEntryGuid = Guid.Empty
            };

            var validator = new UpdateTeamLeagueMatchLineupEntryCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.LineupEntryGuid, command);
        }

        [Fact]
        public void When_PlayerNumberIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                LineupEntryGuid = Guid.NewGuid(),
                PlayerNumber = ""
            };

            var validator = new UpdateTeamLeagueMatchLineupEntryCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.PlayerNumber, command);
        }

        [Fact]
        public void When_PlayerNameIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                LineupEntryGuid = Guid.NewGuid(),
                PlayerNumber = "1",
                PlayerName = ""
            };

            var validator = new UpdateTeamLeagueMatchLineupEntryCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.PlayerName, command);
        }

        [Fact]
        public void When_CommandIsOk_Then_NoValidationErrors()
        {
            var command = new UpdateTeamLeagueMatchLineupEntryCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                LineupEntryGuid = Guid.NewGuid(),
                PlayerNumber = "1",
                PlayerName = "John Doe"
            };

            var validator = new UpdateTeamLeagueMatchLineupEntryCommandValidator();
            validator.ShouldNotHaveValidationErrorFor(x => x.LeagueName, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.MatchGuid, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.TeamName, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.LineupEntryGuid, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.PlayerNumber, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.PlayerName, command);
        }
    }
}