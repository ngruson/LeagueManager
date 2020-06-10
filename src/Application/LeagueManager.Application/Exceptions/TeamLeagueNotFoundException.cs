using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class TeamLeagueNotFoundException : LeagueManagerException
    {
        public TeamLeagueNotFoundException() { }
        public TeamLeagueNotFoundException(string name)
            : base($"Team league \"{name}\" not found.")
        {
        }

        protected TeamLeagueNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}