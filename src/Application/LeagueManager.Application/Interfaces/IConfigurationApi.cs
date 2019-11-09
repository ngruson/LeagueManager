using LeagueManager.Application.Config;
using System.Threading.Tasks;

namespace LeagueManager.Application.Interfaces
{
    public interface IConfigurationApi
    {
        Task<bool> Configure(DbConfig dbConfig, string accessToken);
    }
}