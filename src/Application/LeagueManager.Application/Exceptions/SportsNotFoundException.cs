using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class SportsNotFoundException : Exception
    {
        public SportsNotFoundException(string name) : base("Sports \"{name}\" not found")
        {
        }

        protected SportsNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}