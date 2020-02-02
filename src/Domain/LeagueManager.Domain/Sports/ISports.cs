namespace LeagueManager.Domain.Sports
{
    public interface ISports<TOptions>
    {
        string Name { get; set; }
        TOptions Options { get; set; }
    }
}