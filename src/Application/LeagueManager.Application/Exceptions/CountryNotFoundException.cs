﻿using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class CountryNotFoundException : LeagueManagerException
    {
        public CountryNotFoundException() { }

        public CountryNotFoundException(string name) : base($"Country \"{name}\" not found.")
        {
        }

        protected CountryNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}