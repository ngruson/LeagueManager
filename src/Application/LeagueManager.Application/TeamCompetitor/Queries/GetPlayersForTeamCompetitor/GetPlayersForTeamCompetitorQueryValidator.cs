using FluentValidation;
using System.Data;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor
{
    public class GetPlayersForTeamCompetitorQueryValidator : AbstractValidator<GetPlayersForTeamCompetitorQuery>
    {
        public GetPlayersForTeamCompetitorQueryValidator()
        {
            RuleFor(x => x.LeagueName).NotEmpty();
            RuleFor(x => x.TeamName).NotEmpty();
        }
    }
}