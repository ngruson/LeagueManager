using FluentValidation.TestHelper;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetCompetitionQueryValidatorUnitTests
    {
        [Fact]
        public void When_NameIsEmpty_Then_ValidationError()
        {
            var command = new GetCompetitionQuery
            {
                Name = ""
            };

            var validator = new GetCompetitionQueryValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Fact]
        public void When_NameIsSet_Then_NoValidationError()
        {
            var command = new GetCompetitionQuery
            {
                Name = "Premier League"
            };

            var validator = new GetCompetitionQueryValidator();
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, command);
        }
    }
}