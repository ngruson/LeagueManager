namespace LeagueManager.Application.Interfaces.Dto
{
    public static class PlayerExtensions
    {
        public static string FullName(this IPlayerDto player)
        {
            string fullName = player.FirstName;
            if (!string.IsNullOrEmpty(player.MiddleName))
                fullName += $" {player.MiddleName}";
            fullName += $" {player.LastName}";
            return fullName;
        }
    }
}