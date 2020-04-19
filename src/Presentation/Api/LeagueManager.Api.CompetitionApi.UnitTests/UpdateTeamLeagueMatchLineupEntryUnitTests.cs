using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using Lineup = LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading;
using Xunit;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Commands.UpdateTeamLeagueMatchLineupEntry;
using Microsoft.Extensions.Logging;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class UpdateTeamLeagueMatchLineupEntryUnitTests
    {
        [Fact]
        public async void Given_LineupEntryDoesExist_When_UpdateTeamLeagueMatchLineupEntry_Then_ReturnOk()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchLineupEntryCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new Lineup.LineupEntryDto());
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateLineupEntryDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchLineupEntry("TeamLeague", Guid.NewGuid(), "Tottenham Hotspur", Guid.NewGuid(), dto);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<Lineup.LineupEntryDto>();
        }

        [Fact]
        public async void Given_LineupEntryNotFoundException_When_UpdateTeamLeagueMatchLineupEntry_Then_ReturnBadRequest()
        {
            //Arrange
            var lineupEntryGuid = Guid.NewGuid();
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchLineupEntryCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new LineupEntryNotFoundException(lineupEntryGuid));
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );
            var dto = new UpdateLineupEntryDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchLineupEntry("TeamLeague", Guid.NewGuid(), "Tottenham Hotspur", Guid.NewGuid(), dto);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"No lineup entry found with id \"{lineupEntryGuid}\".");
        }

        [Fact]
        public async void Given_OtherException_When_UpdateTeamLeagueMatchLineupEntry_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchLineupEntryCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new Exception());
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            var dto = new UpdateLineupEntryDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatchLineupEntry("TeamLeague", Guid.NewGuid(), "Tottenham Hotspur", Guid.NewGuid(), dto);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be("Something went wrong!");
        }
    }
}