using MediatR;

namespace LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor
{
    public class AddPlayerToTeamCompetitorCommand : IRequest
    {
        public string LeagueName { get; set; }
        public string TeamName { get; set; }
        public string PlayerNumber { get; set; }
        public string PlayerName{ get; set; }
    }
}