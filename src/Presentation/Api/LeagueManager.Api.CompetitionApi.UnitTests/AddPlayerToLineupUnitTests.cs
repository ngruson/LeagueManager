using AutoMapper;
using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Match.Commands.AddPlayerToLineup;
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
    public class AddPlayerToLineupUnitTests
    {
        [Fact]
        public async void Given_AllConditionsPass_When_AddPlayerToLineup_Then_ReturnSuccess()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            var dto = new AddPlayerToLineupDto
            {
                Number = "1",
                Player = "John Doe"
            };

            //Act
            var result = await controller.AddPlayerToLineup("Premier League",
                new Guid("00000000-0000-0000-0000-000000000000"),
                "Tottenham Hotspur", 
                dto);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Given_TeamLeagueDoesNotExist_When_AddPlayerToLineup_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddPlayerToLineupCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamLeagueNotFoundException("Premier League"));

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            var dto = new AddPlayerToLineupDto
            {
                Number = "1",
                Player = "John Doe"
            };

            //Act
            var result = await controller.AddPlayerToLineup("Premier League",
                new Guid("00000000-0000-0000-0000-000000000000"),
                "Tottenham Hotspur",
                dto);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void Given_MatchDoesNotExist_When_AddPlayerToLineup_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddPlayerToLineupCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new MatchNotFoundException(new Guid("00000000-0000-0000-0000-000000000000")));

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            var dto = new AddPlayerToLineupDto
            {
                Number = "1",
                Player = "John Doe"
            };

            //Act
            var result = await controller.AddPlayerToLineup("Premier League",
                new Guid("00000000-0000-0000-0000-000000000000"),
                "Tottenham Hotspur",
                dto);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void Given_MatchEntryNotFoundException_When_AddPlayerToLineup_Then_ReturnBadRequest()
        {
            //Arrange
            string teamName = "Tottenham Hotspur";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddPlayerToLineupCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new MatchEntryNotFoundException(teamName));

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            var dto = new AddPlayerToLineupDto
            {
                Number = "1",
                Player = "John Doe"
            };

            //Act
            var result = await controller.AddPlayerToLineup("Premier League",
                Guid.NewGuid(),
                teamName,
                dto);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"No match entry found for team \"{teamName}\".") ;
        }

        [Fact]
        public async void Given_PlayerNotFoundException_When_AddPlayerToLineup_Then_ReturnBadRequest()
        {
            //Arrange
            string playerName = "John Doe";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddPlayerToLineupCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new PlayerNotFoundException(playerName));

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            var dto = new AddPlayerToLineupDto
            {
                Number = "1",
                Player = playerName
            };

            //Act
            var result = await controller.AddPlayerToLineup("Premier League",
                Guid.NewGuid(),
                playerName,
                dto);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be($"Player \"{playerName}\" not found.");
        }

        [Fact]
        public async void Given_OtherException_When_AddPlayerToLineup_Then_ReturnBadRequest()
        {
            //Arrange
            string playerName = "John Doe";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<AddPlayerToLineupCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new Exception());

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            var dto = new AddPlayerToLineupDto
            {
                Number = "1",
                Player = playerName
            };

            //Act
            var result = await controller.AddPlayerToLineup("Premier League",
                Guid.NewGuid(),
                playerName,
                dto);

            //Assert
            var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
            errorMessage.Should().Be("Something went wrong!");
        }
    }
}