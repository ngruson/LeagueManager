using System;

namespace LeagueManager.IdentityServer.Exceptions
{
    public class AddLoginException : Exception
    {
        public AddLoginException(string message) : base(message)
        {
        }
    }
}