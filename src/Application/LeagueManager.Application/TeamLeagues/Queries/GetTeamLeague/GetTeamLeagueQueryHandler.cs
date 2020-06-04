using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeague
{
    public class GetTeamLeagueQueryHandler : IRequestHandler<GetTeamLeagueQuery, GetTeamLeagueVm>
    {
        private readonly IMediator mediator;

        public GetTeamLeagueQueryHandler(IMediator mediator) => (this.mediator) = (mediator);

        public async Task<GetTeamLeagueVm> Handle(GetTeamLeagueQuery request, CancellationToken cancellationToken)
        {
            return new GetTeamLeagueVm
            {
                LeagueName = request.LeagueName,
                Table = await mediator.Send(new GetTeamLeagueTableQuery { LeagueName = request.LeagueName }),
                Rounds = await mediator.Send(new GetTeamLeagueRoundsQuery { LeagueName = request.LeagueName })
            };
        }
    }
}