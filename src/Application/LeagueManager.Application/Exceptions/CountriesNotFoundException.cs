using System;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class CountriesNotFoundException : Exception
    {
        public CountriesNotFoundException()
            : base("Countries not found.")
        {
        }
    }
}