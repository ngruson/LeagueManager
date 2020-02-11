﻿using System;
using System.Runtime.Serialization;

namespace LeagueManager.Application.Exceptions
{
    [Serializable]
    public class PlayerNotFoundException : Exception
    {
        public PlayerNotFoundException(string name) : base($"Player \"{name}\" not found.")
        {
        }

        protected PlayerNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}