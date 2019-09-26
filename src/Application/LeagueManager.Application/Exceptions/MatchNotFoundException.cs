using System;

namespace LeagueManager.Application.Exceptions
{
    public class MatchNotFoundException : Exception
    {
        public MatchNotFoundException(Guid guid)
            : base($"Match \"{guid}\" not found.")
        {
        }
    }
}