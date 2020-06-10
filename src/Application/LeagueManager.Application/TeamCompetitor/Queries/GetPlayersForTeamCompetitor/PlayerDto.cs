using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor
{
    public class PlayerDto : IMapFrom<Domain.Player.Player>, IPlayerDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName => this.FullName();
    }
}