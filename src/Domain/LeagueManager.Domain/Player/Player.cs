using System;

namespace LeagueManager.Domain.Player
{
    public class Player : IPlayer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                string returnValue = FirstName;
                if (!string.IsNullOrEmpty(MiddleName))
                    returnValue += $" {MiddleName}";
                returnValue += $" {LastName}";
                return returnValue;
            }
        }
        public DateTime? BirthDate { get; set; }
    }
}