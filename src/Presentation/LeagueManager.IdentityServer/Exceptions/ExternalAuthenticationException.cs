using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueManager.IdentityServer.Exceptions
{
    [Serializable]
    public class ExternalAuthenticationException : Exception
    {
        public ExternalAuthenticationException() : base("External authentication error")
        {
        }
    }
}