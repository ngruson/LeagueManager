using FluentAssertions;
using LeagueManager.Api.TeamApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Teams.Commands.CreateTeam;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.TeamApi.UnitTests
{
    public class CreateTeamUnitTests
    {
        [Fact]
        public async void Given_TeamDoesNotExist_When_CreateTeam_Then_ReturnTeam()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<CreateTeamCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(Unit.Value);

            var controller = new TeamController(mockMediator.Object);
            var command = new CreateTeamCommand { Name = "Liverpool", Country = "England" };

            //Act
            var result = await controller.CreateTeam(command);

            //Assert
            var okResult = result.Should().BeOfType<CreatedResult>().Subject;
        }

        [Fact]
        public async void Given_TeamDoesExist_When_CreateTeam_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<CreateTeamCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamAlreadyExistsException("Liverpool"));

            var controller = new TeamController(mockMediator.Object);
            var command = new CreateTeamCommand { Name = "Liverpool" };

            //Act
            var result = await controller.CreateTeam(command);

            //Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Team \"Liverpool\" already exists.");
        }

        [Fact]
        public async void Given_Exception_When_CreateTeam_Then_ReturnBadRequest()
        {
            //Arrange
            var controller = new TeamController(null);

            //Act
            var result = await controller.CreateTeam(null);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}