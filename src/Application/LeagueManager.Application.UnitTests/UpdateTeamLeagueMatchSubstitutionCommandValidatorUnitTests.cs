using FluentValidation.TestHelper;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution;
using System;
using System.Collections.Generic;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class UpdateTeamLeagueMatchSubstitutionCommandValidatorUnitTests
    {
        [Fact]
        public void When_LeagueNameIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = ""
            };

            var validator = new UpdateTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.LeagueName, command);
        }

        [Fact]
        public void When_MatchGuidIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.Empty
            };

            var validator = new UpdateTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.MatchGuid, command);
        }

        [Fact]
        public void When_TeamNameIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = ""
            };

            var validator = new UpdateTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.TeamName, command);
        }

        [Fact]
        public void When_SubstitutionGuidIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                SubstitutionGuid = Guid.Empty
            };

            var validator = new UpdateTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.SubstitutionGuid, command);
        }

        [Fact]
        public void When_MinuteIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                SubstitutionGuid = Guid.NewGuid(),
                Minute = ""
            };

            var validator = new UpdateTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Minute, command);
        }


        [Fact]
        public void When_PlayerOutIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                SubstitutionGuid = Guid.NewGuid(),
                Minute = "1",
                PlayerOut = ""
            };

            var validator = new UpdateTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.PlayerOut, command);
        }

        [Fact]
        public void When_PlayerInIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                SubstitutionGuid = Guid.NewGuid(),
                Minute = "1",
                PlayerOut = "John Doe",
                PlayerIn = ""
            };

            var validator = new UpdateTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.PlayerIn, command);
        }

        [Fact]
        public void When_CommandIsOk_Then_NoValidationErrors()
        {
            var command = new UpdateTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                SubstitutionGuid = Guid.NewGuid(),
                Minute = "1",
                PlayerOut = "John Doe",
                PlayerIn = "Jane Doe"
            };

            var validator = new UpdateTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldNotHaveValidationErrorFor(x => x.LeagueName, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.MatchGuid, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.TeamName, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.SubstitutionGuid, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.Minute, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.PlayerOut, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.PlayerIn, command);
        }
    }
}