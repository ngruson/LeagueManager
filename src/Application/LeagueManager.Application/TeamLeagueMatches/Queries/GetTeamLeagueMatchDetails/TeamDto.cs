using LeagueManager.Application.Interfaces.Dto;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class TeamDto : ITeamDto
    {
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}