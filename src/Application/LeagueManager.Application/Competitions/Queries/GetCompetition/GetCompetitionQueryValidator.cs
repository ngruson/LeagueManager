using FluentValidation;

namespace LeagueManager.Application.Competitions.Queries.GetCompetition
{
    public class GetCompetitionQueryValidator : AbstractValidator<GetCompetitionQuery>
    {
        public GetCompetitionQueryValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}