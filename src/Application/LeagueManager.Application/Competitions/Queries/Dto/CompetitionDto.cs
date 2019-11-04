using System.Collections.Generic;

namespace LeagueManager.Application.Competitions.Queries.Dto
{
    public class CompetitionDto
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public byte[] Logo { get; set; }
        public List<string> Competitors { get; set; }
    }
}