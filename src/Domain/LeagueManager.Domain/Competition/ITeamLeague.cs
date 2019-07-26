using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Round;
using System.Collections.Generic;

namespace LeagueManager.Domain.Competition
{
    public interface ITeamLeague : ILeague<TeamCompetitor, TeamLeagueRound>
    {
    }
}