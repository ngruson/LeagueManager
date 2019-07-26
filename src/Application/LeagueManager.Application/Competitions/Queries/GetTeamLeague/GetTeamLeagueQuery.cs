using LeagueManager.Application.Competitions.Queries.Dto;
using MediatR;

namespace LeagueManager.Application.Competitions.Queries.GetTeamLeague
{
    public class GetTeamLeagueQuery : IRequest<TeamLeagueDto>
    {
        public string Name { get; set; }
    }
}