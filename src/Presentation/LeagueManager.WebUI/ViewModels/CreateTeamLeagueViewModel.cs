using LeagueManager.Application.Countries.Queries.GetCountries;
using LeagueManager.Application.Teams.Queries.GetTeams;
using LeagueManager.Application.Sports.Queries.GetTeamSports;
using LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;

namespace LeagueManager.WebUI.ViewModels
{
    public class CreateTeamLeagueViewModel : CreateTeamLeagueCommand
    {
        
        public IFormFile LogoFormFile { get; set; }
        
        public IEnumerable<TeamSportDto> TeamSports { get; set; }
        public IEnumerable<TeamDto> AllTeams { get; set; }
        public IEnumerable<CountryDto> Countries { get; set; }

        [DisplayName("Available teams")]
        public IEnumerable<string> TeamsToAdd { get; set; }

        public IEnumerable<string> SelectedTeamIds { get; set; } = new List<string>();
    }
}