using LeagueManager.Application.Countries.Queries.GetCountries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeagueManager.Application.Interfaces
{
    public interface ICountryApi : IConfigurationApi
    {
        Task<IEnumerable<CountryDto>> GetCountries();
    }
}