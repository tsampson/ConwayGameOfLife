using AutoMapper;
using ConwayGameOfLife.Api.Controllers;
using ConwayGameOfLife.Api.Dtos;
using ConwayGameOfLife.Api.Mapping;
using ConwayGameOfLife.Api.Validation;
using ConwayGameOfLife.Models;
using ConwayGameOfLife.Orchestration;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ConwayGameOfLife.Api.Tests.Controllers;

public class GameOfLifeControllerTests
{
    private GameOfLifeController _controller;
    private Mock<IGameOfLifeManager> _managerMock;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GameOfLifeMappingProfile>();
        });
        var mapper = config.CreateMapper();
        _managerMock = new Mock<IGameOfLifeManager>();
        _controller = new GameOfLifeController(mapper, _managerMock.Object, new GridValidator());
    }

    [Test]
    public async Task UploadBoardAsync_HappyPath()
    {
        var board = new List<List<byte>>
        {
            new () { 1, 0 },
            new() { 0, 0 },
            new() { 0, 0 }
        };
        var id = Guid.NewGuid();
        _managerMock.Setup(x => x.UploadBoardAsync(It.IsAny<byte[,]>()))
            .ReturnsAsync(id).Verifiable();

        var result = await _controller.UploadBoardAsync(board);
        var okResult = result as OkObjectResult;

        okResult!.Value.Should().Be(id);
        okResult.StatusCode.Should().Be(200);
        _managerMock.VerifyAll();
    }

    [Test]
    public async Task UploadBoardAsync_BadRequest()
    {
        var board = new List<List<byte>>
        {
            new () { 1 },
            new() { 0, 5 }
        };

        var result = await _controller.UploadBoardAsync(board) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(400);
        result.Value.Should().NotBeNull();
    }

    [Test]
    public async Task GetNextStateAsync_HappyPath()
    {
        var id = Guid.NewGuid();
        var board = new byte[,] { { 1, 0 }, { 0, 0 } };
        var gameState = new GameState
        {
            BoardId = id,
            Generation = 1,
            Population = 1,
            Board = board
        };

        _managerMock.Setup(x => x.GetNextStateAsync(id))
            .ReturnsAsync(gameState).Verifiable();

        var result = await _controller.GetNextStateAsync(id) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(new GameStateResponse
        {
            BoardId = id,
            Generation = 1,
            Population = 1,
            Board = new List<List<byte>> { new() { 1, 0 }, new() { 0, 0 } }
        });
        _managerMock.VerifyAll();
    }


    [Test]
    public async Task GetNextStateAsync_NotFound()
    {
        var id = Guid.NewGuid();

        _managerMock.Setup(x => x.GetNextStateAsync(id))
            .ReturnsAsync((GameState?)null).Verifiable();

        var result = await _controller.GetNextStateAsync(id) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        _managerMock.VerifyAll();
    }

    [Test]
    public async Task GetStatesAheadAsync_HappyPath()
    {
        var id = Guid.NewGuid();
        var generation = 5;
        var board = new byte[,] { { 1, 0 }, { 0, 0 } };
        var expectedBoardState = new GameState
        {
            BoardId = id,
            Generation = 1,
            Population = 1,
            Board = board
        };

        _managerMock.Setup(x => x.GetGenerationAsync(id, generation))
            .ReturnsAsync(expectedBoardState).Verifiable();

        var result = await _controller.GetStatesAheadAsync(id, generation) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(new GameStateResponse
        {
            BoardId = id,
            Generation = 1,
            Population = 1,
            Board = new List<List<byte>> { new() { 1, 0 }, new() { 0, 0 } }
        });
        _managerMock.VerifyAll();
    }

    [Test]
    public async Task GetStatesAheadAsync_NotFound()
    {
        var id = Guid.NewGuid();
        var generation = 5;
        var board = new byte[,] { { 1, 0 }, { 0, 0 } };
        var expectedBoardState = new GameState
        {
            BoardId = id,
            Generation = 1,
            Population = 1,
            Board = board
        };

        _managerMock.Setup(x => x.GetGenerationAsync(id, generation))
            .ReturnsAsync((GameState?)null).Verifiable();

        var result = await _controller.GetStatesAheadAsync(id, generation) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        _managerMock.VerifyAll();
    }

    [Test]
    public async Task GetFinalStateAsync_HappyPath()
    {
        var id = Guid.NewGuid();
        var board = new byte[,] { { 1, 0 }, { 0, 0 } };
        var expectedBoardState = new GameState
        {
            BoardId = id,
            Generation = 1,
            Population = 1,
            Board = board
        };

        _managerMock.Setup(x => x.GetFinalStateAsync(id))
            .ReturnsAsync(expectedBoardState).Verifiable();

        var result = await _controller.GetFinalStateAsync(id) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(new GameStateResponse
        {
            BoardId = id,
            Generation = 1,
            Population = 1,
            Board = new List<List<byte>> { new() { 1, 0 }, new() { 0, 0 } }
        });
        _managerMock.VerifyAll();
    }
}