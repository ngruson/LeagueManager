using FluentAssertions;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Player.Dto;
using LeagueManager.Application.TeamCompetitor.Dto;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using Lineup = LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Application.TeamLeagues.Dto;
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
using LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Commands.UpdateTeamLeagueMatchLineupEntry;

namespace LeagueManager.Infrastructure.UnitTests
{
    public class CompetitionApiUnitTests
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

        [Fact]
        public async void Given_GetIsOK_When_GetCompetitions_Then_ReturnList()
        {
            //Arrange
            var list = new List<CompetitionDto>
            {
                new CompetitionDto
                {
                    Country = "England",
                    Name = "Premier League"
                },
                new CompetitionDto
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

        [Fact]
        public async void Given_GetIsOK_When_GetCompetition_Then_ReturnCompetition()
        {
            //Arrange
            var dto = new CompetitionDto
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

        [Fact]
        public async void Given_GetIsOK_When_GetPlayersForTeamCompetitor_Then_ReturnList()
        {
            //Arrange
            var list = new List<TeamCompetitorPlayerDto>
            {
                new TeamCompetitorPlayerDto
                {
                    Number = "1",
                    Player = new PlayerDto
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    }
                },
                new TeamCompetitorPlayerDto
                {
                    Number = "2",
                    Player = new PlayerDto
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

        [Fact]
        public async void Given_GetIsOK_When_GetTeamLeagueTable_Then_ReturnTeamLeagueTableDto()
        {
            //Arrange
            var dto = new TeamLeagueTableDto();

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
            var query = new GetTeamLeagueTableQuery();
            var result = await sut.GetTeamLeagueTable(query);

            //Assert
            result.Should().NotBeNull();
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

        [Fact]
        public async void Given_GetIsOK_When_GetTeamLeagueRounds_Then_ReturnList()
        {
            //Arrange
            var list = new List<TeamLeagueRoundDto>
            {
                new TeamLeagueRoundDto(),
                new TeamLeagueRoundDto(),
                new TeamLeagueRoundDto(),
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
            var query = new GetTeamLeagueRoundsQuery();
            var result = await sut.GetTeamLeagueRounds(query);

            //Assert
            result.Should().NotBeNull();
            result.ToList().Count().Should().Be(3);
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

        [Fact]
        public async void Given_GetIsOK_When_GetTeamLeagueMatch_Then_ReturnTeamLeagueMatchDto()
        {
            //Arrange
            var dto = new TeamMatchDto();

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
                Content = new JsonContent(new TeamMatchDto())
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
                Content = new JsonContent(new TeamMatchDto())
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
                Content = new JsonContent(new TeamMatchDto())
            });

            var mockOptions = new Mock<IOptions<ApiSettings>>();
            mockOptions.SetupGet(x => x.Value).Returns(new ApiSettings());

            var sut = new CompetitionApi(
                mockHttpRequestFactory.Object,
                mockOptions.Object
            );

            //Act
            var command = new UpdateTeamLeagueMatchScoreCommand();
            var result = await sut.UpdateTeamLeagueMatchScore(command);

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
            var command = new UpdateTeamLeagueMatchScoreCommand();
            var result = await sut.UpdateTeamLeagueMatchScore(command);

            //Assert
            result.Should().BeNull();
        }

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
                Content = new JsonContent(new Lineup.LineupEntryDto())
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
                Content = new JsonContent(new Lineup.LineupEntryDto())
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
}