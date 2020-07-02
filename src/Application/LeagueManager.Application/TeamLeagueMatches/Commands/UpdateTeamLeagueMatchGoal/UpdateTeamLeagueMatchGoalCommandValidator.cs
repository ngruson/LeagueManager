using FluentValidation;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal
{
    public class UpdateTeamLeagueMatchGoalCommandValidator : AbstractValidator<UpdateTeamLeagueMatchGoalCommand>
    {
        public UpdateTeamLeagueMatchGoalCommandValidator()
        {
            RuleFor(x => x.LeagueName).NotEmpty();
            RuleFor(x => x.MatchGuid).NotEmpty();
            RuleFor(x => x.TeamName).NotEmpty();
            RuleFor(x => x.GoalGuid).NotEmpty();
            RuleFor(x => x.Minute).NotEmpty();
            RuleFor(x => x.PlayerName).NotEmpty();
        }
    }
}