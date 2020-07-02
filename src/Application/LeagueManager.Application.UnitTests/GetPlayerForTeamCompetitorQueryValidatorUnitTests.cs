using FluentValidation.TestHelper;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetPlayerForTeamCompetitorQueryValidatorUnitTests
    {
        [Fact]
        public void When_LeagueNameIsEmpty_Then_ValidationError()
        {
            var query = new GetPlayerForTeamCompetitorQuery
            {
                LeagueName = ""                
            };

            var validator = new GetPlayerForTeamCompetitorQueryValidator();
            validator.ShouldHaveValidationErrorFor(x => x.LeagueName, query);
        }

        [Fact]
        public void When_TeamNameIsEmpty_Then_ValidationError()
        {
            var query = new GetPlayerForTeamCompetitorQuery
            {
                LeagueName = "Premier League",
                TeamName = ""
            };

            var validator = new GetPlayerForTeamCompetitorQueryValidator();
            validator.ShouldHaveValidationErrorFor(x => x.TeamName, query);
        }

        [Fact]
        public void When_PlayerNameIsEmpty_Then_ValidationError()
        {
            var query = new GetPlayerForTeamCompetitorQuery
            {
                LeagueName = "Premier League",
                TeamName = "Tottenham Hotspur",
                PlayerName = ""
            };

            var validator = new GetPlayerForTeamCompetitorQueryValidator();
            validator.ShouldHaveValidationErrorFor(x => x.PlayerName, query);
        }

        [Fact]
        public void When_QueryIsOk_Then_NoValidationError()
        {
            var query = new GetPlayerForTeamCompetitorQuery
            {
                LeagueName = "Premier League",
                TeamName = "Tottenham Hotspur",
                PlayerName = "John Doe"
            };

            var validator = new GetPlayerForTeamCompetitorQueryValidator();
            validator.ShouldNotHaveValidationErrorFor(x => x.LeagueName, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.TeamName, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.PlayerName, query);
        }
    }
}