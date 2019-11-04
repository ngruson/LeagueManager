using System;

namespace LeagueManager.Application.Exceptions
{
    public class TeamNotFoundException : Exception
    {
        public TeamNotFoundException(string name)
            : base($"Team \"{name}\" not found.")
        {
        }
    }
}