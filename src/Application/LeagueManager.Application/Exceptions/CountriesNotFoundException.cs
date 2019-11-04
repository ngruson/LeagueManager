using System;

namespace LeagueManager.Application.Exceptions
{
    public class CountriesNotFoundException : Exception
    {
        public CountriesNotFoundException()
            : base("Countries not found.")
        {
        }
    }
}