using FluentValidation.TestHelper;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class UpdateTeamLeagueMatchCommandValidatorUnitTests
    {
        [Fact]
        public void When_HomeTeamIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchCommand
            {
                HomeTeam = ""
            };
            var validator = new UpdateTeamLeagueMatchCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.HomeTeam, command);
        }

        [Fact]
        public void When_AwayTeamIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchCommand
            {
                AwayTeam = ""
            };
            var validator = new UpdateTeamLeagueMatchCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.AwayTeam, command);
        }
    }
}