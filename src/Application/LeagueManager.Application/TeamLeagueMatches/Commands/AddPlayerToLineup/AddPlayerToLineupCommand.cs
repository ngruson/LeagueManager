using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.AddPlayerToLineup
{
    public class AddPlayerToLineupCommand : IRequest
    {
        public string LeagueName { get; set; }
        public Guid MatchGuid { get; set; }
        public string TeamName { get; set; }
        public PlayerDto Player { get; set; }
        public string Number { get; set; }
    }
}