using FluentValidation;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution
{
    public class UpdateTeamLeagueMatchSubstitutionCommandValidator : AbstractValidator<UpdateTeamLeagueMatchSubstitutionCommand>
    {
        public UpdateTeamLeagueMatchSubstitutionCommandValidator()
        {
            RuleFor(x => x.LeagueName).NotEmpty();
            RuleFor(x => x.MatchGuid).NotEmpty();
            RuleFor(x => x.TeamName).NotEmpty();
            RuleFor(x => x.SubstitutionGuid).NotEmpty();
            RuleFor(x => x.Minute).NotEmpty();
            RuleFor(x => x.PlayerOut).NotEmpty();
            RuleFor(x => x.PlayerIn).NotEmpty();
        }
    }
}