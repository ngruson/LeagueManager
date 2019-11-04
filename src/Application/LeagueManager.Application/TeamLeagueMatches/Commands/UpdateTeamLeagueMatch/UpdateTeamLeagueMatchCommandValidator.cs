using FluentValidation;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch
{
    public class UpdateTeamLeagueMatchCommandValidator : AbstractValidator<UpdateTeamLeagueMatchCommand>
    {
        public UpdateTeamLeagueMatchCommandValidator()
        {
            RuleFor(x => x.HomeTeam)
                .NotEmpty()
                .WithMessage(x => "Home team cannot be empty");

            RuleFor(x => x.AwayTeam)
                .NotEmpty()
                .WithMessage(x => "Away team cannot be empty");
        }
    }
}