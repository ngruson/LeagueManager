using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Commands
{
    public class CreateTeamLeagueCommand : IRequest
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public byte[] Logo { get; set; }
        public List<string> Teams { get; set; }
    }
}