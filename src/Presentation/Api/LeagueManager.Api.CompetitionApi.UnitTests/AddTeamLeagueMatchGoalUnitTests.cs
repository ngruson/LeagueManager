using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Player.Dto;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Goals;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class AddTeamLeagueMatchGoalUnitTests
    {
        [Fact]
        public async void Given_GoalIsAdded_When_AddTeamLeagueMatchGoal_Then_ReturnGoal()
        {
            //Arrange
            var goal = new GoalDto
            {
                Guid = Guid.NewGuid(),
                Minute = "1",
                Player = new PlayerDto
                {
                    FirstName = "John",
                    LastName = "Doe",
                    FullName = "John Doe"
                },
                TeamName = "Tottenham Hotspur"
            };

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(goal);


            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                new Guid("00000000-0000-0000-0000-000000000000"),
                "Tottenham Hotspur",
                new AddTeamLeagueMatchGoalDto
                {
                    Minute = "1",
                    PlayerName = "John Doe"
                }
            );

            //Assert
            var badRequest = result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var resultGoal = badRequest.Value.Should().BeAssignableTo<GoalDto>().Subject;
            resultGoal.Should().BeEquivalentTo(goal);
        }

        [Fact]
        public async void Given_TeamLeagueNotFoundExceptionIsThrown_When_AddTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamLeagueNotFoundException("Premier League"));

            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                new Guid("00000000-0000-0000-0000-000000000000"),
                "Tottenham Hotspur",
                new AddTeamLeagueMatchGoalDto
                {
                    Minute = "1",
                    PlayerName = "John Doe"
                }
            );

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Team league \"Premier League\" not found.");
        }

        [Fact]
        public async void Given_MatchNotFoundExceptionIsThrown_When_AddTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new MatchNotFoundException(matchGuid));

            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                matchGuid,
                "Tottenham Hotspur",
                new AddTeamLeagueMatchGoalDto
                {
                    Minute = "1",
                    PlayerName = "John Doe"
                }
            );

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Match \"{matchGuid}\" not found.");
        }

        [Fact]
        public async void Given_MatchEntryNotFoundExceptionIsThrown_When_AddTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var teamName = "Tottenham Hotspur";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new MatchEntryNotFoundException(teamName));

            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                matchGuid,
                "Tottenham Hotspur",
                new AddTeamLeagueMatchGoalDto
                {
                    Minute = "1",
                    PlayerName = "John Doe"
                }
            );

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"No match entry found for team \"{teamName}\".");
        }

        [Fact]
        public async void Given_PlayerNotFoundExceptionIsThrown_When_AddTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var playerName = "John Doe";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new PlayerNotFoundException(playerName));

            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                matchGuid,
                "Tottenham Hotspur",
                new AddTeamLeagueMatchGoalDto
                {
                    Minute = "1",
                    PlayerName = playerName
                }
            );

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Player \"{playerName}\" not found.");
        }

        [Fact]
        public async void Given_OtherExceptionIsThrown_When_AddTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var playerName = "John Doe";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new Exception());

            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                matchGuid,
                "Tottenham Hotspur",
                new AddTeamLeagueMatchGoalDto
                {
                    Minute = "1",
                    PlayerName = playerName
                }
            );

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Something went wrong!");
        }
    }
}