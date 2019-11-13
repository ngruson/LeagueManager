using System;

namespace LeagueManager.IdentityServer.Exceptions
{
    [Serializable]
    public class InvalidReturnURLException : Exception
    {
        public InvalidReturnURLException() : base("Invalid return URL")
        {
        }
    }
}