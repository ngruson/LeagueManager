using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class SubstitutionNotFoundException : LeagueManagerException
    {
        public SubstitutionNotFoundException() { }
        public SubstitutionNotFoundException(Guid guid)
            : base($"No goal found with id \"{guid}\".")
        {
        }

        protected SubstitutionNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}