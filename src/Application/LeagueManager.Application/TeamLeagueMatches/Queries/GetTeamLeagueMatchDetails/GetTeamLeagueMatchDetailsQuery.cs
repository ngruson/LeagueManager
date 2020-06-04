using LeagueManager.Application.Interfaces.Dto;
using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class GetTeamLeagueMatchDetailsQuery : IRequest<TeamMatchDto>
    {
        public string LeagueName { get; set; }
        public Guid Guid { get; set; }
    }
}