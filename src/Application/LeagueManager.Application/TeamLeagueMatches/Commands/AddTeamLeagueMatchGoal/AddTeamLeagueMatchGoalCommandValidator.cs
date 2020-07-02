using FluentValidation;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal
{
    public class AddTeamLeagueMatchGoalCommandValidator : AbstractValidator<AddTeamLeagueMatchGoalCommand>
    {
        public AddTeamLeagueMatchGoalCommandValidator()
        {
            RuleFor(x => x.LeagueName).NotEmpty();
            RuleFor(x => x.MatchGuid).NotEmpty();
            RuleFor(x => x.TeamName).NotEmpty();
            RuleFor(x => x.Minute).NotEmpty();
            RuleFor(x => x.PlayerName).NotEmpty();
        }
    }
}