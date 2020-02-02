using LeagueManager.Application.TeamLeagues.Dto;
using MediatR;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable
{
    public class GetTeamLeagueTableQuery : IRequest<TeamLeagueTableDto>
    {
        public string LeagueName { get; set; }
    }
}