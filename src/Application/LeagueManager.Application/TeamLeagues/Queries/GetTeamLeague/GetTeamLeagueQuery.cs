using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeague
{
    public class GetTeamLeagueQuery : IRequest<GetTeamLeagueVm>
    {
        public string LeagueName { get; set; }
    }
}