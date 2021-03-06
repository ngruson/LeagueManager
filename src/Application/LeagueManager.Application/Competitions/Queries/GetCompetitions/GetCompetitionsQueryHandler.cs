﻿using LeagueManager.Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using LeagueManager.Domain.Competition;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace LeagueManager.Application.Competitions.Queries.GetCompetitions
{
    public class GetCompetitionsQueryHandler : IRequestHandler<GetCompetitionsQuery, IEnumerable<CompetitionDto>>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public GetCompetitionsQueryHandler(
            ILeagueManagerDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        private async Task<List<CompetitionDto>> GetTeamLeagues(string country)
        {
            IQueryable<TeamLeague> query = context.TeamLeagues
                .Include(c => c.Country)
                .Include(t => t.Competitors)
                    .ThenInclude(c => c.Team);

            if (!string.IsNullOrEmpty(country))
                query = query.Where(l => l.Country != null && l.Country.Name == country);

            var leagues = await query
                .ProjectTo<CompetitionDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            return leagues;
                
        }

        public async Task<IEnumerable<CompetitionDto>> Handle(GetCompetitionsQuery request, CancellationToken cancellationToken)
        {
            var list = await GetTeamLeagues(request.Country);
            return list;
        }
    }
}