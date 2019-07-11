using FluentAssertions;
using LeagueManager.Application.Countries.Queries.GetCountries;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
        [Fact]
        public async void ReturnEmptyListWhenNoCountriesExist()
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
        public async void ReturnOrderedListWhenCountriesExist()
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

        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<Country> countries)
        {
            var mockSet = new Mock<DbSet<Country>>();
            mockSet.As<IQueryable<Country>>().Setup(m => m.Provider).Returns(countries.Provider);
            mockSet.As<IQueryable<Country>>().Setup(m => m.Expression).Returns(countries.Expression);
            mockSet.As<IQueryable<Country>>().Setup(m => m.ElementType).Returns(countries.ElementType);
            mockSet.As<IQueryable<Country>>().Setup(m => m.GetEnumerator()).Returns(countries.GetEnumerator());

            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.Countries).Returns(mockSet.Object);

            return mockContext;
        }
    }
}