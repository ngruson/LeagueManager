using FluentAssertions;
using Xunit;
using LeagueManager.Domain.UnitTests.TestData;
using LeagueManager.Domain.Sports;

namespace LeagueManager.Domain.UnitTests
{
    public class TeamLeagueRoundsUnitTests
    {
        

        [Fact]
        public void Given_TeamsAreAdded_When_CreateRounds_Then_CreateAllRounds()
        {
            //Arrange
            var sports = new TeamSports { Options = new TeamSportsOptions { AmountOfPlayers = 11 } };
            var league = new TeamLeagueBuilder()
                .WithSports(sports)
                .WithCompetitors(new TeamsBuilder().Build())
                .Build();

            //Act
            league.CreateRounds();

            //Assert
            league.Rounds.Count.Should().Be(6);
            league.Rounds.ForEach(r =>
                r.Matches.Count.Should().Be(2)
            );
        }
    }
}