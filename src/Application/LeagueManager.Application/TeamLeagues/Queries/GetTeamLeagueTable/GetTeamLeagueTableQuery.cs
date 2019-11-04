using LeagueManager.Application.TeamLeagues.Queries.Dto;
using MediatR;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable
{
    public class GetTeamLeagueTableQuery : IRequest<TeamLeagueTableDto>
    {
        public string LeagueName { get; set; }
    }
}