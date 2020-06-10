using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague
{
    public class CreateTeamLeagueCommand : IRequest<TeamLeagueDto>
    {
        public string Sports { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public byte[] Logo { get; set; }

        public List<string> SelectedTeams { get; set; } = new List<string>();
    }
}