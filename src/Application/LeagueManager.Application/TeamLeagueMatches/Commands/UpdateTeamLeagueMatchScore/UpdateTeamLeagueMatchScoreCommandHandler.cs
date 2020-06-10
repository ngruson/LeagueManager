using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore
{
    public class UpdateTeamLeagueMatchScoreCommandHandler : IRequestHandler<UpdateTeamLeagueMatchScoreCommand, TeamMatchDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public UpdateTeamLeagueMatchScoreCommandHandler(
            ILeagueManagerDbContext context,
            IMapper mapper) => (this.context, this.mapper) = (context, mapper);
        
        public async Task<TeamMatchDto> Handle(UpdateTeamLeagueMatchScoreCommand request, CancellationToken cancellationToken)
        {
            var league = await context.TeamLeagues
                .SingleOrDefaultAsync(x => x.Name == request.LeagueName, cancellationToken);

            if (league == null)
                throw new TeamLeagueNotFoundException(request.LeagueName);

            var match = league.GetMatch(request.Guid);
            if (match == null)
                throw new MatchNotFoundException(request.Guid);

            foreach (var matchEntry in request.MatchEntries)
            {
                var me = match.MatchEntries.SingleOrDefault(me => me.Team.Name == matchEntry.Team);
                if (me != null)
                {
                    if (me.Score == null)
                        me.Score = new Domain.Score.IntegerScore();
                    me.Score.Value = matchEntry.Score;
                }
                else
                    throw new MatchEntryNotFoundException(matchEntry.Team);
            }

            // Save changes
            await context.SaveChangesAsync(cancellationToken);

            TeamMatchDto dto = new TeamMatchDto();
            mapper.Map(match, dto, typeof(Domain.Match.TeamLeagueMatch), typeof(TeamMatchDto));
            return dto;
        }
    }
}