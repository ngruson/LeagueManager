using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueManager.Domain.Entities
{
    public class LeagueTeam
    {
        public int Id { get; set; }
        public Team Team { get; set; }
    }
}