using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueManager.Application.Competitions.Queries.GetCompetitions
{
    public class CompetitionsListVm
    {
        public IList<CompetitionDto> Competitions { get; set; }
    }
}
