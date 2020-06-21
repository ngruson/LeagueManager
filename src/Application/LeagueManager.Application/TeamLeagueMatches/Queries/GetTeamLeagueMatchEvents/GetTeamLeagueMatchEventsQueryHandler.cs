using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Interfaces.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class GetTeamLeagueMatchEventsQueryHandler : IRequestHandler<GetTeamLeagueMatchEventsQuery, MatchEventsDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly ILogger<GetTeamLeagueMatchEventsQueryHandler> logger;

        public GetTeamLeagueMatchEventsQueryHandler(
                ILeagueManagerDbContext context,
                ILogger<GetTeamLeagueMatchEventsQueryHandler> logger
            ) => (this.context, this.logger) = (context, logger);

        public async Task<MatchEventsDto> Handle(GetTeamLeagueMatchEventsQuery request, CancellationToken cancellationToken)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            string methodName = nameof(Handle);
            logger.LogInformation($"{methodName}: Request received");

            logger.LogInformation($"{methodName}: Retrieving match events");
            var teamLeague = await context.TeamLeagues
                .ProjectTo<TeamLeagueDto>(config)
                .SingleOrDefaultAsync(tl => tl.Name == request.LeagueName);

            if (teamLeague == null)
            {
                var ex = new TeamLeagueNotFoundException(request.LeagueName);
                logger.LogError(ex, $"League '{request.LeagueName}' was not found");
                throw ex;
            }

            var match = teamLeague.GetMatch<RoundDto, TeamMatchDto, ITeamMatchEntryEventsDto>(request.MatchGuid);
            var matchEntry = match.MatchEntries.SingleOrDefault(me => me.Team.Name == request.TeamName);

            logger.LogInformation($"{methodName}: Returning match events");
            return new MatchEventsDto
            {
                Goals = matchEntry.Goals,
                Substitutions = matchEntry.Substitutions
            };
        }
    }
}