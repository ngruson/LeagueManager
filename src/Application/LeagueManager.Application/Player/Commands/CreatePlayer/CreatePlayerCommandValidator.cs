using FluentValidation;

namespace LeagueManager.Application.Player.Commands.CreatePlayer
{
    public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
    {
        public CreatePlayerCommandValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty();
            RuleFor(p => p.LastName).NotEmpty();
        }
    }
}