using FluentAssertions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Player;
using LeagueManager.Application.Player.Commands.CreatePlayer;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LeagueManager.Application.UnitTests
{
    public class CreatePlayerCommandUnitTests
    {
        private Mock<ILeagueManagerDbContext> MockDbContext(IQueryable<Domain.Player.Player> players)
        {
            var mockSet = players.BuildMockDbSet();
            var mockContext = new Mock<ILeagueManagerDbContext>();
            mockContext.Setup(c => c.Players).Returns(mockSet.Object);

            return mockContext;
        }

        [Fact]
        public async void Given_PlayerDoesNotExist_When_CreatePlayer_Then_PlayerIsReturned()
        {
            // Arrange
            var teams = new List<Domain.Player.Player>();
            var contextMock = MockDbContext(teams.AsQueryable());
            var handler = new CreatePlayerCommandHandler(contextMock.Object, Mapper.CreateMapper());

            //Act
            var result = await handler.Handle(new CreatePlayerCommand { 
                FirstName = "John", LastName = "Doe" 
            }, CancellationToken.None);

            //Assert
            result.Should().BeOfType<PlayerDto>();
            contextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public void Given_PlayerAlreadyExists_When_CreatePlayer_Then_ExceptionIsThrown()
        {
            // Arrange
            var teams = new List<Domain.Player.Player>
            {
                new Domain.Player.Player
                {
                    FirstName = "John",
                    LastName = "Doe"
                }
            };
            var contextMock = MockDbContext(teams.AsQueryable());
            var handler = new CreatePlayerCommandHandler(contextMock.Object, Mapper.CreateMapper());

            //Act
            var command = new CreatePlayerCommand
            {
                FirstName = "John",
                LastName = "Doe"
            };
            Func<Task> func = async () => await handler.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<PlayerAlreadyExistsException>();
        }
    }
}