using AutoMapper;
using FluentAssertions;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Application.AutoMapper;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class GetCompetitionsUnitTests
    {
        [Fact]
        public async void Given_CompetitionsExist_When_GetCompetitions_Then_ReturnCompetitions()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<GetCompetitionsQuery>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new CompetitionDto[]
                {
                    new CompetitionDto { Name = "Premier League", Country = "England" },
                    new CompetitionDto { Name = "Ligue 1", Country = "France" },
                    new CompetitionDto { Name = "Primera Division", Country = "Spain" },
                });
            var mockLogger = new Mock<ILogger<CompetitionController>>();

            var controller = new CompetitionController(
                mockMediator.Object,
                Mapper.CreateMapper(),
                mockLogger.Object
            );

            //Act
            var result = await controller.GetCompetitions(null);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var competitions = okResult.Value.Should().BeAssignableTo<IEnumerable<CompetitionDto>>().Subject;
            var orderedList = competitions
                .OrderBy(t => t.Name)
                .Select(t => new CompetitionDto { Name = t.Name });
            competitions.Count().Should().Be(3);
            competitions.SequenceEqual(orderedList);
        }

        [Fact]
        public async void Given_Exception_When_GetCompetitions_Then_ReturnBadRequest()
        {
            //Arrange
            var controller = new CompetitionController(null, null, null);

            //Act
            var result = await controller.GetCompetitions(null);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}