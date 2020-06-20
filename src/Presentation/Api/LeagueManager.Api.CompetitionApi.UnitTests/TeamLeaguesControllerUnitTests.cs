using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Match.Commands.AddPlayerToLineup;
using LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeague;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueCompetitors;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchSubstitution;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class TeamLeaguesControllerUnitTests
    {
        public class GetTeamLeague
        {
            [Fact]
            public async void Given_TableExist_When_GetTeamLeague_Then_ReturnTable()
            {
                //Arrange
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(
                        new GetTeamLeagueVm()
                    );

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeague("Premier League");

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<GetTeamLeagueVm>();
            }

            [Fact]
            public async void Given_LeagueManagerExceptionIsThrown_When_GetTeamLeague_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(
                        new TeamLeagueNotFoundException("Premier League")
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeague("Premier League");

                //Assert
                result.Should().BeAssignableTo<BadRequestObjectResult>();
            }

            [Fact]
            public async void Given_ExceptionIsThrown_When_GetTeamLeague_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(
                        new Exception()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeague("Premier League");

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class GetCompetitors
        {
            [Fact]
            public async void Given_CompetitorsExist_When_GetCompetitors_Then_ReturnCompetitors()
            {
                //Arrange
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueCompetitorsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(
                        new List<CompetitorDto>
                        {
                            new CompetitorDto { TeamName = "Team 1 " },
                            new CompetitorDto { TeamName = "Team 2 " }
                        }
                    );

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetCompetitors("Premier League");

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<IEnumerable<CompetitorDto>>();
            }

            [Fact]
            public async void Given_LeagueManagerExceptionIsThrown_When_GetCompetitors_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueCompetitorsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(
                        new TeamLeagueNotFoundException("Premier League")
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetCompetitors("Premier League");

                //Assert
                result.Should().BeAssignableTo<BadRequestObjectResult>();
            }

            [Fact]
            public async void Given_ExceptionIsThrown_When_GetCompetitors_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueCompetitorsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(
                        new Exception()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetCompetitors("Premier League");

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class GetPlayersForTeamCompetitor
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
                    .ReturnsAsync(new Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor.CompetitorPlayerDto[]
                    {
                    new Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor.CompetitorPlayerDto
                    {
                        Number = "1",
                        Player = new Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor.PlayerDto
                        {
                            FirstName = "John",
                            LastName = "Doe"
                        }
                    },
                    new Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor.CompetitorPlayerDto
                    {
                        Number = "2",
                        Player = new Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor.PlayerDto
                        {
                            FirstName = "Jane",
                            LastName = "Doe"
                        }
                    }
                    });
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetPlayersForTeamCompetitor(null, null);

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                var players = okResult.Value.Should().BeAssignableTo<IEnumerable<Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor.CompetitorPlayerDto>>().Subject;
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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetPlayersForTeamCompetitor(null, null);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class GetPlayerForTeamCompetitor
        {
            [Fact]
            public async void Given_CompetitorHasPlayers_When_GetPlayerForTeamCompetitor_Then_ReturnPlayer()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetPlayerForTeamCompetitorQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(new Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor.CompetitorPlayerDto
                    {
                        Number = "1",
                        Player = new Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor.PlayerDto
                        {
                            FirstName = "John",
                            LastName = "Doe"
                        }
                    }
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetPlayerForTeamCompetitor(null, null, null);

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor.CompetitorPlayerDto>();
            }

            [Fact]
            public async void Given_TeamLeagueNotFoundExceptionIsThrown_When_GetPlayerForTeamCompetitor_Then_ReturnBadRequest()
            {
                //Arrange
                string leagueName = "Premier League";
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetPlayerForTeamCompetitorQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new TeamLeagueNotFoundException(leagueName));
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetPlayerForTeamCompetitor(null, null, null);

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
                        It.IsAny<GetPlayerForTeamCompetitorQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new TeamNotFoundException(teamName));
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetPlayerForTeamCompetitor(null, null, null);

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
                        It.IsAny<GetPlayerForTeamCompetitorQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new PlayerNotFoundException(playerName));
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetPlayerForTeamCompetitor(null, null, null);

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
                        It.IsAny<GetPlayerForTeamCompetitorQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new Exception());
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetPlayerForTeamCompetitor(null, null, null);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class GetTeamLeagueRounds
        {
            [Fact]
            public async void Given_RoundsExist_When_GetTeamLeagueRounds_Then_ReturnRounds()
            {
                //Arrange
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueRoundsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(
                        new GetTeamLeagueRoundsVm
                        {
                            Rounds = new List<Application.TeamLeagues.Queries.GetTeamLeagueRounds.RoundDto>
                            {
                                new Application.TeamLeagues.Queries.GetTeamLeagueRounds.RoundDto()
                                {
                                    Name = "Round 1"
                                },
                                new Application.TeamLeagues.Queries.GetTeamLeagueRounds.RoundDto()
                                {
                                    Name = "Round 2"
                                }
                            }
                        }
                    );

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueRounds("Premier League");

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                var vm = okResult.Value.Should().BeOfType<GetTeamLeagueRoundsVm>().Subject;
                vm.Rounds.Should().BeAssignableTo<IEnumerable<Application.TeamLeagues.Queries.GetTeamLeagueRounds.RoundDto>>();
            }

            [Fact]
            public async void Given_LeagueManagerExceptionIsThrown_When_GetPlayerForTeamCompetitor_Then_ReturnBadRequest()
            {
                //Arrange
                string leagueName = "Premier League";
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueRoundsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new TeamLeagueNotFoundException(leagueName));
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueRounds(null);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Team league \"{leagueName}\" not found.");
            }

            [Fact]
            public async void Given_ExceptionIsThrown_When_GetTeamLeagueRounds_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueRoundsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(
                        new Exception()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueRounds("Premier League");

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class GetTeamLeagueTable
        {
            [Fact]
            public async void Given_TableExist_When_GetTeamLeagueTable_Then_ReturnTable()
            {
                //Arrange
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueTableQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(
                        new GetTeamLeagueTableVm {  Table = new TableDto() }
                    );

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueTable("Premier League");

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                var vm = okResult.Value.Should().BeOfType<GetTeamLeagueTableVm>().Subject;
                vm.Table.Should().BeAssignableTo<TableDto>();
            }

            [Fact]
            public async void Given_LeagueManagerExceptionIsThrown_When_GetTeamLeagueTable_Then_ReturnBadRequest()
            {
                //Arrange
                string leagueName = "Premier League";
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueTableQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new TeamLeagueNotFoundException(leagueName));
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueTable(null);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Team league \"{leagueName}\" not found.");
            }

            [Fact]
            public async void Given_ExceptionIsThrown_When_GetTeamLeagueTable_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueTableQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(
                        new Exception()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueTable("Premier League");

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class GetTeamLeagueMatch
        {
            [Fact]
            public async void Given_MatchExists_When_GetTeamLeagueMatch_Then_ReturnMatch()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueMatchQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(
                        new Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch.TeamMatchDto()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueMatch("Premier League", Guid.NewGuid());

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch.TeamMatchDto>();
            }

            [Fact]
            public async void Given_LeagueManagerExceptionIsThrown_When_GetTeamLeagueMatch_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueMatchQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(
                        new TeamLeagueNotFoundException()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueMatch("Premier League", Guid.NewGuid());

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            }

            [Fact]
            public async void Given_ExceptionIsThrown_When_GetTeamLeagueMatch_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueMatchQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(
                        new Exception()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueMatch("Premier League", Guid.NewGuid());

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class GetTeamLeagueMatchDetails
        {
            [Fact]
            public async void Given_MatchExists_When_GetTeamLeagueMatchDetails_Then_ReturnMatch()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueMatchDetailsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(
                        new Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.TeamMatchDto()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueMatchDetails("Premier League", Guid.NewGuid());

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.TeamMatchDto>();
            }

            [Fact]
            public async void Given_LeagueManagerExceptionIsThrown_When_GetTeamLeagueMatchDetails_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueMatchDetailsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(
                        new TeamLeagueNotFoundException()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueMatchDetails("Premier League", Guid.NewGuid());

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            }

            [Fact]
            public async void Given_ExceptionIsThrown_When_GetTeamLeagueMatchDetails_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueMatchDetailsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(
                        new Exception()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueMatchDetails("Premier League", Guid.NewGuid());

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class GetTeamLeagueMatchLineupEntry
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
                        new Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry.LineupEntryDto()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueMatchLineupEntry("Premier League", Guid.NewGuid(), Guid.NewGuid());

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry.LineupEntryDto>();
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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueMatchLineupEntry("Premier League", Guid.NewGuid(), Guid.NewGuid());

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class GetTeamLeagueMatchGoal
        {
            [Fact]
            public async void Given_GoalExists_When_GetTeamLeagueMatchGoal_Then_ReturnGoal()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueMatchGoalQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(
                        new Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal.GoalDto()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueMatchGoal(
                    "Premier League",
                    Guid.NewGuid(),
                    Guid.NewGuid()
                );

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal.GoalDto>();
            }

            [Fact]
            public async void Given_GoalNotFoundExceptionIsThrown_When_GetTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                var goalGuid = Guid.NewGuid();
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueMatchGoalQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new GoalNotFoundException(goalGuid));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueMatchGoal(
                    "Premier League",
                    Guid.NewGuid(),
                    Guid.NewGuid()
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"No goal found with id \"{goalGuid}\".");
            }

            [Fact]
            public async void Given_OtherExceptionIsThrown_When_GetTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                var goalGuid = Guid.NewGuid();
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueMatchGoalQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new Exception());

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetTeamLeagueMatchGoal(
                    "Premier League",
                    Guid.NewGuid(),
                    Guid.NewGuid()
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class AddPlayerToLineup
        {
            [Fact]
            public async void Given_AllConditionsPass_When_AddPlayerToLineup_Then_ReturnSuccess()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new AddPlayerToLineupCommand
                {
                    Number = "1",
                    Player = "John Doe"
                };

                //Act
                var result = await controller.AddPlayerToLineup("Premier League",
                    new Guid("00000000-0000-0000-0000-000000000000"),
                    "Tottenham Hotspur",
                    command);

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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new AddPlayerToLineupCommand
                {
                    Number = "1",
                    Player = "John Doe"
                };

                //Act
                var result = await controller.AddPlayerToLineup("Premier League",
                    new Guid("00000000-0000-0000-0000-000000000000"),
                    "Tottenham Hotspur",
                    command);

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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new AddPlayerToLineupCommand
                {
                    Number = "1",
                    Player = "John Doe"
                };

                //Act
                var result = await controller.AddPlayerToLineup("Premier League",
                    new Guid("00000000-0000-0000-0000-000000000000"),
                    "Tottenham Hotspur",
                    command);

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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new AddPlayerToLineupCommand
                {
                    Number = "1",
                    Player = "John Doe"
                };

                //Act
                var result = await controller.AddPlayerToLineup("Premier League",
                    Guid.NewGuid(),
                    teamName,
                    command);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"No match entry found for team \"{teamName}\".");
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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new AddPlayerToLineupCommand
                {
                    Number = "1",
                    Player = playerName
                };

                //Act
                var result = await controller.AddPlayerToLineup("Premier League",
                    Guid.NewGuid(),
                    playerName,
                    command);

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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new AddPlayerToLineupCommand
                {
                    Number = "1",
                    Player = playerName
                };

                //Act
                var result = await controller.AddPlayerToLineup("Premier League",
                    Guid.NewGuid(),
                    playerName,
                    command);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class AddPlayerToTeamCompetitor
        {
            [Fact]
            public async void Given_CompetitorHasPlayers_When_AddPlayerToTeamCompetitor_Then_ReturnPlayer()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddPlayerToTeamCompetitor(null, null);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }

        public class AddTeamLeagueMatchGoal
        {
            [Fact]
            public async void Given_GoalIsAdded_When_AddTeamLeagueMatchGoal_Then_ReturnGoal()
            {
                //Arrange
                var goal = new Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal.GoalDto
                {
                    Minute = "1",
                    PlayerName = "John Doe"
                };

                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(goal);


                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                    new Guid("00000000-0000-0000-0000-000000000000"),
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchGoalCommand
                    {
                        Minute = "1",
                        PlayerName = "John Doe"
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<OkObjectResult>().Subject;
                var resultGoal = badRequest.Value.Should().BeAssignableTo<Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal.GoalDto>().Subject;
                resultGoal.Should().BeEquivalentTo(goal);
            }

            [Fact]
            public async void Given_TeamLeagueNotFoundExceptionIsThrown_When_AddTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new TeamLeagueNotFoundException("Premier League"));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                    new Guid("00000000-0000-0000-0000-000000000000"),
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchGoalCommand
                    {
                        Minute = "1",
                        PlayerName = "John Doe"
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Team league \"Premier League\" not found.");
            }

            [Fact]
            public async void Given_MatchNotFoundExceptionIsThrown_When_AddTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new MatchNotFoundException(matchGuid));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                    matchGuid,
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchGoalCommand
                    {
                        Minute = "1",
                        PlayerName = "John Doe"
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Match \"{matchGuid}\" not found.");
            }

            [Fact]
            public async void Given_MatchEntryNotFoundExceptionIsThrown_When_AddTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
                var teamName = "Tottenham Hotspur";
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new MatchEntryNotFoundException(teamName));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                    matchGuid,
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchGoalCommand
                    {
                        Minute = "1",
                        PlayerName = "John Doe"
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"No match entry found for team \"{teamName}\".");
            }

            [Fact]
            public async void Given_PlayerNotFoundExceptionIsThrown_When_AddTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
                var playerName = "John Doe";
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new PlayerNotFoundException(playerName));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                    matchGuid,
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchGoalCommand
                    {
                        Minute = "1",
                        PlayerName = playerName
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Player \"{playerName}\" not found.");
            }

            [Fact]
            public async void Given_OtherExceptionIsThrown_When_AddTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
                var playerName = "John Doe";
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new Exception());

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchGoal("Premier League",
                    matchGuid,
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchGoalCommand
                    {
                        Minute = "1",
                        PlayerName = playerName
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Something went wrong!");
            }
        }

        public class AddTeamLeagueMatchSubstitution
        {
            [Fact]
            public async void Given_SubstitutionIsAdded_When_AddTeamLeagueMatchSubstitution_Then_ReturnSubstitution()
            {
                //Arrange
                var sub = new Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchSubstitution.SubstitutionDto
                {
                    Minute = "1",
                    PlayerOut = "John Doe",
                    PlayerIn = "Jane Doe"
                };

                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(sub);

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchSubstitution("Premier League",
                    new Guid("00000000-0000-0000-0000-000000000000"),
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchSubstitutionCommand
                    {
                        Minute = "1",
                        PlayerOut = "John Doe",
                        PlayerIn = "Jane Doe",
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<OkObjectResult>().Subject;
                var resultSub = badRequest.Value.Should().BeAssignableTo<Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchSubstitution.SubstitutionDto>().Subject;
                resultSub.Should().BeEquivalentTo(sub);
            }

            [Fact]
            public async void Given_TeamLeagueNotFoundExceptionIsThrown_When_AddTeamLeagueMatchSubstitution_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new TeamLeagueNotFoundException("Premier League"));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchSubstitution("Premier League",
                    new Guid("00000000-0000-0000-0000-000000000000"),
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchSubstitutionCommand
                    {
                        Minute = "1",
                        PlayerOut = "John Doe",
                        PlayerIn = "Jane Doe"
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Team league \"Premier League\" not found.");
            }

            [Fact]
            public async void Given_MatchNotFoundExceptionIsThrown_When_AddTeamLeagueMatchSubstitution_Then_ReturnBadRequest()
            {
                //Arrange
                var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new MatchNotFoundException(matchGuid));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchSubstitution("Premier League",
                    matchGuid,
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchSubstitutionCommand
                    {
                        Minute = "1",
                        PlayerOut = "John Doe",
                        PlayerIn = "Jane Doe"
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Match \"{matchGuid}\" not found.");
            }

            [Fact]
            public async void Given_MatchEntryNotFoundExceptionIsThrown_When_AddTeamLeagueMatchSubstitution_Then_ReturnBadRequest()
            {
                //Arrange
                var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
                var teamName = "Tottenham Hotspur";
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new MatchEntryNotFoundException(teamName));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchSubstitution("Premier League",
                    matchGuid,
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchSubstitutionCommand
                    {
                        Minute = "1",
                        PlayerOut = "John Doe",
                        PlayerIn = "Jane Doe"
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"No match entry found for team \"{teamName}\".");
            }

            [Fact]
            public async void Given_PlayerNotFoundExceptionIsThrown_When_AddTeamLeagueMatchSubstitution_Then_ReturnBadRequest()
            {
                //Arrange
                var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
                var playerName = "John Doe";
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new PlayerNotFoundException(playerName));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchSubstitution("Premier League",
                    matchGuid,
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchSubstitutionCommand
                    {
                        Minute = "1",
                        PlayerOut = playerName
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Player \"{playerName}\" not found.");
            }

            [Fact]
            public async void Given_OtherExceptionIsThrown_When_AddTeamLeagueMatchSubstitution_Then_ReturnBadRequest()
            {
                //Arrange
                var matchGuid = new Guid("00000000-0000-0000-0000-000000000000");
                var playerName = "John Doe";
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<AddTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new Exception());

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.AddTeamLeagueMatchSubstitution("Premier League",
                    matchGuid,
                    "Tottenham Hotspur",
                    new AddTeamLeagueMatchSubstitutionCommand
                    {
                        Minute = "1",
                        PlayerOut = playerName
                    }
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Something went wrong!");
            }
        }

        public class UpdateTeamLeagueMatch
        {
            [Fact]
            public async void Given_MatchDoesExist_When_UpdateTeamLeagueMatch_Then_ReturnOk()
            {
                //Arrange
                var leagueName = "Premier League";
                var homeTeam = "Tottenham Hotspur";
                var awayTeam = "Chelsea";
                var startTime = new DateTime(2020, 01, 01, 20, 15, 0);

                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch.TeamMatchDto
                    {
                        TeamLeagueName = leagueName,
                        StartTime = startTime,
                        MatchEntries = new List<ITeamMatchEntryDto>
                        {
                            new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch.TeamMatchEntryDto
                            {
                                HomeAway = Application.Interfaces.Dto.HomeAway.Home,
                                Team = new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch.TeamDto 
                                { 
                                    Name = homeTeam 
                                }
                            },
                            new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch.TeamMatchEntryDto
                            {
                                HomeAway = Application.Interfaces.Dto.HomeAway.Away,
                                Team = new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch.TeamDto 
                                { 
                                    Name = awayTeam 
                                }
                            }
                        }
                    });
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new UpdateTeamLeagueMatchCommand
                {
                    HomeTeam = homeTeam,
                    AwayTeam = awayTeam,
                    StartTime = startTime
                };

                //Act
                var result = await controller.UpdateTeamLeagueMatch("TeamLeague", Guid.NewGuid(), command);

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                var match = okResult.Value.Should().BeAssignableTo<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch.TeamMatchDto>().Subject;
                match.TeamLeagueName.Should().Be(leagueName);
                match.MatchEntries.ToList().Count.Should().Be(2);
                match.MatchEntries.Single(me => me.HomeAway == Application.Interfaces.Dto.HomeAway.Home)
                    .Team.Name.Should().Be(homeTeam);
                match.MatchEntries.Single(me => me.HomeAway == Application.Interfaces.Dto.HomeAway.Away)
                    .Team.Name.Should().Be(awayTeam);
                match.StartTime.Should().Be(startTime);
            }

            [Fact]
            public async void Given_MatchDoesNotExist_When_UpdateTeamLeagueMatch_Then_ThrowMatchNotFoundException()
            {
                //Arrange
                var guid = Guid.NewGuid();
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new MatchNotFoundException(guid));
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,                    
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new UpdateTeamLeagueMatchCommand();

                //Act
                var result = await controller.UpdateTeamLeagueMatch("TeamLeague", Guid.NewGuid(), command);

                //Assert
                var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
                error.Value.Should().Be($"Match \"{guid}\" not found.");
            }

            [Fact]
            public async void Given_TeamDoesNotExist_When_UpdateTeamLeagueMatch_Then_ThrowTeamNotFoundException()
            {
                //Arrange
                var homeTeam = "Team A";
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new TeamNotFoundException(homeTeam));
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new UpdateTeamLeagueMatchCommand
                {
                    HomeTeam = homeTeam
                };

                //Act
                var result = await controller.UpdateTeamLeagueMatch("TeamLeague", Guid.NewGuid(), command);

                //Assert
                var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
                error.Value.Should().Be($"Team \"{homeTeam}\" not found.");
            }

            [Fact]
            public async void Given_OtherException_When_UpdateTeamLeagueMatch_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new Exception());
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new UpdateTeamLeagueMatchCommand();

                //Act
                var result = await controller.UpdateTeamLeagueMatch("TeamLeague", Guid.NewGuid(), command);

                //Assert
                var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
                error.Value.Should().Be("Something went wrong!");
            }
        }

        public class UpdateTeamLeagueMatchScore
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
                    .ReturnsAsync(new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore.TeamMatchDto());
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );
                var dto = new UpdateTeamLeagueMatchScoreDto();

                //Act
                var result = await controller.UpdateTeamLeagueMatchScore("TeamLeague", Guid.NewGuid(), dto);

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore.TeamMatchDto>();
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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var dto = new UpdateTeamLeagueMatchScoreDto();

                //Act
                var result = await controller.UpdateTeamLeagueMatchScore("TeamLeague", Guid.NewGuid(), dto);

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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );
                var dto = new UpdateTeamLeagueMatchScoreDto();

                //Act
                var result = await controller.UpdateTeamLeagueMatchScore("TeamLeague", Guid.NewGuid(), dto);

                //Assert
                var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
                error.Value.Should().Be($"Team \"{teamName}\" not found.");
            }

            [Fact]
            public async void Given_OtherException_When_UpdateTeamLeagueMatchScore_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchScoreCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new Exception());
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );
                var dto = new UpdateTeamLeagueMatchScoreDto();

                //Act
                var result = await controller.UpdateTeamLeagueMatchScore("TeamLeague", Guid.NewGuid(), dto);

                //Assert
                var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
                error.Value.Should().Be("Something went wrong!");
            }
        }

        public class UpdateTeamLeagueMatchLineupEntry
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
                    .ReturnsAsync(new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry.LineupEntryDto());
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var command = new UpdateTeamLeagueMatchLineupEntryCommand();
                var result = await controller.UpdateTeamLeagueMatchLineupEntry("TeamLeague", Guid.NewGuid(), "Tottenham Hotspur", Guid.NewGuid(), command);

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry.LineupEntryDto>();
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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );
                var command = new UpdateTeamLeagueMatchLineupEntryCommand();

                //Act
                var result = await controller.UpdateTeamLeagueMatchLineupEntry("TeamLeague", Guid.NewGuid(), "Tottenham Hotspur", Guid.NewGuid(), command);

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
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new UpdateTeamLeagueMatchLineupEntryCommand();

                //Act
                var result = await controller.UpdateTeamLeagueMatchLineupEntry("TeamLeague", Guid.NewGuid(), "Tottenham Hotspur", Guid.NewGuid(), command);

                //Assert
                var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
                error.Value.Should().Be("Something went wrong!");
            }
        }

        public class UpdateTeamLeagueMatchGoal
        {
            [Fact]
            public async void Given_GoalDoesExist_When_UpdateTeamLeagueMatchGoal_Then_ReturnOk()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal.GoalDto());
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new UpdateTeamLeagueMatchGoalCommand();

                //Act
                var result = await controller.UpdateTeamLeagueMatchGoal(
                    "TeamLeague",
                    Guid.NewGuid(),
                    "Tottenham Hotspur",
                    Guid.NewGuid(),
                    command);

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal.GoalDto>();
            }

            [Fact]
            public async void Given_TeamLeagueNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                string teamLeague = "Premier League";

                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new TeamLeagueNotFoundException(teamLeague));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new UpdateTeamLeagueMatchGoalCommand();

                //Act
                var result = await controller.UpdateTeamLeagueMatchGoal(
                    "TeamLeague",
                    Guid.NewGuid(),
                    "Tottenham Hotspur",
                    Guid.NewGuid(),
                    command);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Team league \"Premier League\" not found.");
            }

            [Fact]
            public async void Given_MatchNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                var matchGuid = Guid.NewGuid();

                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new MatchNotFoundException(matchGuid));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new UpdateTeamLeagueMatchGoalCommand();

                //Act
                var result = await controller.UpdateTeamLeagueMatchGoal(
                    "TeamLeague",
                    matchGuid,
                    "Tottenham Hotspur",
                    Guid.NewGuid(),
                    command);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Match \"{matchGuid}\" not found.");
            }

            [Fact]
            public async void Given_MatchEntryNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                var teamName = "Tottenham Hotspur";

                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new MatchEntryNotFoundException(teamName));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new UpdateTeamLeagueMatchGoalCommand();

                //Act
                var result = await controller.UpdateTeamLeagueMatchGoal(
                    "TeamLeague",
                    Guid.NewGuid(),
                    teamName,
                    Guid.NewGuid(),
                    command);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"No match entry found for team \"{teamName}\".");
            }

            [Fact]
            public async void Given_PlayerNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                var playerName = "John Doe";

                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new PlayerNotFoundException(playerName));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new UpdateTeamLeagueMatchGoalCommand();

                //Act
                var result = await controller.UpdateTeamLeagueMatchGoal(
                    "TeamLeague",
                    Guid.NewGuid(),
                    "Tottenham Hotspur",
                    Guid.NewGuid(),
                    command);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Player \"{playerName}\" not found.");
            }

            [Fact]
            public async void Given_OtherExceptionIsThrown_When_UpdateTeamLeagueMatchGoal_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchGoalCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new Exception());

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                var command = new UpdateTeamLeagueMatchGoalCommand();

                //Act
                var result = await controller.UpdateTeamLeagueMatchGoal(
                    "TeamLeague",
                    Guid.NewGuid(),
                    "Tottenham Hotspur",
                    Guid.NewGuid(),
                    command);

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Something went wrong!");
            }
        }

        public class UpdateTeamLeagueMatchSubstitution
        {
            [Fact]
            public async void Given_SubstitutionDoesExist_When_UpdateTeamLeagueMatchSubstitution_Then_ReturnOk()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution.SubstitutionDto());
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.UpdateTeamLeagueMatchSubstitution(
                    "TeamLeague",
                    Guid.NewGuid(),
                    "Tottenham Hotspur",
                    Guid.NewGuid(),
                    new UpdateTeamLeagueMatchSubstitutionDto()
                );

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution.SubstitutionDto>();
            }

            [Fact]
            public async void Given_TeamLeagueNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchSubstitution_Then_ReturnBadRequest()
            {
                //Arrange
                string teamLeague = "Premier League";

                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new TeamLeagueNotFoundException(teamLeague));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.UpdateTeamLeagueMatchSubstitution(
                    "TeamLeague",
                    Guid.NewGuid(),
                    "Tottenham Hotspur",
                    Guid.NewGuid(),
                    new UpdateTeamLeagueMatchSubstitutionDto()
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Team league \"Premier League\" not found.");
            }

            [Fact]
            public async void Given_MatchNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchSubstitution_Then_ReturnBadRequest()
            {
                //Arrange
                var matchGuid = Guid.NewGuid();

                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new MatchNotFoundException(matchGuid));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.UpdateTeamLeagueMatchSubstitution(
                    "TeamLeague",
                    matchGuid,
                    "Tottenham Hotspur",
                    Guid.NewGuid(),
                    new UpdateTeamLeagueMatchSubstitutionDto()
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Match \"{matchGuid}\" not found.");
            }

            [Fact]
            public async void Given_MatchEntryNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchSubstitution_Then_ReturnBadRequest()
            {
                //Arrange
                var teamName = "Tottenham Hotspur";

                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new MatchEntryNotFoundException(teamName));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.UpdateTeamLeagueMatchSubstitution(
                    "TeamLeague",
                    Guid.NewGuid(),
                    teamName,
                    Guid.NewGuid(),
                    new UpdateTeamLeagueMatchSubstitutionDto()
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"No match entry found for team \"{teamName}\".");
            }

            [Fact]
            public async void Given_PlayerNotFoundExceptionIsThrown_When_UpdateTeamLeagueMatchSubstitution_Then_ReturnBadRequest()
            {
                //Arrange
                var playerName = "John Doe";

                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new PlayerNotFoundException(playerName));

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.UpdateTeamLeagueMatchSubstitution(
                    "TeamLeague",
                    Guid.NewGuid(),
                    "Tottenham Hotspur",
                    Guid.NewGuid(),
                    new UpdateTeamLeagueMatchSubstitutionDto()
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Player \"{playerName}\" not found.");
            }

            [Fact]
            public async void Given_OtherExceptionIsThrown_When_UpdateTeamLeagueMatchSubstitution_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<UpdateTeamLeagueMatchSubstitutionCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new Exception());

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.UpdateTeamLeagueMatchSubstitution(
                    "TeamLeague",
                    Guid.NewGuid(),
                    "Tottenham Hotspur",
                    Guid.NewGuid(),
                    new UpdateTeamLeagueMatchSubstitutionDto()
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be($"Something went wrong!");
            }
        }

        public class GetTeamLeagueMatchEvents
        {
            [Fact]
            public async void Given_MatchEventsExists_When_GetTeamLeagueMatchEvents_Then_ReturnMatchEvents()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueMatchEventsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(
                        new MatchEventsDto()
                    );
                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetMatchEvents(
                    "Premier League",
                    Guid.NewGuid(),
                    "Tottenham Hotspur"
                );

                //Assert
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.Value.Should().BeAssignableTo<MatchEventsDto>();
            }

            [Fact]
            public async void Given_ExceptionIsThrown_When_GetTeamLeagueMatchEvents_Then_ReturnBadRequest()
            {
                //Arrange
                var mockMediator = new Mock<IMediator>();
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTeamLeagueMatchEventsQuery>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .Throws(new Exception());

                var mockLogger = new Mock<ILogger<TeamLeaguesController>>();

                var controller = new TeamLeaguesController(
                    mockMediator.Object,
                    mockLogger.Object,
                    Mapper.CreateMapper()
                );

                //Act
                var result = await controller.GetMatchEvents(
                    "Premier League",
                    Guid.NewGuid(),
                    "Tottenham Hotspur"
                );

                //Assert
                var badRequest = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
                var errorMessage = badRequest.Value.Should().BeAssignableTo<string>().Subject;
                errorMessage.Should().Be("Something went wrong!");
            }
        }
    }
}