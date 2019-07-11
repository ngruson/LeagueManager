﻿using LeagueManager.Application.Teams.Queries.GetTeams;
using LeagueManager.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel;

namespace LeagueManager.WebUI.ViewModels
{
    public class TeamLeagueViewModel
    {
        public string Name { get; set; }
        public IEnumerable<TeamDto> AllTeams { get; set; }
        
        [DisplayName("Available teams")]
        public IEnumerable<string> TeamsToAdd { get; set; }

        [DisplayName("Selected teams")]
        public IEnumerable<Team> SelectedTeams { get; set; }

        public IEnumerable<string> SelectedTeamIds { get; set; }

        public string TeamApiUrl { get; set; }
        public string CountryApiUrl { get; set; }
    }
}