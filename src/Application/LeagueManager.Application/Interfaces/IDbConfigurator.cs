using LeagueManager.Application.Config;

namespace LeagueManager.Application.Interfaces
{
    public interface IDbConfigurator
    {
        void Configure(DbConfig dbConfig);
    }
}