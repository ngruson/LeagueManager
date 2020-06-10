using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague;
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
    public class CompetitionsControllerUnitTests
    {
        public class GetCompetitions
        {
            [Fact]
            public async void Given_CompetitionsExist_When_GetCompetitions_Then_ReturnCompetitions()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetCompetitionsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(new Application.Competitions.Queries.GetCompetitions.CompetitionDto[]
                    {
                    new Application.Competitions.Queries.GetCompetitions.CompetitionDto { Name = "Premier League", Country = "England" },
                    new Application.Competitions.Queries.GetCompetitions.CompetitionDto { Name = "Ligue 1", Country = "France" },
                    new Application.Competitions.Queries.GetCompetitions.CompetitionDto { Name = "Primera Division", Country = "Spain" },
                    });

                var controller = new CompetitionsController(
                    mockMediator.Object
                );

                //Act
                var result = await controller.GetCompetitions(null);

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                var competitions = okResult.Value.Should().BeAssignableTo<IEnumerable<Application.Competitions.Queries.GetCompetitions.CompetitionDto>>().Subject;
                var orderedList = competitions
                    .OrderBy(t => t.Name)
                    .Select(t => new Application.Competitions.Queries.GetCompetitions.CompetitionDto { Name = t.Name });
                competitions.Count().Should().Be(3);
                competitions.SequenceEqual(orderedList);
            }

            [Fact]
            public async void Given_Exception_When_GetCompetitions_Then_ReturnBadRequest()
            {
                //Arrange
                var controller = new CompetitionsController(null);

                //Act
                var result = await controller.GetCompetitions(null);

                //Assert
                result.Should().BeOfType<BadRequestObjectResult>();
            }
        }

        public class GetCompetition
        {
            [Fact]
            public async void Given_CompetitionsExist_When_GetCompetition_Then_ReturnCompetition()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetCompetitionQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(
                        new Application.Competitions.Queries.GetCompetition.CompetitionDto { Name = "Premier League", Country = "England" }
                    );

                var controller = new CompetitionsController(
                    mockMediator.Object
                );

                //Act
                var result = await controller.GetCompetition("Premier League");

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                var competition = okResult.Value.Should().BeAssignableTo<Application.Competitions.Queries.GetCompetition.CompetitionDto>().Subject;
                competition.Name.Should().Be("Premier League");
            }

            [Fact]
            public async void Given_Exception_When_GetCompetition_Then_ReturnBadRequest()
            {
                //Arrange
                var controller = new CompetitionsController(null);

                //Act
                var result = await controller.GetCompetition(null);

                //Assert
                result.Should().BeOfType<BadRequestObjectResult>();
            }
        }

        public class CreateTeamLeague
        {
            [Fact]
            public async void Given_TeamLeagueDoesNotExist_When_CreateTeamLeague_Then_ReturnSuccess()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                var controller = new CompetitionsController(
                    mockMediator.Object
                );

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

                var controller = new CompetitionsController(
                    mockMediator.Object
                );

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
                var mockLogger = new Mock<ILogger<CompetitionsController>>();

                var controller = new CompetitionsController(
                    mockMediator.Object
                );

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
                var mockLogger = new Mock<ILogger<CompetitionsController>>();

                var controller = new CompetitionsController(
                    mockMediator.Object
                );
                var command = new CreateTeamLeagueCommand();

                //Act
                var result = await controller.CreateTeamLeague(command);

                //Assert
                var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
                error.Value.Should().Be("Something went wrong!");
            }
        }
    }
}