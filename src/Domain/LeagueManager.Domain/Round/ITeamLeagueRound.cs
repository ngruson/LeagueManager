using LeagueManager.Domain.Match;
using System.Collections.Generic;

namespace LeagueManager.Domain.Round
{
    public interface ITeamLeagueRound : ILeagueRound
    {
        List<TeamMatch> Matches { get; set; }
    }
}