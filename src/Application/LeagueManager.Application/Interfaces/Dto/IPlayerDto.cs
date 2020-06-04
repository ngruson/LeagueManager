namespace LeagueManager.Application.Interfaces.Dto
{
    public interface IPlayerDto
    {
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string FullName { get; }
    }
}