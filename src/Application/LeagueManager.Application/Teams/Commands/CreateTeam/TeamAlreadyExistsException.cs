using System;

namespace LeagueManager.Application.Teams.Commands.CreateTeam
{
    public class TeamAlreadyExistsException : Exception
    {
        public TeamAlreadyExistsException(string name)
            : base($"Team \"{name}\" already exists.")
        {
        }
    }
}