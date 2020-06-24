using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class LineupNotFoundException : LeagueManagerException
    {
        public LineupNotFoundException(string teamName) : base($"No lineup found for team '{teamName}'") 
        { 
        }

        protected LineupNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}