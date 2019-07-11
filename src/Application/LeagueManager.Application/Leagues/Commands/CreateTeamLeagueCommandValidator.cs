using FluentValidation;

namespace LeagueManager.Application.Leagues.Commands
{
    public class CreateTeamLeagueCommandValidator : AbstractValidator<CreateTeamLeagueCommand>
    {
        public CreateTeamLeagueCommandValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50).NotEmpty();
        }
    }
}