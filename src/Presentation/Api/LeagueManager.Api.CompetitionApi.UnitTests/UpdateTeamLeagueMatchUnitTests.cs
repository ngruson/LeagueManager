using AutoMapper;
using FluentAssertions;
using LeagueManager.Api.CompetitionApi.AutoMapper;
using LeagueManager.Api.CompetitionApi.Controllers;
using LeagueManager.Api.CompetitionApi.Dto;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagueMatches.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace LeagueManager.Api.CompetitionApi.UnitTests
{
    public class UpdateTeamLeagueMatchUnitTests
    {
        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile<CompetitionApiProfile>();
            });

            return config.CreateMapper();
        }

        [Fact]
        public async void Given_MatchDoesExist_When_UpdateTeamLeagueMatch_Then_ReturnOk()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            var controller = new CompetitionController(
                mockMediator.Object,
                CreateMapper());
            var dto = new UpdateTeamLeagueMatchDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatch("TeamLeague", dto);

            //Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void Given_MatchDoesNotExist_When_UpdateTeamLeagueMatch_Then_ThrowMatchNotFoundException()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new MatchNotFoundException(guid));

            var controller = new CompetitionController(
                mockMediator.Object,
                CreateMapper());
            var dto = new UpdateTeamLeagueMatchDto();

            //Act
            var result = await controller.UpdateTeamLeagueMatch("TeamLeague", dto);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be($"Match \"{guid}\" not found.");
        }

        [Fact]
        public async void Given_TeamDoesNotExist_When_UpdateTeamLeagueMatch_Then_ThrowTeamNotFoundException()
        {
            //Arrange
            var homeTeam = "Team A";
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(
                    It.IsAny<UpdateTeamLeagueMatchCommand>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new TeamNotFoundException(homeTeam));

            var controller = new CompetitionController(
                mockMediator.Object,
                CreateMapper());
            var dto = new UpdateTeamLeagueMatchDto
            {
                HomeTeam = homeTeam
            };

            //Act
            var result = await controller.UpdateTeamLeagueMatch("TeamLeague", dto);

            //Assert
            var error = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            error.Value.Should().Be($"Team \"{homeTeam}\" not found.");
        }
    }
}