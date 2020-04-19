using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class TeamNotFoundException : LeagueManagerException
    {
        public TeamNotFoundException(string name)
            : base($"Team \"{name}\" not found.")
        {
        }

        protected TeamNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}