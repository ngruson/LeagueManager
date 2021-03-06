﻿using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Competitor;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor
{
    public class CompetitorPlayerDto : IMapFrom<TeamCompetitorPlayer>
    {
        public string Number { get; set; }
        public PlayerDto Player { get; set; }
    }
}