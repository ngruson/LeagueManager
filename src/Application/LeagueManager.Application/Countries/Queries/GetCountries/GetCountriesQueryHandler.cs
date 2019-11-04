using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            var countries = await context.Countries.OrderBy(t => t.Name).ToListAsync();

            if (countries != null)
                return countries.Select(t => new CountryDto { Code = t.Code, Name = t.Name });

            return null;
        }
    }
}