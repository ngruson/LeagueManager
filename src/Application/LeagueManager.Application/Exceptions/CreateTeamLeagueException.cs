using System;

namespace LeagueManager.Application.Exceptions
{
    public class CreateTeamLeagueException : Exception
    {
        public CreateTeamLeagueException(string name)
            : base($"Team league \"{name}\" could not be created.")
        {
        }
    }
}