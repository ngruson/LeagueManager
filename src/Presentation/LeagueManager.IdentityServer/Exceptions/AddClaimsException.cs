using System;

namespace LeagueManager.IdentityServer.Exceptions
{
    [Serializable]
    public class AddClaimsException : Exception
    {
        public AddClaimsException(string message) : base(message)
        {
        }
    }
}