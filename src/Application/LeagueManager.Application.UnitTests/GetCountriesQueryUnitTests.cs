using FluentAssertions;
using LeagueManager.Application.Countries.Queries.GetCountries;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Common;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class GetCountriesQueryUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<Country> countries)
        {
            var countriesDbSet = countries.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.Countries).Returns(countriesDbSet.Object);
            return mockContext;
        }

        [Fact]
        public async void Given_NoCountriesExist_When_GetCountriesIsCalled_Then_ReturnEmptyList()
        {
            // Arrange
            var countries = new List<Country>();
            var contextMock = MockDbContext(countries.AsQueryable());
            var handler = new GetCountriesQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(new GetCountriesQuery(), CancellationToken.None);

            //Assert
            result.Count().Should().Be(0);
        }

        [Fact]
        public async void Given_CountriesExist_When_GetCountries_Then_ReturnOrderedList()
        {
            // Arrange
            var countries = new List<Country> {
                new Country {  Name = "Mexico"},
                new Country {  Name = "England"},
                new Country {  Name = "Spain"}
            };
            var contextMock = MockDbContext(countries.AsQueryable());
            var handler = new GetCountriesQueryHandler(contextMock.Object);

            //Act
            var result = await handler.Handle(new GetCountriesQuery(), CancellationToken.None);

            //Assert
            var orderedList = countries
                .OrderBy(t => t.Name)
                .Select(t => new CountryDto { Name = t.Name });
            result.Count().Should().Be(3);
            result.SequenceEqual(orderedList);
        }
    }
}