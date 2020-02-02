using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class CompetitionAlreadyExistsException : Exception
    {
        public CompetitionAlreadyExistsException(string name)
            : base($"Competition \"{name}\" already exists.")
        {
        }

        protected CompetitionAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}