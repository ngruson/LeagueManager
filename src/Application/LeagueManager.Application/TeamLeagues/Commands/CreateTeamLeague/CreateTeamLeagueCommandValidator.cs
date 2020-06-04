using FluentValidation;

namespace LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague
{
    public class CreateTeamLeagueCommandValidator : AbstractValidator<CreateTeamLeagueCommand>
    {
        public CreateTeamLeagueCommandValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(50)
                .WithMessage(x => "Name cannot be longer than 50 characters");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(x => "Name cannot be empty");
            
            RuleFor(x => x.Name)
                .Must(x => !x.Contains("/"))
                .WithMessage(x => "Name cannot contain '/'");

            RuleFor(x => x.Sports)
                .NotEmpty()
                .WithMessage(x => "Sports cannot be empty");

            RuleFor(x => x.SelectedTeams)
                .NotNull()
                .Must(x => x != null && x.Count > 0)
                .WithMessage(x => "No teams are specified");

            RuleFor(x => x.SelectedTeams)
                .Must(x => x != null && x.Count % 2 == 0)
                .WithMessage("Amount of teams cannot be an odd number");
        }
    }
}