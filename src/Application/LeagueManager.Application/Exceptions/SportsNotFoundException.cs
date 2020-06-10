using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class SportsNotFoundException : LeagueManagerException
    {
        public SportsNotFoundException() { }
        public SportsNotFoundException(string name) : base("Sports \"{name}\" not found")
        {
        }

        protected SportsNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}