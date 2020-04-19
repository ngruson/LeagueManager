using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Goals;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class UpdateTeamLeagueMatchGoalUnitTests
    {
        [Fact]
        public async void Given_GoalDoesExist_When_UpdateTeamLeagueMatchGoal_Then_ReturnOk()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new GoalDto());
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateTeamLeagueMatchGoalDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchGoal(
                "TeamLeague", 
                Guid.NewGuid(), 
                "Tottenham Hotspur", 
                Guid.NewGuid(), 
                dto);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<GoalDto>();
        }

        [Fact]
        public async void Given_TeamLeagueNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            string teamLeague = "Premier League";

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamLeagueNotFoundException(teamLeague));

            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateTeamLeagueMatchGoalDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchGoal(
                "TeamLeague",
                Guid.NewGuid(),
                "Tottenham Hotspur",
                Guid.NewGuid(),
                dto);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Team league \"Premier League\" not found.");
        }

        [Fact]
        public async void Given_MatchNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            var matchGuid = Guid.NewGuid();

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new MatchNotFoundException(matchGuid));

            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateTeamLeagueMatchGoalDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchGoal(
                "TeamLeague",
                matchGuid,
                "Tottenham Hotspur",
                Guid.NewGuid(),
                dto);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Match \"{matchGuid}\" not found.");
        }

        [Fact]
        public async void Given_MatchEntryNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            var teamName = "Tottenham Hotspur";

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new MatchEntryNotFoundException(teamName));

            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateTeamLeagueMatchGoalDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchGoal(
                "TeamLeague",
                Guid.NewGuid(),
                teamName,
                Guid.NewGuid(),
                dto);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"No match entry found for team \"{teamName}\".");
        }

        [Fact]
        public async void Given_PlayerNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            var playerName = "John Doe";

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new PlayerNotFoundException(playerName));

            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateTeamLeagueMatchGoalDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchGoal(
                "TeamLeague",
                Guid.NewGuid(),
                "Tottenham Hotspur",
                Guid.NewGuid(),
                dto);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Player \"{playerName}\" not found.");
        }

        [Fact]
        public async void Given_OtherExceptionIsThrown_When_UpdateTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new Exception());

            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateTeamLeagueMatchGoalDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchGoal(
                "TeamLeague",
                Guid.NewGuid(),
                "Tottenham Hotspur",
                Guid.NewGuid(),
                dto);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Something went wrong!");
        }
    }
}