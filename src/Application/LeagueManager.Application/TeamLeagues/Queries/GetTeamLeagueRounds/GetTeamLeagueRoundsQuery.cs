using MediatR;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds
{
    public class GetTeamLeagueRoundsQuery : IRequest<GetTeamLeagueRoundsVm>
    {
        public string LeagueName { get; set; }
    }
}