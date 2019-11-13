using System;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class TeamAlreadyExistsException : Exception
    {
        public TeamAlreadyExistsException(string name)
            : base($"Team \"{name}\" already exists.")
        {
        }
    }
}