using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class GetTeamLeagueMatchEventsUnitTests
    {
        [Fact]
        public async void Given_MatchEventsExists_When_GetTeamLeagueMatchEvents_Then_ReturnMatchEvents()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueMatchEventsQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(
                    new MatchEventsDto()
                );
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.GetMatchEvents(
                "Premier League",
                Guid.NewGuid(),
                "Tottenham Hotspur"
            );

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<MatchEventsDto>();
        }

        [Fact]
        public async void Given_ExceptionIsThrown_When_GetTeamLeagueMatchEvents_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueMatchEventsQuery>(),
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
            var result = await controller.GetMatchEvents(
                "Premier League",
                Guid.NewGuid(),
                "Tottenham Hotspur"
            );

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be("Something went wrong!");
        }
    }
}
