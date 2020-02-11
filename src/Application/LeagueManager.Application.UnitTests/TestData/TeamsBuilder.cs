using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competitor;
using System.Collections.Generic;

namespace LeagueManager.Application.UnitTests.TestData
{
    public class TeamsBuilder
    {
        public List<Team> Build()
        {
            var country = new Country { Code = "EN", Name = "England" };
            return new List<Team>
            {
                new Team {  Name = "Tottenham Hotspur", Country = country },
                new Team {  Name = "Manchester City", Country = country },
                new Team {  Name = "Liverpool", Country = country },
                new Team {  Name =  "Chelsea", Country = country }
            };
        }
    }
}