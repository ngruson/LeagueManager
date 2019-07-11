using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.Countries.Queries.GetCountries
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IEnumerable<CountryDto>>
    {
        private readonly ILeagueManagerDbContext context;

        public GetCountriesQueryHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            List<Country> countries = null;
            await Task.Run(() =>
            {
                countries = context.Countries.OrderBy(t => t.Name).ToList();
            });

            if (countries != null)
                return countries.Select(t => new CountryDto { Code = t.Code, Name = t.Name });

            return null;
        }
    }
}