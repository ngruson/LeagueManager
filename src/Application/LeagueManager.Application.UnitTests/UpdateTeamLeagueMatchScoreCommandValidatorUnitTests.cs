using FluentValidation.TestHelper;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using System;
using System.Collections.Generic;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class UpdateTeamLeagueMatchScoreCommandValidatorUnitTests
    {
        [Fact]
        public void When_LeagueNameIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchScoreCommand
            {
                LeagueName = "",
                MatchEntries = new List<TeamMatchEntryRequestDto>()
                {
                    new TeamMatchEntryRequestDto()
                }
            };

            var validator = new UpdateTeamLeagueMatchScoreCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.LeagueName, command);
        }

        [Fact]
        public void When_GuidIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchScoreCommand
            {
                LeagueName = "Premier League",
                Guid = Guid.Empty,
                MatchEntries = new List<TeamMatchEntryRequestDto>()
                {
                    new TeamMatchEntryRequestDto()
                }
            };

            var validator = new UpdateTeamLeagueMatchScoreCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.Guid, command);
        }

        [Fact]
        public void When_MatchEntriesIsEmpty_Then_ValidationError()
        {
            var command = new UpdateTeamLeagueMatchScoreCommand
            {
                LeagueName = "Premier League",
                Guid = Guid.NewGuid(),
                MatchEntries = new List<TeamMatchEntryRequestDto>() {  null }
            };

            var validator = new UpdateTeamLeagueMatchScoreCommandValidator();
            validator.ShouldHaveValidationErrorFor(x => x.MatchEntries, command);
        }

        [Fact]
        public void When_CommandIsOk_Then_NoValidationErrors()
        {
            var command = new UpdateTeamLeagueMatchScoreCommand
            {
                LeagueName = "Premier League",
                Guid = Guid.NewGuid(),
                MatchEntries = new List<TeamMatchEntryRequestDto>()
                {
                    new TeamMatchEntryRequestDto()
                }
            };

            var validator = new UpdateTeamLeagueMatchScoreCommandValidator();
            validator.ShouldNotHaveValidationErrorFor(x => x.LeagueName, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.Guid, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.MatchEntries, command);
        }
    }
}