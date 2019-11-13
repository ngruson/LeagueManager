using System;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class MatchNotFoundException : Exception
    {
        public MatchNotFoundException(Guid guid)
            : base($"Match \"{guid}\" not found.")
        {
        }
    }
}