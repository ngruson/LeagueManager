using System;

namespace LeagueManager.Application.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException()
            : base("Configuration failed!")
        {
        }
    }
}