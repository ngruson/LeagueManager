using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class MatchNotFoundException : LeagueManagerException
    {
        public MatchNotFoundException(Guid guid)
            : base($"Match \"{guid}\" not found.")
        {
        }

        protected MatchNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}