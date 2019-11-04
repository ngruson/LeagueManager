using MediatR;

namespace LeagueManager.Application.Teams.Commands.CreateTeam
{
    public class CreateTeamCommand : IRequest
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }
}