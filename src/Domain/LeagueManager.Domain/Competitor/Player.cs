using System;

namespace LeagueManager.Domain.Competitor
{
    public class Player : IPlayer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}