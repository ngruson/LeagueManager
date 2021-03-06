﻿using MediatR;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor
{
    public class GetPlayersForTeamCompetitorQuery : IRequest<IEnumerable<CompetitorPlayerDto>>
    {
        public string LeagueName { get; set; }
        public string TeamName { get; set; }
    }
}