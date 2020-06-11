using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class CreateTeamLeagueException : LeagueManagerException
    {
        public CreateTeamLeagueException() { }
        public CreateTeamLeagueException(string name)
            : base($"Team league \"{name}\" could not be created.")
        {
        }

        protected CreateTeamLeagueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}