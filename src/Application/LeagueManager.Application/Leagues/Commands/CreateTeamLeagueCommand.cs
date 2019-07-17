using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.Leagues.Commands
{
    public class CreateTeamLeagueCommand : IRequest
    {
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public List<string> Teams { get; set; }
    }
}