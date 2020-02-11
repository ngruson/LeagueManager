using FluentAssertions;
using LeagueManager.Application.Sports.Queries.GetTeamSports;
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

namespace LeagueManager.Infrastructure.UnitTests
{
    public class SportApiUnitTests
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

            var sut = new SportApi(
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

            var sut = new CountryApi(
                mockHttpRequestFactory.Object,
                mockOptions.Object
            );

            //Act
            var result = await sut.Configure(null, null);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void Given_GetIsOK_When_GetTeamSports_Then_ReturnList()
        {
            //Arrange
            var list = new List<TeamSportDto>
            {
                new TeamSportDto
                {
                    Name = "Soccer"
                },
                new TeamSportDto
                {
                    Name = "Basketball"
                },
                new TeamSportDto
                {
                    Name = "Baseball"
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

            var sut = new SportApi(
                mockHttpRequestFactory.Object,
                mockOptions.Object
            );

            //Act
            var result = await sut.GetTeamSports();

            //Assert
            result.Should().NotBeNull();
            result.ToList().Count().Should().Be(3);
        }

        [Fact]
        public async void Given_GetIsNotOK_When_GetTeamSports_Then_ReturnNull()
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

            var sut = new SportApi(
                mockHttpRequestFactory.Object,
                mockOptions.Object
            );

            //Act
           var result = await sut.GetTeamSports();

            //Assert
            result.Should().BeNull();
        }
    }
}