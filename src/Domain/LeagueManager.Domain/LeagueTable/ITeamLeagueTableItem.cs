using LeagueManager.Domain.Competitor;

namespace LeagueManager.Domain.LeagueTable
{
    public interface ITeamLeagueTableItem : ILeagueTableItem
    {
        Team Team { get; set; }
    }
}