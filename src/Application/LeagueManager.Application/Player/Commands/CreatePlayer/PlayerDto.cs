using LeagueManager.Application.Common.Mappings;

namespace LeagueManager.Application.Player.Commands.CreatePlayer
{
    public class PlayerDto : IMapFrom<Domain.Player.Player>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
    }
}