using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        
        public async Task<TeamMatchDto> Handle(UpdateTeamLeagueMatchScoreCommand request, CancellationToken cancellationToken)
        {
            var league = await context.TeamLeagues
                .Include(t => t.Rounds)
                    .ThenInclude(r => r.Matches)
                        .ThenInclude(m => m.MatchEntries)
                            .ThenInclude(me => me.Score)
                .SingleOrDefaultAsync(x => x.Name == request.LeagueName, cancellationToken);

            var match = league.GetMatch(request.Guid);
            if (match == null)
                throw new MatchNotFoundException(request.Guid);

            var homeTeam = await context.Teams.SingleOrDefaultAsync(t => t.Name == request.HomeMatchEntry.Team.Name, 
                cancellationToken);
            if (homeTeam == null)
                throw new TeamNotFoundException(request.HomeMatchEntry.Team.Name);

            var awayTeam = await context.Teams.SingleOrDefaultAsync(t => t.Name == request.AwayMatchEntry.Team.Name,
                cancellationToken);
            if (awayTeam == null)
                throw new TeamNotFoundException(request.AwayMatchEntry.Team.Name);

            // Score for home team
            Domain.Match.HomeAway homeAway;
            Enum.TryParse(request.HomeMatchEntry.HomeAway.ToString(), out homeAway);
            var homeMatchEntry = match.MatchEntries.SingleOrDefault(
                me => me.HomeAway == homeAway && me.Team.Name == request.HomeMatchEntry.Team.Name);
            if (homeMatchEntry.Score == null)
                homeMatchEntry.Score = new Domain.Score.IntegerScore();
            homeMatchEntry.Score.Value = request.HomeMatchEntry.Score.Value;

            // Score for away team
            Enum.TryParse(request.AwayMatchEntry.HomeAway.ToString(), out homeAway);
            var awayMatchEntry = match.MatchEntries.SingleOrDefault(
                me => me.HomeAway == homeAway && me.Team.Name == request.AwayMatchEntry.Team.Name);
            if (awayMatchEntry.Score == null)
                awayMatchEntry.Score = new Domain.Score.IntegerScore();
            awayMatchEntry.Score.Value = request.AwayMatchEntry.Score.Value;

            // Save changes
            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<TeamMatchDto>(match);
        }
    }
}