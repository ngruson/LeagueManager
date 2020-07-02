using FluentValidation.TestHelper;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetPlayersForTeamCompetitorQueryValidatorUnitTests
    {
        [Fact]
        public void When_LeagueNameIsEmpty_Then_ValidationError()
        {
            var query = new GetPlayersForTeamCompetitorQuery
            {
                LeagueName = ""           
            };

            var validator = new GetPlayersForTeamCompetitorQueryValidator();
            validator.ShouldHaveValidationErrorFor(x => x.LeagueName, query);
        }

        [Fact]
        public void When_TeamNameIsEmpty_Then_ValidationError()
        {
            var query = new GetPlayersForTeamCompetitorQuery
            {
                LeagueName = "Premier League",
                TeamName = ""
            };

            var validator = new GetPlayersForTeamCompetitorQueryValidator();
            validator.ShouldHaveValidationErrorFor(x => x.TeamName, query);
        }

        [Fact]
        public void When_QueryIsOk_Then_NoValidationError()
        {
            var query = new GetPlayersForTeamCompetitorQuery
            {
                LeagueName = "Premier League",
                TeamName = "Tottenham Hotspur",
            };

            var validator = new GetPlayersForTeamCompetitorQueryValidator();
            validator.ShouldNotHaveValidationErrorFor(x => x.LeagueName, query);
            validator.ShouldNotHaveValidationErrorFor(x => x.TeamName, query);
        }
    }
}