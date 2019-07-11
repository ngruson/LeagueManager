using LeagueManager.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.Leagues.Commands
{
    public class CreateTeamLeagueCommand : IRequest
    {
        public string Name { get; set; }
        public List<LeagueTeam> Teams { get; set; }
    }
}