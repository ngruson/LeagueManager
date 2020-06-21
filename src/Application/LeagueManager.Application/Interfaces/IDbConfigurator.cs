using LeagueManager.Application.Config;
using System.Threading.Tasks;

namespace LeagueManager.Application.Interfaces
{
    public interface IDbConfigurator
    {
        Task Configure(DbConfig dbConfig);
    }
}