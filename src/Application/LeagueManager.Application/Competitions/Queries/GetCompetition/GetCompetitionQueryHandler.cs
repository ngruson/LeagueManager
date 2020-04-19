using Microsoft.EntityFrameworkCore;
using LeagueManager.Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using LeagueManager.Domain.Competition;
using AutoMapper;
using LeagueManager.Application.Competitions.Queries.Dto;
using Microsoft.Extensions.Logging;

namespace LeagueManager.Application.Competitions.Queries.GetCompetition
{
    public class GetCompetitionQueryHandler : IRequestHandler<GetCompetitionQuery, CompetitionDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<GetCompetitionQueryHandler> logger;

        public GetCompetitionQueryHandler(
            ILeagueManagerDbContext context,
            IMapper mapper,
            ILogger<GetCompetitionQueryHandler> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<CompetitionDto> Handle(GetCompetitionQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting team leagues with name '{Name}'", request.Name);
            var list = new List<TeamLeague>();
            var teamLeague = await context.TeamLeagues
                .Include(t => t.Sports)
                .Include(t => t.Country)
                .Include(t => t.Competitors)
                    .ThenInclude(c => c.Team)
                .SingleOrDefaultAsync(l => l.Name == request.Name, cancellationToken);

            if (teamLeague != null)
            {
                logger.LogInformation($"Team league found with name = '{request.Name}' ");
                list.Add(teamLeague);
            }
            else
                logger.LogInformation($"No team league found with name = '{request.Name}' ");


            var competition = list.SingleOrDefault(c => c.Name == request.Name);

            if (competition != null)
            {
                logger.LogInformation($"Returning competition with name = '{request.Name}' ");
                return mapper.Map<CompetitionDto>(competition);
            }

            logger.LogInformation($"Returning null");
            return null;
        }
    }
}