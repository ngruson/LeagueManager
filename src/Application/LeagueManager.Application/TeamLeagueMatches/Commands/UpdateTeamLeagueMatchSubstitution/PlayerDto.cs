using LeagueManager.Application.Interfaces.Dto;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution
{
    public class PlayerDto : IPlayerDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName => this.FullName();
    }
}