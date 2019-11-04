﻿using LeagueManager.Application.Teams.Queries.GetTeams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeagueManager.Application.Interfaces
{
    public interface ITeamApi
    {
        Task<IEnumerable<TeamDto>> GetTeams();
    }
}