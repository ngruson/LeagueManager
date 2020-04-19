using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagueMatches.Goals;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class GetTeamLeagueMatchGoalUnitTests
    {
        [Fact]
        public async void Given_GoalExists_When_GetTeamLeagueMatchGoal_Then_ReturnGoal()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueMatchGoalQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(
                    new GoalDto()
                );
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.GetTeamLeagueMatchGoal(
                "Premier League", 
                Guid.NewGuid(), 
                Guid.NewGuid()
            );

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<GoalDto>();
        }

        [Fact]
        public async void Given_GoalNotFoundExceptionIsThrown_When_GetTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            var goalGuid = Guid.NewGuid();
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueMatchGoalQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new GoalNotFoundException(goalGuid));

            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.GetTeamLeagueMatchGoal(
                "Premier League",
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"No goal found with id \"{goalGuid}\".");
        }

        [Fact]
        public async void Given_OtherExceptionIsThrown_When_GetTeamLeagueMatchGoal_Then_ReturnBadRequest()
        {
            //Arrange
            var goalGuid = Guid.NewGuid();
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueMatchGoalQuery>(),
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
            var result = await controller.GetTeamLeagueMatchGoal(
                "Premier League",
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be("Something went wrong!");
        }
    }
}
