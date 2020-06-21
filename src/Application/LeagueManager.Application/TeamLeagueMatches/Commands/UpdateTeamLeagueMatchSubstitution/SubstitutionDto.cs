using AutoMapper;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Match;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution
{
    public class SubstitutionDto : ISubstitutionDto
    {
        public Guid Guid { get; set; }
        public string TeamMatchEntryTeamName { get; set; }
        public string Minute { get; set; }
        public IPlayerDto PlayerOut { get; set; }
        public IPlayerDto PlayerIn { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TeamMatchEntrySubstitution, SubstitutionDto>();
        }
    }
}