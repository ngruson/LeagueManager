using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Queries.Dto
{
    public class TeamLeagueDto
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public byte[] Logo { get; set; }

        public IEnumerable<string> Competitors { get; set; }
        public IEnumerable<TeamLeagueRoundDto> Rounds { get; set; }
        public TeamLeagueTableDto Table { get; set; }
    }
}