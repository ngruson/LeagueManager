using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.TeamLeagues.Dto;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class GetTeamLeagueRoundsUnitTests
    {
        [Fact]
        public async void Given_RoundsExist_When_GetTeamLeagueRounds_Then_ReturnRounds()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueRoundsQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(
                    new List<TeamLeagueRoundDto>()
                );

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            //Act
            var result = await controller.GetTeamLeagueRounds("Premier League");

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<IEnumerable<TeamLeagueRoundDto>>();
        }

        [Fact]
        public async void Given_ExceptionIsThrown_When_GetTeamLeagueRounds_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueRoundsQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(
                    new Exception()
                );

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            //Act
            var result = await controller.GetTeamLeagueRounds("Premier League");

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be("Something went wrong!");
        }
    }
}