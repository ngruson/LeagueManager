using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public class UpdateTeamLeagueMatchScoreUnitTests
    {
        [Fact]
        public async void Given_MatchDoesExist_When_UpdateTeamLeagueMatchScore_Then_ReturnOk()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchScoreCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new TeamMatchDto());

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());
            var dto = new UpdateScoreDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchScore("TeamLeague", Guid.NewGuid().ToString(), dto);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<TeamMatchDto>();
        }

        [Fact]
        public async void Given_MatchNotFoundException_When_UpdateTeamLeagueMatchScore_Then_ReturnBadRequest()
        {
            //Arrange
            var matchGuid = Guid.NewGuid();
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchScoreCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new MatchNotFoundException(matchGuid));

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());
            var dto = new UpdateScoreDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchScore("TeamLeague", Guid.NewGuid().ToString(), dto);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be($"Match \"{matchGuid}\" not found.");
        }

        [Fact]
        public async void Given_TeamNotFoundException_When_UpdateTeamLeagueMatchScore_Then_ReturnBadRequest()
        {
            //Arrange
            var teamName = "Tottenham Hotspur";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchScoreCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamNotFoundException(teamName));

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());
            var dto = new UpdateScoreDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchScore("TeamLeague", Guid.NewGuid().ToString(), dto);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be($"Team \"{teamName}\" not found.");
        }

        [Fact]
        public async void Given_OtherException_When_UpdateTeamLeagueMatchScore_Then_ReturnBadRequest()
        {
            //Arrange
            var teamName = "Tottenham Hotspur";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchScoreCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new Exception());

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());
            var dto = new UpdateScoreDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchScore("TeamLeague", Guid.NewGuid().ToString(), dto);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be("Something went wrong!");
        }
    }
}