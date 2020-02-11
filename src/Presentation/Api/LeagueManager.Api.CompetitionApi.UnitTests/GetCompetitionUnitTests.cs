using AutoMapper;
using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.AutoMapper;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class GetCompetitionUnitTests
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
                    new CompetitionDto { Name = "Premier League", Country = "England" }
                );

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper());

            //Act
            var result = await controller.GetCompetition("Premier League");

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var competition = okResult.Value.Should().BeAssignableTo<CompetitionDto>().Subject;
            competition.Name.Should().Be("Premier League");
        }

        [Fact]
        public async void Given_Exception_When_GetCompetition_Then_ReturnBadRequest()
        {
            //Arrange
            var controller = new CompetitionController(null, null);

            //Act
            var result = await controller.GetCompetition(null);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}