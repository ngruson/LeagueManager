using LeagueManager.Domain.Competition;

namespace LeagueManager.Domain.Round
{
    public interface IRound
    {
        int Id { get; set; }
        string Name { get; set; }
        //ICompetition Competition { get; set; }
    }
}