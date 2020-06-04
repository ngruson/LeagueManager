using System.Collections.Generic;

namespace LeagueManager.Application.Interfaces.Dto
{
    public interface ITeamLeagueDto<TRound>
    {
        List<TRound> Rounds { get; set; }        
    }
}