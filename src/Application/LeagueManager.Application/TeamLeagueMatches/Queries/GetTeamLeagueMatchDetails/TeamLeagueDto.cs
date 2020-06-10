﻿using LeagueManager.Application.Interfaces.Dto;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class TeamLeagueDto : ITeamLeagueDto<RoundDto>
    {
        public string Name { get; set; }        
        public List<RoundDto> Rounds { get; set; }
    }
}