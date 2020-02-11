using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Round;
using LeagueManager.Domain.Sports;

namespace LeagueManager.Domain.Competition
{
    public interface ITeamLeague : ILeague<TeamSports, TeamSportsOptions, TeamCompetitor, TeamLeagueRound>
    {
    }
}