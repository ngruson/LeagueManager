using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Goals;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal
{
    public class GetTeamLeagueMatchGoalQueryHandler : IRequestHandler<GetTeamLeagueMatchGoalQuery, GoalDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;

        public GetTeamLeagueMatchGoalQueryHandler(
             ILeagueManagerDbContext context,
            IConfigurationProvider config)
                => (this.context, this.config) = (context, config);

        public async Task<GoalDto> Handle(GetTeamLeagueMatchGoalQuery request, CancellationToken cancellationToken)
        {
            var goal = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .SelectMany(l => l.Rounds.SelectMany(r =>
                        r.Matches
                        .Where(m => m.Guid == request.MatchGuid)
                        .SelectMany(m => m.MatchEntries.SelectMany(me => me.Goals))))
                .ProjectTo<GoalDto>(config)
                .SingleOrDefaultAsync(l => l.Guid == request.GoalGuid);

            if (goal == null)
                throw new GoalNotFoundException(request.GoalGuid);

            return goal;
        }
    }
}