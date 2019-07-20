using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Points;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeagueManager.Domain.Match
{
    public class TeamMatch : ITeamMatch
    {
        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        public List<TeamMatchEntry> MatchEntries { get; set; } = new List<TeamMatchEntry>();

        public Team Winner
        {
            get
            {
                var home = MatchEntries.SingleOrDefault(me => 
                    me.HomeAway == HomeAway.Home);
                var away = MatchEntries.SingleOrDefault(me =>
                    me.HomeAway == HomeAway.Away);

                if (home.Score.Value > away.Score.Value)
                    return home.Team;
                else if (home.Score.Value < away.Score.Value)
                    return away.Team;

                return null;
            }
        }

        public Team Loser
        {
            get
            {
                var home = MatchEntries.SingleOrDefault(me =>
                    me.HomeAway == HomeAway.Home);
                var away = MatchEntries.SingleOrDefault(me =>
                    me.HomeAway == HomeAway.Away);

                if (home.Score.Value < away.Score.Value)
                    return home.Team;
                else if (home.Score.Value > away.Score.Value)
                    return away.Team;

                return null;
            }
        }

        public bool IsDraw
        {
            get
            {
                var home = MatchEntries.SingleOrDefault(me =>
                    me.HomeAway == HomeAway.Home);
                var away = MatchEntries.SingleOrDefault(me =>
                    me.HomeAway == HomeAway.Away);

                if ((home.Score.Value.HasValue) && (away.Score.Value.HasValue))
                {
                    return home.Score.Value == away.Score.Value;
                }

                return false;
            }
        }

        public int GetGoalsFor(Team team)
        {
            var meTeam = MatchEntries.SingleOrDefault(me => me.Team == team);
            return meTeam?.Score?.Value ?? 0;
        }

        public int GetGoalsAgainst(Team team)
        {
            var meOpponent = MatchEntries.SingleOrDefault(me => me.Team != team);
            return meOpponent?.Score?.Value ?? 0;
        }

        public int GetPointsFor(Team team, PointSystem pointSystem)
        {
            var meTeam = MatchEntries.SingleOrDefault(me => me.Team == team);

            if ((meTeam == null) || (pointSystem == null))
                return 0;

            if (meTeam.Team == Winner)
                return pointSystem.Win;
            else if (meTeam.Team == Loser)
                return pointSystem.Lost;
            else if (IsDraw)
                return pointSystem.Draw;

            return 0;
        }
    }
}