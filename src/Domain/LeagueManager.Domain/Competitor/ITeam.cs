using LeagueManager.Domain.Common;

namespace LeagueManager.Domain.Competitor
{
    public interface ITeam
    {
        string Name { get; set; }
        Country Country { get; set; }
        byte[] Logo { get; set; }
    }
}