using LeagueManager.Application.TeamLeagueMatches.Dto;
using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch
{
    public class GetTeamLeagueMatchQuery : IRequest<TeamMatchDto>
    {
        public string LeagueName { get; set; }
        public Guid Guid { get; set; }
    }
}