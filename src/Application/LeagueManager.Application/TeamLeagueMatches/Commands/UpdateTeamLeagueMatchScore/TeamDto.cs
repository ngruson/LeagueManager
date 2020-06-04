using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Competitor;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore
{
    public class TeamDto : IMapFrom<Team>, ITeamDto
    {
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}