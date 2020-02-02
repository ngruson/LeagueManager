using System;

namespace LeagueManager.IdentityServer.Exceptions
{
    [Serializable]
    public class CreateRoleException : Exception
    {
        public CreateRoleException(string message) : base(message)
        {
        }
    }
}