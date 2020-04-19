using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    public abstract class LeagueManagerException : Exception
    {
        public LeagueManagerException(string message) : base(message)
        {
        }

        protected LeagueManagerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}