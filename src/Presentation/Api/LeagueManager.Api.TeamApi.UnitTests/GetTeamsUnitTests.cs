using FluentAssertions;
using LeagueManager.Api.TeamApi.Controllers;
using LeagueManager.Application.Teams.Queries.GetTeams;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.TeamApi.UnitTests
{
    public class TeamControllerUnitTests
    {
        [Fact]
        public async void Given_TeamExist_When_GetTeams_Then_ReturnTeam()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamsQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new TeamDto[]
                {
                    new TeamDto { Name = "Liverpool", Country = "England" }
                });

            var controller = new TeamController(mockMediator.Object);

            //Act
            var result = await controller.GetTeams();

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var teams = okResult.Value.Should().BeAssignableTo<IEnumerable<TeamDto>>().Subject;
            teams.ToList().Count.Should().Be(1);
            var team = teams.ToList()[0];
            team.Name.Should().Be("Liverpool");
            team.Country.Should().Be("England");
        }

        [Fact]
        public async void Given_TeamExistWithNoCountry_When_GetTeams_Then_ReturnTeam()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamsQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new TeamDto[]
                {
                    new TeamDto { Name = "Liverpool" }
                });

            var controller = new TeamController(mockMediator.Object);

            //Act
            var result = await controller.GetTeams();

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var teams = okResult.Value.Should().BeAssignableTo<IEnumerable<TeamDto>>().Subject;
            teams.ToList().Count.Should().Be(1);
            var team = teams.ToList()[0];
            team.Name.Should().Be("Liverpool");
            team.Country.Should().BeNull();
        }

        [Fact]
        public async void Given_Exception_When_GetTeams_Then_ReturnBadRequest()
        {
            //Arrange
            var controller = new TeamController(null);

            //Act
            var result = await controller.GetTeams();

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}