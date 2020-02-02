using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    public class CountryNotFoundException : Exception
    {
        public CountryNotFoundException(string name) : base($"Country \"{name}\" not found.")
        {
        }

        protected CountryNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}