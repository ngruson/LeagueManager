using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Sports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Persistence.EntityFramework.UnitTests
{
    public class DbInitializerUnitTests
    {
        private List<Country> CreateCountries()
        {
            return new List<Country>
            {
                new Country { Name = "England" },
                new Country { Name = "Ireland" },
                new Country { Name = "Netherlands" }
            };
        }

        private List<TeamSports> CreateTeamSports()
        {
            return new List<TeamSports>
            {
                new TeamSports { Name = "Soccer" },
                new TeamSports { Name = "Basketball" }
            };
        }

        private List<Team> CreateTeams()
        {
            return new List<Team>
            {
                new Team { Name = "Tottenham Hotspur" },
                new Team { Name = "Chelsea" }
            };
        }

        private Mock<ILeagueManagerDbContext> MockDbContext(
            DbSet<Country> countries,
            DbSet<TeamSports> teamSports,
            DbSet<Team> teams)
        {
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.Countries).Returns(countries);
            mockContext.Setup(c => c.TeamSports).Returns(teamSports);
            mockContext.Setup(c => c.Teams).Returns(teams);

            return mockContext;
        }

        [Fact]
        public async void Given_NoCountriesExist_When_Initialize_Then_CountriesAreAdded()
        {
            //Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockImageFileLoader = new Mock<IImageFileLoader>();

            var mockCountries = new List<Country>().AsQueryable().BuildMockDbSet();
            var mockTeamSports = CreateTeamSports().AsQueryable().BuildMockDbSet();
            var mockTeams = new List<Team>().AsQueryable().BuildMockDbSet();
            var mockDbContext = MockDbContext(
                mockCountries.Object,
                mockTeamSports.Object,
                mockTeams.Object
            );

            var sut = new DbInitializer(
                mockConfig.Object,
                mockImageFileLoader.Object
            );

            //Act
            await sut.Initialize(mockDbContext.Object);


            //Assert
            mockDbContext.Verify(x => x.EnsureCreated());
            mockCountries.Verify(x => x.AddRange(It.IsAny<Country[]>()));
            mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void Given_CountriesExist_When_Initialize_Then_NoCountriesAreAdded()
        {
            //Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockImageFileLoader = new Mock<IImageFileLoader>();

            var mockCountries = new List<Country>
                { 
                    new Country { Name = "England" },
                    new Country { Name = "Ireland" }
                }
                .AsQueryable().BuildMockDbSet();
            var mockTeamSports = CreateTeamSports().AsQueryable().BuildMockDbSet();
            var mockTeams = new List<Team>().AsQueryable().BuildMockDbSet();
            var mockDbContext = MockDbContext(
                mockCountries.Object,
                mockTeamSports.Object,
                mockTeams.Object
            );

            var sut = new DbInitializer(
                mockConfig.Object,
                mockImageFileLoader.Object
            );

            //Act
            await sut.Initialize(mockDbContext.Object);


            //Assert
            mockDbContext.Verify(x => x.EnsureCreated());
            mockCountries.Verify(x => x.AddRange(It.IsAny<Country[]>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async void Given_NoTeamSportsExist_When_Initialize_Then_TeamSportsAreAdded()
        {
            //Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockImageFileLoader = new Mock<IImageFileLoader>();

            var mockCountries = CreateCountries().AsQueryable().BuildMockDbSet();
            var mockTeamSports = new List<TeamSports>().AsQueryable().BuildMockDbSet();
            var mockTeams = new List<Team>().AsQueryable().BuildMockDbSet();
            var mockDbContext = MockDbContext(
                mockCountries.Object,
                mockTeamSports.Object,
                mockTeams.Object
            );

            var sut = new DbInitializer(
                mockConfig.Object,
                mockImageFileLoader.Object
            );

            //Act
            await sut.Initialize(mockDbContext.Object);


            //Assert
            mockDbContext.Verify(x => x.EnsureCreated());
            mockTeamSports.Verify(x => x.Add(It.IsAny<TeamSports>()));
            mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void Given_TeamSportsExist_When_Initialize_Then_NoTeamSportsAreAdded()
        {
            //Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockImageFileLoader = new Mock<IImageFileLoader>();

            var mockCountries = CreateCountries().AsQueryable().BuildMockDbSet();
            var mockTeamSports = CreateTeamSports().AsQueryable().BuildMockDbSet();
            var mockTeams = new List<Team>().AsQueryable().BuildMockDbSet();
            var mockDbContext = MockDbContext(
                mockCountries.Object,
                mockTeamSports.Object,
                mockTeams.Object
            );

            var sut = new DbInitializer(
                mockConfig.Object,
                mockImageFileLoader.Object
            );

            //Act
            await sut.Initialize(mockDbContext.Object);


            //Assert
            mockDbContext.Verify(x => x.EnsureCreated());
            mockTeamSports.Verify(x => x.Add(It.IsAny<TeamSports>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async void Given_NoTeamsExist_When_Initialize_Then_TeamsAreAdded()
        {
            //Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.SetupGet(x => x[It.Is<string>(s => s == "ASPNETCORE_ENVIRONMENT")]).Returns("Development");
            var mockImageFileLoader = new Mock<IImageFileLoader>();

            var mockCountries = CreateCountries().AsQueryable().BuildMockDbSet();
            var mockTeamSports = CreateTeamSports().AsQueryable().BuildMockDbSet();
            var mockTeams = new List<Team>().AsQueryable().BuildMockDbSet();
            var mockDbContext = MockDbContext(
                mockCountries.Object,
                mockTeamSports.Object,
                mockTeams.Object
            );

            var sut = new DbInitializer(
                mockConfig.Object,
                mockImageFileLoader.Object
            );

            //Act
            await sut.Initialize(mockDbContext.Object);


            //Assert
            mockDbContext.Verify(x => x.EnsureCreated());
            mockTeams.Verify(x => x.AddRange(It.IsAny<Team[]>()));
            mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void Given_TeamsExist_When_Initialize_Then_NoTeamsAreAdded()
        {
            //Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.SetupGet(x => x[It.Is<string>(s => s == "ASPNETCORE_ENVIRONMENT")]).Returns("Development");
            var mockImageFileLoader = new Mock<IImageFileLoader>();

            var mockCountries = CreateCountries().AsQueryable().BuildMockDbSet();
            var mockTeamSports = CreateTeamSports().AsQueryable().BuildMockDbSet();
            var mockTeams = CreateTeams().AsQueryable().BuildMockDbSet();
            var mockDbContext = MockDbContext(
                mockCountries.Object,
                mockTeamSports.Object,
                mockTeams.Object
            );

            var sut = new DbInitializer(
                mockConfig.Object,
                mockImageFileLoader.Object
            );

            //Act
            await sut.Initialize(mockDbContext.Object);


            //Assert
            mockDbContext.Verify(x => x.EnsureCreated());
            mockTeams.Verify(x => x.AddRange(It.IsAny<Team[]>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}