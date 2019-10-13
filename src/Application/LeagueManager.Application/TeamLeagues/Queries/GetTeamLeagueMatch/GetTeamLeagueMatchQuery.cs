using LeagueManager.Application.TeamLeagues.Queries.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueMatch
{
    public class GetTeamLeagueMatchQuery : IRequest<TeamMatchDto>
    {
        public string LeagueName { get; set; }
        public Guid Guid { get; set; }
    }
}