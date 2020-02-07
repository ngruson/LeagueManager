using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class AddPlayerToTeamCompetitorUnitTests
    {
        [Fact]
        public async void Given_CompetitorHasPlayers_When_AddPlayerToTeamCompetitor_Then_ReturnPlayer()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            //Act
            var command = new AddPlayerToTeamCompetitorCommand
            {
                LeagueName = "Premier League",
                TeamName = "Tottenham Hotspur",
                PlayerName = "John Doe",
                PlayerNumber = "1"
            };
            var result = await controller.AddPlayerToTeamCompetitor("Premier League", command);

            //Assert
            var okResult = result.Should().BeOfType<CreatedResult>().Subject;
            var resultCommand = okResult.Value.Should().BeAssignableTo<AddPlayerToTeamCompetitorCommand>().Subject;
            resultCommand.Should().BeSameAs(command);
        }

        [Fact]
        public async void Given_TeamLeagueNotFoundExceptionIsThrown_When_GetPlayerForTeamCompetitor_Then_ReturnBadRequest()
        {
            //Arrange
            string leagueName = "Premier League";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddPlayerToTeamCompetitorCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamLeagueNotFoundException(leagueName));

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            //Act
            var result = await controller.AddPlayerToTeamCompetitor(null, null);

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
                    It.IsAny<AddPlayerToTeamCompetitorCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamNotFoundException(teamName));

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            //Act
            var result = await controller.AddPlayerToTeamCompetitor(null, null);

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
                    It.IsAny<AddPlayerToTeamCompetitorCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new PlayerNotFoundException(playerName));

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            //Act
            var result = await controller.AddPlayerToTeamCompetitor(null, null);

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
                    It.IsAny<AddPlayerToTeamCompetitorCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new Exception());

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            //Act
            var result = await controller.AddPlayerToTeamCompetitor(null, null);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be("Something went wrong!");
        }
    }
}