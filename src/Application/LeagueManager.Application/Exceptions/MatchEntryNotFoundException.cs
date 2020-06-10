using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class MatchEntryNotFoundException : LeagueManagerException
    {
        public MatchEntryNotFoundException() { }
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