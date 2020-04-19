using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    public class CreateTeamLeagueException : LeagueManagerException
    {
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