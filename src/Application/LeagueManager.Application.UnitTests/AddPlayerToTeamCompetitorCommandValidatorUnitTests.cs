using FluentValidation.TestHelper;
using LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class AddPlayerToTeamCompetitorCommandValidatorUnitTests
    {
        [Fact]
        public void When_LeagueNameIsEmpty_Then_ValidationError()
        {
            var command = new AddPlayerToTeamCompetitorCommand
            {
                LeagueName = ""
            };

            var validator = new AddPlayerToTeamCompetitorCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.LeagueName, command);
        }

        [Fact]
        public void When_TeamNameIsEmpty_Then_ValidationError()
        {
            var command = new AddPlayerToTeamCompetitorCommand
            {
                LeagueName = "Premier League",
                TeamName = ""
            };

            var validator = new AddPlayerToTeamCompetitorCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.TeamName, command);
        }

        [Fact]
        public void When_PlayerNameIsEmpty_Then_ValidationError()
        {
            var command = new AddPlayerToTeamCompetitorCommand
            {
                LeagueName = "Premier League",
                TeamName = "Tottenham Hotspur",
                PlayerName = ""
            };

            var validator = new AddPlayerToTeamCompetitorCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.PlayerName, command);
        }

        [Fact]
        public void When_PlayerNumberIsEmpty_Then_ValidationError()
        {
            var command = new AddPlayerToTeamCompetitorCommand
            {
                LeagueName = "Premier League",
                TeamName = "Tottenham Hotspur",
                PlayerName = "John Doe",
                PlayerNumber = ""
            };

            var validator = new AddPlayerToTeamCompetitorCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.PlayerNumber, command);
        }
    }
}