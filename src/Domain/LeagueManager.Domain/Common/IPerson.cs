using System;

namespace LeagueManager.Domain.Common
{
    public interface IPerson
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime? BirthDate { get; set; }
    }
}