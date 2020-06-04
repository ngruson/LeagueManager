using System.Collections.Generic;

namespace LeagueManager.Application.Interfaces.Dto
{
    public interface IRoundDto<T>
    {
        string Name { get; set; }
        List<T> Matches { get; set; }
    }
}