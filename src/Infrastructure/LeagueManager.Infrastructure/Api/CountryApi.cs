using LeagueManager.Application.Countries.Queries.GetCountries;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Infrastructure.HttpHelpers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeagueManager.Infrastructure.Api
{
    public class CountryApi : ICountryApi
    {
        private readonly IHttpRequestFactory httpRequestFactory;
        private readonly ApiSettings apiSettings;

        public CountryApi(IHttpRequestFactory httpRequestFactory,
            IOptions<ApiSettings> options)
        {
            this.httpRequestFactory = httpRequestFactory;
            this.apiSettings = options.Value;
        }

        public async Task<IEnumerable<CountryDto>> GetCountries()
        {
            var response = await httpRequestFactory.Get(apiSettings.CountryApiUrl);
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<IEnumerable<CountryDto>>();
            throw new CountriesNotFoundException();
        }
    }
}