using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class GetTeamLeagueMatchDetailsUnitTests
    {
        [Fact]
        public async void Given_MatchExists_When_GetTeamLeagueMatchDetails_Then_ReturnMatch()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueMatchDetailsQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(
                    new TeamMatchDto()
                );
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.GetTeamLeagueMatchDetails("Premier League", Guid.NewGuid());

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<TeamMatchDto>();
        }

        [Fact]
        public async void Given_ExceptionIsThrown_When_GetTeamLeagueMatchDetails_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueMatchDetailsQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(
                    new Exception()
                );
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.GetTeamLeagueMatchDetails("Premier League", Guid.NewGuid());

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be("Something went wrong!");
        }
    }
}