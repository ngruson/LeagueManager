using FluentAssertions;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Infrastructure.Api;
using LeagueManager.Infrastructure.HttpHelpers;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeague;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueCompetitors;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution;

namespace LeagueManager.Infrastructure.UnitTests
{
    public class CompetitionApiUnitTests
    {
        public class Configure
        {
            [Fact]
            public async void Given_PutIsOK_When_Configure_Then_ReturnTrue()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var result = await sut.Configure(null, null);

                //Assert
                result.Should().BeTrue();
            }

            [Fact]
            public async void Given_PutIsNotOK_When_Configure_Then_ReturnFalse()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var result = await sut.Configure(null, null);

                //Assert
                result.Should().BeFalse();
            }
        }

        public class CreateTeamLeague
        {
            [Fact]
            public async void Given_PostIsOK_When_CreateTeamLeague_Then_Ok()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Post(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                //Act
                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Assert
                var command = new CreateTeamLeagueCommand();
                await sut.CreateTeamLeague(command);
            }
            [Fact]
            public void Given_PostIsNotOK_When_CreateTeamLeague_Then_CreateTeamLeagueExceptionIsThrown()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Post(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );
                //Act

                var command = new CreateTeamLeagueCommand();
                Func<Task> func = async () => await sut.CreateTeamLeague(command);

                //Assert
                func.Should().Throw<CreateTeamLeagueException>();
            }
        }

        public class GetCompetitions
        {
            [Fact]
            public async void Given_GetIsOK_When_GetCompetitions_Then_ReturnList()
            {
                //Arrange
                var list = new List<Application.Competitions.Queries.GetCompetitions.CompetitionDto>
            {
                new Application.Competitions.Queries.GetCompetitions.CompetitionDto
                {
                    Country = "England",
                    Name = "Premier League"
                },
                new Application.Competitions.Queries.GetCompetitions.CompetitionDto
                {
                    Country = "Spain",
                    Name = "Primera Division"
                }
            };

                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(list)
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetCompetitionsQuery();
                var resultList = await sut.GetCompetitions(query);

                //Assert
                resultList.ToList().Count().Should().Be(2);
            }
            [Fact]
            public async void Given_GetIsNotOK_When_GetCompetitions_Then_ReturnEmptyList()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetCompetitionsQuery();
                var resultList = await sut.GetCompetitions(query);

                //Assert
                resultList.ToList().Count().Should().Be(0);
            }
        }

        public class GetCompetition
        {
            [Fact]
            public async void Given_GetIsOK_When_GetCompetition_Then_ReturnCompetition()
            {
                //Arrange
                var dto = new Application.Competitions.Queries.GetCompetition.CompetitionDto
                {
                    Country = "England",
                    Name = "Premier League"
                };

                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(dto)
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetCompetitionQuery();
                var result = await sut.GetCompetition(query);

                //Assert
                result.Should().NotBeNull();
            }

            [Fact]
            public async void Given_GetIsNotOK_When_GetCompetition_Then_ReturnEmptyCompetition()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetCompetitionQuery();
                var result = await sut.GetCompetition(query);

                //Assert
                result.Should().NotBeNull();
                result.Name.Should().BeNull();
            }
        }

        public class GetCompetitors
        {
            [Fact]
            public async void Given_GetIsOK_When_GetCompetitors_Then_ReturnList()
            {
                //Arrange
                var list = new List<CompetitorDto>
                {
                    new CompetitorDto
                    {
                        TeamName = "Team 1"
                    },
                    new CompetitorDto
                    {
                        TeamName = "Team 2"
                    }
                };

                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(list)
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueCompetitorsQuery();
                var result = await sut.GetCompetitors(query);

                //Assert
                result.Should().NotBeNull();
                result.ToList().Count().Should().Be(2);
            }

            [Fact]
            public async void Given_GetIsNotOK_When_GetCompetitors_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueCompetitorsQuery();
                var result = await sut.GetCompetitors(query);

                //Assert
                result.Should().BeNull();
            }
        }
        
        public class GetPlayersForTeamCompetitor
        {
            [Fact]
            public async void Given_GetIsOK_When_GetPlayersForTeamCompetitor_Then_ReturnList()
            {
                //Arrange
                var list = new List<CompetitorPlayerDto>
                {
                    new CompetitorPlayerDto
                    {
                        Number = "1",
                        Player = new Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor.PlayerDto
                        {
                            FirstName = "John",
                            LastName = "Doe"
                        }
                    },
                    new CompetitorPlayerDto
                    {
                        Number = "2",
                        Player = new Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor.PlayerDto
                        {
                            FirstName = "Jane",
                            LastName = "Doe"
                        }
                    }
                };

                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(list)
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetPlayersForTeamCompetitorQuery();
                var result = await sut.GetPlayersForTeamCompetitor(query);

                //Assert
                result.Should().NotBeNull();
                result.ToList().Count().Should().Be(2);
            }

            [Fact]
            public async void Given_GetIsNotOK_When_GetPlayersForTeamCompetitor_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetPlayersForTeamCompetitorQuery();
                var result = await sut.GetPlayersForTeamCompetitor(query);

                //Assert
                result.Should().BeNull();
            }
        }

        public class GetTeamLeague
        {
            [Fact]
            public async void Given_GetIsOK_When_GetTeamLeague_Then_ReturnTeamLeagueDto()
            {
                //Arrange
                var vm = new GetTeamLeagueVm();

                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(vm)
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var result = await sut.GetTeamLeague("LeagueName");

                //Assert
                result.Should().NotBeNull();
            }
            [Fact]
            public async void Given_GetIsNotOK_When_GetTeamLeague_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var result = await sut.GetTeamLeague("LeagueName");

                //Assert
                result.Should().BeNull();
            }
        }

        public class GetTeamLeagueRounds
        {
            [Fact]
            public async void Given_GetIsOK_When_GetTeamLeagueRounds_Then_ReturnList()
            {
                //Arrange
                var list = new List<Application.TeamLeagues.Queries.GetTeamLeagueRounds.RoundDto>
            {
                new Application.TeamLeagues.Queries.GetTeamLeagueRounds.RoundDto(),
                new Application.TeamLeagues.Queries.GetTeamLeagueRounds.RoundDto(),
                new Application.TeamLeagues.Queries.GetTeamLeagueRounds.RoundDto(),
            };

                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new GetTeamLeagueRoundsVm
                    {
                        Rounds = list
                    })
                }); ; ;

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueRoundsQuery();
                var result = await sut.GetTeamLeagueRounds(query);

                //Assert
                result.Should().NotBeNull();
                result.Rounds.ToList().Count().Should().Be(3);
            }

            [Fact]
            public async void Given_GetIsNotOK_When_GetTeamLeagueRounds_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueRoundsQuery();
                var result = await sut.GetTeamLeagueRounds(query);

                //Assert
                result.Should().BeNull();
            }
        }

        public class GetTeamLeagueTable
        {
            [Fact]
            public async void Given_GetIsOK_When_GetTeamLeagueTable_Then_ReturnTable()
            {
                //Arrange
                var table = new TableDto
                {
                    Items = new List<TableItemDto>
                    {
                        new TableItemDto(),
                        new TableItemDto()
                    }
                };

                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new GetTeamLeagueTableVm
                    {
                        Table = table
                    })
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueTableQuery();
                var result = await sut.GetTeamLeagueTable(query);

                //Assert
                result.Should().NotBeNull();
                result.Table.Items.Count.Should().Be(2);
            }

            [Fact]
            public async void Given_GetIsNotOK_When_GetTeamLeagueTable_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueTableQuery();
                var result = await sut.GetTeamLeagueTable(query);

                //Assert
                result.Should().BeNull();
            }
        }

        public class GetTeamLeagueMatch
        {
            [Fact]
            public async void Given_GetIsOK_When_GetTeamLeagueMatch_Then_ReturnTeamLeagueMatchDto()
            {
                //Arrange
                var dto = new Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch.TeamMatchDto();

                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(dto)
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchQuery();
                var result = await sut.GetTeamLeagueMatch(query);

                //Assert
                result.Should().NotBeNull();
            }
            [Fact]
            public async void Given_GetIsNotOK_When_GetTeamLeagueMatch_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchQuery();
                var result = await sut.GetTeamLeagueMatch(query);

                //Assert
                result.Should().BeNull();
            }
        }

        public class GetTeamLeagueMatchDetails
        {
            [Fact]
            public async void Given_GetIsOK_When_GetTeamLeagueMatchDetails_Then_ReturnTeamMatchDto()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.TeamMatchDto())
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchDetailsQuery();
                var result = await sut.GetTeamLeagueMatchDetails(query);

                //Assert
                result.Should().NotBeNull();
            }
            [Fact]
            public async void Given_GetIsNotOK_When_GetTeamLeagueMatchDetails_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchDetailsQuery();
                var result = await sut.GetTeamLeagueMatchDetails(query);

                //Assert
                result.Should().BeNull();
            }
        }
        
        public class UpdateTeamLeagueMatch
        {
            [Fact]
            public async void Given_PutIsOK_When_UpdateTeamLeagueMatch_Then_ReturnTeamMatchDto()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch.TeamMatchDto())
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var command = new UpdateTeamLeagueMatchCommand();
                var result = await sut.UpdateTeamLeagueMatch(command);

                //Assert
                result.Should().NotBeNull();
            }
            [Fact]
            public async void Given_PutIsNotOK_When_UpdateTeamLeagueMatch_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var command = new UpdateTeamLeagueMatchCommand();
                var result = await sut.UpdateTeamLeagueMatch(command);

                //Assert
                result.Should().BeNull();
            }
        }

        public class UpdateTeamLeagueMatchScore
        {
            [Fact]
            public async void Given_PutIsOK_When_UpdateTeamLeagueMatchScore_Then_ReturnTeamMatchDto()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore.TeamMatchDto())
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var dto = new UpdateTeamLeagueMatchScoreDto();
                var result = await sut.UpdateTeamLeagueMatchScore(null, Guid.NewGuid(), dto);

                //Assert
                result.Should().NotBeNull();
            }
            [Fact]
            public async void Given_PutIsNotOK_When_UpdateTeamLeagueMatchScore_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var dto = new UpdateTeamLeagueMatchScoreDto();
                var result = await sut.UpdateTeamLeagueMatchScore(null, Guid.NewGuid(), dto);

                //Assert
                result.Should().BeNull();
            }
        }

        public class GetTeamLeagueMatchLineupEntry
        {
            [Fact]
            public async void Given_GetIsOK_When_GetTeamLeagueMatchLineupEntry_Then_ReturnLineupEntryDto()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry.LineupEntryDto())
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchLineupEntryQuery();
                var result = await sut.GetTeamLeagueMatchLineupEntry(query);

                //Assert
                result.Should().NotBeNull();
            }
            [Fact]
            public async void Given_GetIsNotOK_When_GetTeamLeagueMatchLineupEntry_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchLineupEntryQuery();
                var result = await sut.GetTeamLeagueMatchLineupEntry(query);

                //Assert
                result.Should().BeNull();
            }
        }
        
        public class UpdateTeamLeagueMatchLineupEntry
        {
            [Fact]
            public async void Given_PutIsOK_When_UpdateTeamLeagueMatchLineupEntry_Then_ReturnLineupEntryDto()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry.LineupEntryDto())
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var command = new UpdateTeamLeagueMatchLineupEntryCommand();
                var result = await sut.UpdateTeamLeagueMatchLineupEntry(command);

                //Assert
                result.Should().NotBeNull();
            }
            [Fact]
            public async void Given_PutIsNotOK_When_UpdateTeamLeagueMatchLineupEntry_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var command = new UpdateTeamLeagueMatchLineupEntryCommand();
                var result = await sut.UpdateTeamLeagueMatchLineupEntry(command);

                //Assert
                result.Should().BeNull();
            }
        }

        public class GetTeamLeagueMatchEvents
        {
            [Fact]
            public async void Given_GetIsOK_When_GetTeamLeagueMatchEvents_Then_ReturnMatchEvents()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new MatchEventsDto())
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchEventsQuery();
                var result = await sut.GetTeamLeagueMatchEvents(query);

                //Assert
                result.Should().NotBeNull();
                result.Should().BeAssignableTo<MatchEventsDto>();
            }
            [Fact]
            public async void Given_GetIsNotOK_When_GetTeamLeagueMatchEvents_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchEventsQuery();
                var result = await sut.GetTeamLeagueMatchEvents(query);

                //Assert
                result.Should().BeNull();
            }
        }

        public class GetTeamLeagueMatchGoal
        {
            [Fact]
            public async void Given_GetIsOK_When_GetTeamLeagueMatchGoal_Then_ReturnGoal()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new MatchEventsDto())
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchGoalQuery();
                var result = await sut.GetTeamLeagueMatchGoal(query);

                //Assert
                result.Should().NotBeNull();
                result.Should().BeAssignableTo<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal.GoalDto>();
            }

            [Fact]
            public async void Given_GetIsNotOK_When_GetTeamLeagueMatchGoal_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchGoalQuery();
                var result = await sut.GetTeamLeagueMatchGoal(query);

                //Assert
                result.Should().BeNull();
            }
        }

        public class UpdateTeamLeagueMatchGoal
        {
            [Fact]
            public async void Given_PutIsOK_When_UpdateTeamLeagueMatchGoal_Then_ReturnGoalDto()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal.GoalDto())
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var command = new UpdateTeamLeagueMatchGoalCommand();
                var result = await sut.UpdateTeamLeagueMatchGoal(command);

                //Assert
                result.Should().NotBeNull();
            }

            [Fact]
            public async void Given_PutIsNotOK_When_UpdateTeamLeagueMatchGoal_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var command = new UpdateTeamLeagueMatchGoalCommand();
                var result = await sut.UpdateTeamLeagueMatchGoal(command);

                //Assert
                result.Should().BeNull();
            }
        }

        public class GetTeamLeagueMatchSubstitution
        {
            [Fact]
            public async void Given_GetIsOK_When_GetTeamLeagueMatchSubstitution_Then_ReturnSubstitution()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution.SubstitutionDto())
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchSubstitutionQuery();
                var result = await sut.GetTeamLeagueMatchSubstitution(query);

                //Assert
                result.Should().NotBeNull();
                result.Should().BeAssignableTo<Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution.SubstitutionDto>();
            }

            [Fact]
            public async void Given_GetIsNotOK_When_GetTeamLeagueMatchSubstitution_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Get(
                    It.IsAny<string>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var query = new GetTeamLeagueMatchSubstitutionQuery();
                var result = await sut.GetTeamLeagueMatchSubstitution(query);

                //Assert
                result.Should().BeNull();
            }
        }

        public class UpdateTeamLeagueMatchSubstitution
        {
            [Fact]
            public async void Given_PutIsOK_When_UpdateTeamLeagueMatchSubstitution_Then_ReturnSubstitutionDto()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution.SubstitutionDto())
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var result = await sut.UpdateTeamLeagueMatchSubstitution(
                    "Premier League", Guid.NewGuid(), "Tottenham Hotspur", Guid.NewGuid(), 
                    new UpdateTeamLeagueMatchSubstitutionDto()
                );

                //Assert
                result.Should().NotBeNull();
            }

            [Fact]
            public async void Given_PutIsNotOK_When_UpdateTeamLeagueMatchSubstitution_Then_ReturnNull()
            {
                //Arrange
                var mockHttpRequestFactory = new Mock<IHttpRequestFactory>();
                mockHttpRequestFactory.Setup(x => x.Put(
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

                var mockOptions = new Mock<IOptions<ApiSettings>>();
                mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

                var sut = new CompetitionApi(
                    mockHttpRequestFactory.Object,
                    mockOptions.Object
                );

                //Act
                var result = await sut.UpdateTeamLeagueMatchSubstitution(
                    "Premier League", Guid.NewGuid(), "Tottenham Hotspur", Guid.NewGuid(),
                    new UpdateTeamLeagueMatchSubstitutionDto()
                );

                //Assert
                result.Should().BeNull();
            }
        }

    }
}