﻿using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class CountriesNotFoundException : Exception
    {
        public CountriesNotFoundException()
            : base("Countries not found.")
        {
        }

        protected CountriesNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}