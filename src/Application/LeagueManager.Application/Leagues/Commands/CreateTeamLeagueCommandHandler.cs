using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.Leagues.Commands
{
    public class CreateTeamLeagueCommandHandler : IRequestHandler<CreateTeamLeagueCommand, Unit>
    {
        private readonly ILeagueManagerDbContext context;

        public CreateTeamLeagueCommandHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(CreateTeamLeagueCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}