using LeagueManager.Application.Common.Mappings;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor
{
    public class PlayerDto : IMapFrom<Domain.Player.Player>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
    }
}