namespace LeagueManager.Domain.Score
{
    public class IntegerScore : IScore
    {
        public int Id { get; set; }
        public int? Value { get; set; }
        
    }
}