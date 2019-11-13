using System;

namespace LeagueManager.IdentityServer.Exceptions
{
    [Serializable]
    public class CreateUserException : Exception
    {
        public CreateUserException(string message) : base(message)
        {
        }
    }
}