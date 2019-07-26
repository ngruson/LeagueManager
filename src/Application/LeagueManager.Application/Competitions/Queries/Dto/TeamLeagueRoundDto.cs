using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueManager.Application.Competitions.Queries.Dto
{
    public class TeamLeagueRoundDto
    {
        public string Name { get; set; }
        public IEnumerable<TeamMatchDto> Matches { get; set; }
    }
}