using System;

namespace LeagueManager.Application.Exceptions
{
    public class CompetitionAlreadyExistsException : Exception
    {
        public CompetitionAlreadyExistsException(string name)
            : base($"Competition \"{name}\" already exists.")
        {
        }
    }
}