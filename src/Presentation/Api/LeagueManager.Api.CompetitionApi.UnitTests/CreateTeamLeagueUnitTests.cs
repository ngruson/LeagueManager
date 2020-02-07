using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading;
using Xunit;
using AutoMapper;
using LeagueManager.Application.AutoMapper;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class CreateTeamLeagueUnitTests
    {
        [Fact]
        public async void Given_TeamLeagueDoesNotExist_When_CreateTeamLeague_Then_ReturnSuccess()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            var command = new CreateTeamLeagueCommand
            {
                Name = "Premier League",
                Country = "England"
            };

            //Act
            var result = await controller.CreateTeamLeague(command);

            //Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async void Given_TeamLeagueAlreadyExists_When_CreateTeamLeague_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<CreateTeamLeagueCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new CompetitionAlreadyExistsException("Premier League"));
            
            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            var command = new CreateTeamLeagueCommand
            {
                Name = "Premier League",
                Country = "England"
            };

            //Act
            var result = await controller.CreateTeamLeague(command);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be("Competition \"Premier League\" already exists.");
        }

        [Fact]
        public async void Given_TeamNotFound_When_CreateTeamLeague_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<CreateTeamLeagueCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamNotFoundException("Liverpool"));

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            var command = new CreateTeamLeagueCommand();

            //Act
            var result = await controller.CreateTeamLeague(command);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be("Team \"Liverpool\" not found.");
        }

        [Fact]
        public async void Given_OtherExceptionOccurs_When_CreateTeamLeague_Then_ReturnBadRequest()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<CreateTeamLeagueCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new Exception());

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());
            var command = new CreateTeamLeagueCommand();

            //Act
            var result = await controller.CreateTeamLeague(command);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be("Something went wrong!");
        }
    }
}