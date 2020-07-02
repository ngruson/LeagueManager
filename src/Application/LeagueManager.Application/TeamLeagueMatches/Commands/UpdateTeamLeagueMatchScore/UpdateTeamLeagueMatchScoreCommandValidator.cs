using FluentValidation;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore
{
    public class UpdateTeamLeagueMatchScoreCommandValidator : AbstractValidator<UpdateTeamLeagueMatchScoreCommand>
    {
        public UpdateTeamLeagueMatchScoreCommandValidator()
        {
            RuleFor(x => x.LeagueName).NotEmpty();
            RuleFor(x => x.Guid).NotEmpty();
            RuleForEach(x => x.MatchEntries)
                .NotNull();
        }
    }
}