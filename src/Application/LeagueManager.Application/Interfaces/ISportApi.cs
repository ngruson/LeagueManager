using LeagueManager.Application.Sports.Queries.GetTeamSports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeagueManager.Application.Interfaces
{
    public interface ISportApi : IConfigurationApi
    {
        Task<IEnumerable<TeamSportDto>> GetTeamSports();
    }
}