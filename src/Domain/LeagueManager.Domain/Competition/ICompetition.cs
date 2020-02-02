using LeagueManager.Domain.Common;

namespace LeagueManager.Domain.Competition
{
    public interface ICompetition
    {
        string Name { get; set; }
        Country Country { get; set; }
        byte[] Logo { get; set; }

        void CreateRounds();
    }
}