using MediatR;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable
{
    public class GetTeamLeagueTableQuery : IRequest<GetTeamLeagueTableVm>
    {
        public string LeagueName { get; set; }
    }
}