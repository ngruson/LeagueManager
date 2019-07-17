using LeagueManager.Application.Leagues.Commands;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;
using System.Collections.Generic;

namespace LeagueManager.Application.UnitTests
{
    public class CreateTeamLeagueCommandValidatorUnitTests
    {
        [Fact]
        public void When_NameIsEmpty_Then_ValidationError()
        {
            var command = new CreateTeamLeagueCommand
            {
                Name = "",
                Teams = new List<string> { "Team A", "Team B" }
            };
            var validator = new CreateTeamLeagueCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Fact]
        public void When_NameContainsSlash_Then_ValidationError()
        {
            var command = new CreateTeamLeagueCommand
            {
                Name = "Premier League 2019/2020",
                Teams = new List<string> { "Team A", "Team B" }
            };
            var validator = new CreateTeamLeagueCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Fact]
        public void When_NameIsLongerThan50_Then_ValidationError()
        {
            var command = new CreateTeamLeagueCommand
            {
                Name = "Premier LeaguePremier LeaguePremier LeaguePremier LeaguePremier LeaguePremier LeaguePremier LeaguePremier LeaguePremier League",
                Teams = new List<string> {  "Team A", "Team B" }
            };
            var validator = new CreateTeamLeagueCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Fact]
        public void When_NoTeamsAreSpecified_Then_ValidationError()
        {
            var command = new CreateTeamLeagueCommand
            {
                Name = "Premier League 2019-2020",
                Teams = null
            };
            var validator = new CreateTeamLeagueCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Teams, command);
        }
    }
}
