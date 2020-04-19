using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class TeamsNotFoundException : LeagueManagerException
    {
        public TeamsNotFoundException()
            : base($"Teams could not be found.")
        {
        }

        protected TeamsNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}