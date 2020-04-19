using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamCompetitor.Dto;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
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
    public class GetPlayersForTeamCompetitorUnitTests
    {
        [Fact]
        public async void Given_CompetitorHasPlayers_When_GetPlayersForTeamCompetitor_Then_ReturnPlayers()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetPlayersForTeamCompetitorQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new TeamCompetitorPlayerDto[]
                {
                    new TeamCompetitorPlayerDto
                    {
                        Number = "1",
                        Player = new Application.Player.Dto.PlayerDto
                        {
                            FirstName = "John",
                            LastName = "Doe"
                        }
                    },
                    new TeamCompetitorPlayerDto
                    {
                        Number = "2",
                        Player = new Application.Player.Dto.PlayerDto
                        {
                            FirstName = "Jane",
                            LastName = "Doe"
                        }
                    }
                });
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.GetPlayersForTeamCompetitor(null, null);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var players = okResult.Value.Should().BeAssignableTo<IEnumerable<TeamCompetitorPlayerDto>>().Subject;
            players.Count().Should().Be(2);
        }

        [Fact]
        public async void Given_TeamLeagueNotFoundExceptionIsThrown_When_GetPlayersForTeamCompetitor_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetPlayersForTeamCompetitorQuery>(),
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
            var result = await controller.GetPlayersForTeamCompetitor(null, null);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be("Team league \"Premier League\" not found.");
        }

        [Fact]
        public async void Given_TeamNotFoundExceptionIsThrown_When_GetPlayersForTeamCompetitor_Then_ReturnBadRequest()
        {
            //Arrange
            string teamName = "Tottenham Hotspur";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetPlayersForTeamCompetitorQuery>(),
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
            var result = await controller.GetPlayersForTeamCompetitor(null, null);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Team \"{teamName}\" not found.");
        }

        [Fact]
        public async void Given_OtherExceptionIsThrown_When_GetPlayersForTeamCompetitor_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetPlayersForTeamCompetitorQuery>(),
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
            var result = await controller.GetPlayersForTeamCompetitor(null, null);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be("Something went wrong!");
        }
    }
}