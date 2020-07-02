using FluentValidation;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor
{
    public class GetPlayerForTeamCompetitorQueryValidator : AbstractValidator<GetPlayerForTeamCompetitorQuery>
    {
        public GetPlayerForTeamCompetitorQueryValidator()
        {
            RuleFor(x => x.LeagueName).NotEmpty();
            RuleFor(x => x.TeamName).NotEmpty();
            RuleFor(x => x.PlayerName).NotEmpty();
        }
    }
}