using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class GetTeamLeagueMatchLineupEntryUnitTests
    {
        [Fact]
        public async void Given_LineupEntryExists_When_GetTeamLeagueMatchLineupEntry_Then_ReturnLineupEntry()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueMatchLineupEntryQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(
                    new LineupEntryDto()
                );

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            //Act
            var result = await controller.GetTeamLeagueMatchLineupEntry("Premier League", Guid.NewGuid(), Guid.NewGuid());

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<LineupEntryDto>();
        }

        [Fact]
        public async void Given_LineupEntryNotFoundExceptionIsThrown_When_GetTeamLeagueMatchLineupEntry_Then_ReturnBadRequest()
        {
            //Arrange
            var lineupEntryGuid = Guid.NewGuid();
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueMatchLineupEntryQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(
                    new LineupEntryNotFoundException(lineupEntryGuid)
                );

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            //Act
            var result = await controller.GetTeamLeagueMatchLineupEntry("Premier League", Guid.NewGuid(), lineupEntryGuid);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"No lineup entry found with id \"{ lineupEntryGuid}\".");
        }

        [Fact]
        public async void Given_OtherExceptionIsThrown_When_GetTeamLeagueMatchLineupEntry_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetTeamLeagueMatchLineupEntryQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(
                    new Exception()
                );

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            //Act
            var result = await controller.GetTeamLeagueMatchLineupEntry("Premier League", Guid.NewGuid(), Guid.NewGuid());

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be("Something went wrong!");
        }
    }
}
