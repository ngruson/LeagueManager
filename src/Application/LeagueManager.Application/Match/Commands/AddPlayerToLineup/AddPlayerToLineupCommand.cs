using MediatR;
using System;

namespace LeagueManager.Application.Match.Commands.AddPlayerToLineup
{
    public class AddPlayerToLineupCommand : IRequest
    {
        public string LeagueName { get; set; }
        public Guid Guid { get; set; }
        public string Team { get; set; }
        public string Player { get; set; }
        public string Number { get; set; }
    }
}