using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class PlayerAlreadyExistsException : LeagueManagerException
    {
        public PlayerAlreadyExistsException() { }
        public PlayerAlreadyExistsException(string name)
            : base($"Player \"{name}\" already exists.")
        {
        }

        protected PlayerAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}