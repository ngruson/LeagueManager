using MediatR;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeague
{
    public class GetTeamLeagueQuery : IRequest<GetTeamLeagueVm>
    {
        public string LeagueName { get; set; }
    }
}