using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.LeagueTable;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable
{
    public class TableDto : IMapFrom<TeamLeagueTable>
    {
        public List<TableItemDto> Items { get; set; }
    }
}