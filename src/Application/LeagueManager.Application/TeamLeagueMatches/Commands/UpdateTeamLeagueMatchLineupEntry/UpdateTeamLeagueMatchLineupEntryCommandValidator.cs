using FluentValidation;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry
{
    public class UpdateTeamLeagueMatchLineupEntryCommandValidator : AbstractValidator<UpdateTeamLeagueMatchLineupEntryCommand>
    {
        public UpdateTeamLeagueMatchLineupEntryCommandValidator()
        {
            RuleFor(x => x.LeagueName).NotEmpty();
            RuleFor(x => x.MatchGuid).NotEmpty();
            RuleFor(x => x.TeamName).NotEmpty();
            RuleFor(x => x.LineupEntryGuid).NotEmpty();
            RuleFor(x => x.PlayerNumber).NotEmpty();
            RuleFor(x => x.PlayerName).NotEmpty();
        }   
    }
}