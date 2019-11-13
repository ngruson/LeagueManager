using System;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class TeamNotFoundException : Exception
    {
        public TeamNotFoundException(string name)
            : base($"Team \"{name}\" not found.")
        {
        }
    }
}