using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.Countries.Queries.GetCountries
{
    public class GetCountriesQuery : IRequest<IEnumerable<CountryDto>>
    {
    }
}