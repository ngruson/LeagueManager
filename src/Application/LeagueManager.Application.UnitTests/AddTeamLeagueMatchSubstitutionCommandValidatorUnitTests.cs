using FluentValidation.TestHelper;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchSubstitution;
using System;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class AddTeamLeagueMatchSubstitutionCommandValidatorUnitTests
    {
        [Fact]
        public void When_LeagueNameIsEmpty_Then_ValidationError()
        {
            var query = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = ""
            };

            var validator = new AddTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.LeagueName, query);
        }

        [Fact]
        public void When_MatchGuidIsEmpty_Then_ValidationError()
        {
            var query = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.Empty
            };

            var validator = new AddTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.MatchGuid, query);
        }

        [Fact]
        public void When_TeamNameIsEmpty_Then_ValidationError()
        {
            var query = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = ""
            };

            var validator = new AddTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.TeamName, query);
        }

        [Fact]
        public void When_MinuteIsEmpty_Then_ValidationError()
        {
            var query = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                Minute = ""
            };

            var validator = new AddTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Minute, query);
        }

        [Fact]
        public void When_PlayerOutIsEmpty_Then_ValidationError()
        {
            var query = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                Minute = "1",
                PlayerOut = ""
            };

            var validator = new AddTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.PlayerOut, query);
        }

        [Fact]
        public void When_PlayerInIsEmpty_Then_ValidationError()
        {
            var query = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                Minute = "1",
                PlayerOut = "John Doe",
                PlayerIn = ""
            };

            var validator = new AddTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.PlayerIn, query);
        }

        [Fact]
        public void When_CommandIsOk_Then_NoValidationErrors()
        {
            var query = new AddTeamLeagueMatchSubstitutionCommand
            {
                LeagueName = "Premier League",
                MatchGuid = Guid.NewGuid(),
                TeamName = "Tottenham Hotspur",
                Minute = "1",
                PlayerOut = "John Doe",
                PlayerIn = "Jane Doe"
            };

            var validator = new AddTeamLeagueMatchSubstitutionCommandValidator();
            validator.ShouldNotHaveValidationErrorFor(x => x.LeagueName, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.MatchGuid, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.TeamName, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.Minute, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.PlayerOut, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.PlayerIn, query);
        }
    }
}