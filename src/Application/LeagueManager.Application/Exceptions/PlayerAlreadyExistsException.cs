using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    public class PlayerAlreadyExistsException : Exception
    {
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