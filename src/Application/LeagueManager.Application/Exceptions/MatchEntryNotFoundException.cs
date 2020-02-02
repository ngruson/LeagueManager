﻿using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class MatchEntryNotFoundException : Exception
    {
        public MatchEntryNotFoundException(string team)
            : base($"No match entry found for team \"{team}\".")
        {
        }

        protected MatchEntryNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}