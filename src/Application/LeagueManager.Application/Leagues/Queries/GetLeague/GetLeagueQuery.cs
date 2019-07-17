using MediatR;

namespace LeagueManager.Application.Leagues.Queries.GetLeague
{
    public class GetLeagueQuery : IRequest<LeagueDto>
    {
        public string Name { get; set; }
    }
}