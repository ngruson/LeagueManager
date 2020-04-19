using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class TeamAlreadyExistsException : LeagueManagerException
    {
        public TeamAlreadyExistsException(string name)
            : base($"Team \"{name}\" already exists.")
        {
        }

        protected TeamAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}