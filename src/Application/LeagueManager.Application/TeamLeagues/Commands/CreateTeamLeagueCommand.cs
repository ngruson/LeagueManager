using LeagueManager.Application.TeamLeagues.Dto;
using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Commands
{
    public class CreateTeamLeagueCommand : IRequest<TeamLeagueDto>
    {
        public string Sports { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public byte[] Logo { get; set; }
        public List<string> Teams { get; set; }
    }
}