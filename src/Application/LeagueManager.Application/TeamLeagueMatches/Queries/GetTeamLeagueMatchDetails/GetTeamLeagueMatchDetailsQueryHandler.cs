using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Interfaces.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class GetTeamLeagueMatchDetailsQueryHandler : IRequestHandler<GetTeamLeagueMatchDetailsQuery, TeamMatchDto>
    {
        private readonly ILeagueManagerDbContext context;

        public GetTeamLeagueMatchDetailsQueryHandler(ILeagueManagerDbContext context)
            => (this.context) = (context);

        public async Task<TeamMatchDto> Handle(GetTeamLeagueMatchDetailsQuery request, CancellationToken cancellationToken)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            var teamLeague = await context.TeamLeagues
                .ProjectTo<TeamLeagueDto>(config)
                .SingleOrDefaultAsync(tl => tl.Name == request.LeagueName);

            var match = teamLeague.GetMatch<RoundDto, TeamMatchDto, ITeamMatchEntryWithDetailsDto>(request.Guid);

            if (match != null)
                return match;
            return null;
        }
    }
}