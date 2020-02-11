using System.Collections.Generic;

namespace LeagueManager.Application.UnitTests.TestData
{
    public class PlayerBuilder
    {
        public List<Domain.Player.Player> Build()
        {
            return new List<Domain.Player.Player>
            {
                new Domain.Player.Player
                {
                    FirstName =  "John",
                    LastName = "Doe"
                },
                new Domain.Player.Player
                {
                    FirstName =  "Jane",
                    LastName = "Doe"
                }
            };
        }
    }
}