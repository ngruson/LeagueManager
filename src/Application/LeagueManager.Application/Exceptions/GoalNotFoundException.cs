using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class GoalNotFoundException : LeagueManagerException
    {
        public GoalNotFoundException() { }
        public GoalNotFoundException(Guid guid)
            : base($"No goal found with id \"{guid}\".")
        {
        }

        protected GoalNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}