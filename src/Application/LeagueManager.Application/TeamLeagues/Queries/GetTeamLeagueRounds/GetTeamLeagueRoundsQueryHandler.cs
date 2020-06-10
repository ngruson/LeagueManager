using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds
{
    public class GetTeamLeagueRoundsQueryHandler : IRequestHandler<GetTeamLeagueRoundsQuery, GetTeamLeagueRoundsVm>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public GetTeamLeagueRoundsQueryHandler(
            ILeagueManagerDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<GetTeamLeagueRoundsVm> Handle(GetTeamLeagueRoundsQuery request, CancellationToken cancellationToken)
        {
            var teamLeague = await context.TeamLeagues
                .ProjectTo<TeamLeagueDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(l => l.Name == request.LeagueName, cancellationToken);

            if (teamLeague != null)
            {
                teamLeague.Rounds = teamLeague.Rounds
                    .OrderBy(r => r.Name.Substring(r.Name.IndexOf(' ') + 1).PadLeft(2, '0'))
                    .ToList();
                teamLeague.Rounds.ToList().ForEach(r =>
                {
                    r.Matches = r.Matches.OrderBy(m => m.StartTime).ToList();
                    r.Matches.ToList().ForEach(m =>
                    {
                        m.MatchEntries = m.MatchEntries
                            .OrderByDescending(me => me.HomeAway.ToString())
                            .ToList();
                    });
                });

                return new GetTeamLeagueRoundsVm
                {
                    Name = teamLeague.Name,
                    Rounds = teamLeague.Rounds,
                    SelectedRound = teamLeague.Rounds.ToList()[0].Name
                };
            }

            return null;
        }
    }
}