using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Player.Dto;
using LeagueManager.Application.TeamCompetitor.Dto;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor;
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
    public class GetPlayerForTeamCompetitorUnitTests
    {
        [Fact]
        public async void Given_CompetitorHasPlayers_When_GetPlayerForTeamCompetitor_Then_ReturnPlayer()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetPlayerForTeamCompetitorQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new TeamCompetitorPlayerDto
                    {
                        Number = "1",
                        Player = new PlayerDto
                        {
                            FirstName = "John",
                            LastName = "Doe"
                        }
                    }
                );
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.GetPlayerForTeamCompetitor(null, null, null);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<TeamCompetitorPlayerDto>();
        }

        [Fact]
        public async void Given_TeamLeagueNotFoundExceptionIsThrown_When_GetPlayerForTeamCompetitor_Then_ReturnBadRequest()
        {
            //Arrange
            string leagueName = "Premier League";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetPlayerForTeamCompetitorQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamLeagueNotFoundException(leagueName));
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.GetPlayerForTeamCompetitor(null, null, null);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Team league \"{leagueName}\" not found.");
        }

        [Fact]
        public async void Given_TeamNotFoundExceptionIsThrown_When_GetPlayerForTeamCompetitor_Then_ReturnBadRequest()
        {
            //Arrange
            string teamName = "Tottenham Hotspur";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetPlayerForTeamCompetitorQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamNotFoundException(teamName));
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.GetPlayerForTeamCompetitor(null, null, null);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Team \"{teamName}\" not found.");
        }

        [Fact]
        public async void Given_PlayerNotFoundExceptionIsThrown_When_GetPlayerForTeamCompetitor_Then_ReturnBadRequest()
        {
            //Arrange
            string playerName = "John Doe";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetPlayerForTeamCompetitorQuery>(),
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
            var result = await controller.GetPlayerForTeamCompetitor(null, null, null);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Player \"{playerName}\" not found.");
        }

        [Fact]
        public async void Given_OtherExceptionIsThrown_When_GetPlayerForTeamCompetitor_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetPlayerForTeamCompetitorQuery>(),
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
            var result = await controller.GetPlayerForTeamCompetitor(null, null, null);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be("Something went wrong!");
        }
    }
}
