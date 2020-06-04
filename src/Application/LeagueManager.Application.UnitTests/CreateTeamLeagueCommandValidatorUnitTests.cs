using FluentValidation.TestHelper;
using Xunit;
using System.Collections.Generic;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague;

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
                SelectedTeams = new List<string> { "Team A", "Team B" }
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
                SelectedTeams = new List<string> { "Team A", "Team B" }
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
                SelectedTeams = new List<string> {  "Team A", "Team B" }
            };
            var validator = new CreateTeamLeagueCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Fact]
        public void When_SportsIsEmpty_Then_ValidationError()
        {
            var command = new CreateTeamLeagueCommand
            {
                Name = "Premier League",
                Sports = "",
                SelectedTeams = new List<string> { "Team A", "Team B" }
            };
            var validator = new CreateTeamLeagueCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Sports, command);
        }

        [Fact]
        public void When_NoTeamsAreSpecified_Then_ValidationError()
        {
            var command = new CreateTeamLeagueCommand
            {
                Name = "Premier League 2019-2020",
                SelectedTeams = null
            };
            var validator = new CreateTeamLeagueCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.SelectedTeams, command);
        }
    }
}