using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class UpdateTeamLeagueMatchUnitTests
    {
        [Fact]
        public async void Given_MatchDoesExist_When_UpdateTeamLeagueMatch_Then_ReturnOk()
        {
            //Arrange
            var leagueName = "Premier League";
            var homeTeam = "Tottenham Hotspur";
            var awayTeam = "Chelsea";
            var startTime = new DateTime(2020, 01, 01, 20, 15, 0);

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new TeamMatchDto
                {
                    TeamLeague = leagueName,
                    StartTime = startTime,
                    MatchEntries = new List<TeamMatchEntryDto>
                    {
                        new TeamMatchEntryDto
                        {
                            HomeAway = HomeAway.Home,
                            Team = new TeamDto { Name = homeTeam }
                        },
                        new TeamMatchEntryDto
                        {
                            HomeAway = HomeAway.Away,
                            Team = new TeamDto { Name = awayTeam }
                        }
                    }        
                });
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateTeamLeagueMatchDto
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                StartTime = startTime
            };

            //Act
            var result = await controller.UpdateTeamLeagueMatch("TeamLeague", Guid.NewGuid().ToString(), dto);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var match = okResult.Value.Should().BeAssignableTo<TeamMatchDto>().Subject;
            match.TeamLeague.Should().Be(leagueName);
            match.MatchEntries.Count.Should().Be(2);
            match.MatchEntries.Single(me => me.HomeAway == HomeAway.Home)
                .Team.Name.Should().Be(homeTeam);
            match.MatchEntries.Single(me => me.HomeAway == HomeAway.Away)
                .Team.Name.Should().Be(awayTeam);
            match.StartTime.Should().Be(startTime);
        }

        [Fact]
        public async void Given_MatchDoesNotExist_When_UpdateTeamLeagueMatch_Then_ThrowMatchNotFoundException()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new MatchNotFoundException(guid));
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateTeamLeagueMatchDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatch("TeamLeague", Guid.NewGuid().ToString(), dto);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be($"Match \"{guid}\" not found.");
        }

        [Fact]
        public async void Given_TeamDoesNotExist_When_UpdateTeamLeagueMatch_Then_ThrowTeamNotFoundException()
        {
            //Arrange
            var homeTeam = "Team A";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamNotFoundException(homeTeam));
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateTeamLeagueMatchDto
            {
                HomeTeam = homeTeam
            };

            //Act
            var result = await controller.UpdateTeamLeagueMatch("TeamLeague", Guid.NewGuid().ToString(), dto);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be($"Team \"{homeTeam}\" not found.");
        }

        [Fact]
        public async void Given_OtherException_When_UpdateTeamLeagueMatch_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new Exception());
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateTeamLeagueMatchDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatch("TeamLeague", Guid.NewGuid().ToString(), dto);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be("Something went wrong!");
        }
    }
}