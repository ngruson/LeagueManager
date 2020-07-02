using FluentValidation.TestHelper;
using LeagueManager.Application.Player.Commands.CreatePlayer;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class CreatePlayerCommandValidatorUnitTests
    {
        [Fact]
        public void When_FirstAndLastNameAreSet_Then_NoValidationError()
        {
            var command = new CreatePlayerCommand
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var validator = new CreatePlayerCommandValidator();
            validator.ShouldNotHaveValidationErrorFor(x => x.FirstName, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.LastName, command);
        }

        [Fact]
        public void When_FirstNameIsEmpty_Then_ValidationError()
        {
            var command = new CreatePlayerCommand
            {
                FirstName = ""
            };

            var validator = new CreatePlayerCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.FirstName, command);
        }

        [Fact]
        public void When_LastNameIsEmpty_Then_ValidationError()
        {
            var command = new CreatePlayerCommand
            {
                FirstName = "John",
                LastName = ""
            };

            var validator = new CreatePlayerCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.LastName, command);
        }
    }
}