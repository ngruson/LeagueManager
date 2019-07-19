using FluentAssertions;
using LeagueManager.Api.CountryApi.Controllers;
using LeagueManager.Application.Countries.Queries.GetCountries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CountryApi.UnitTests
{
    public class GetCountriesUnitTests
    {
        [Fact]
        public async void Given_CountriesExist_When_GetCountries_Then_ReturnCountries()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetCountriesQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new CountryDto[]
                {
                    new CountryDto { Name = "England" },
                    new CountryDto { Name = "Spain" },
                    new CountryDto { Name = "France" },
                });

            var controller = new CountryController(mockMediator.Object);

            //Act
            var result = await controller.Get();

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var countries = okResult.Value.Should().BeAssignableTo<IEnumerable<CountryDto>>().Subject;
            var orderedList = countries
                .OrderBy(t => t.Name)
                .Select(t => new CountryDto { Name = t.Name });
            countries.Count().Should().Be(3);
            countries.SequenceEqual(orderedList);
        }

        [Fact]
        public async void Given_Exception_When_GetCountries_Then_ReturnBadRequest()
        {
            //Arrange
            var controller = new CountryController(null);

            //Act
            var result = await controller.Get();

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}