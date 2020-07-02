using FluentValidation;

namespace LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor
{
    public class AddPlayerToTeamCompetitorCommandValidator : AbstractValidator<AddPlayerToTeamCompetitorCommand>
    {
        public AddPlayerToTeamCompetitorCommandValidator()
        {
            RuleFor(c => c.LeagueName).NotEmpty();
            RuleFor(c => c.TeamName).NotEmpty();
            RuleFor(c => c.PlayerNumber).NotEmpty();
            RuleFor(c => c.PlayerName).NotEmpty();
        }
    }
}