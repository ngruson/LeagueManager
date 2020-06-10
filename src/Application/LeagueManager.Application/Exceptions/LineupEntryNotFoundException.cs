using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class LineupEntryNotFoundException : LeagueManagerException
    {
        public LineupEntryNotFoundException() { }
        public LineupEntryNotFoundException(Guid guid)
            : base($"No lineup entry found with id \"{guid}\".")
        {
        }

        protected LineupEntryNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}