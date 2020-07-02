using FluentValidation;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchSubstitution
{
    public class AddTeamLeagueMatchSubstitutionCommandValidator : AbstractValidator<AddTeamLeagueMatchSubstitutionCommand>
    {
        public AddTeamLeagueMatchSubstitutionCommandValidator()
        {
            RuleFor(x => x.LeagueName).NotEmpty();
            RuleFor(x => x.MatchGuid).NotEmpty();
            RuleFor(x => x.TeamName).NotEmpty();
            RuleFor(x => x.Minute).NotEmpty();
            RuleFor(x => x.PlayerOut).NotEmpty();
            RuleFor(x => x.PlayerIn).NotEmpty();
        }   
    }
}