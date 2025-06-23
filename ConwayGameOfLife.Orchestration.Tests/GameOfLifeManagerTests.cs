using ConwayGameOfLife.Infrastructure;
using ConwayGameOfLife.Models;
using ConwayGameOfLife.Processing;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ConwayGameOfLife.Orchestration.Tests;

public class GameOfLifeManagerTests
{
    private IGameOfLifeManager _manager;
    private Mock<IGameOfLifeRepository> _repositoryMock;
    private Mock<IGameOfLifeProcessor> _processorMock;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IGameOfLifeRepository>();
        _processorMock = new Mock<IGameOfLifeProcessor>();

        var settings = new Dictionary<string, string> { {"MaxGenerations", "3" } };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        _manager = new GameOfLifeManager(_repositoryMock.Object, _processorMock.Object, configuration);
    }

    [Test]
    public async Task UploadBoardAsync_HappyPath()
    {
        var board = new byte[,] { { 1, 0 }, { 0, 0 } };
        var id = Guid.NewGuid();
        _repositoryMock.Setup(x => x.GetOrCreateGameAsync(board))
            .ReturnsAsync(id).Verifiable();

        var result = await _manager.UploadBoardAsync(board);

        result.Should().Be(id);
        _repositoryMock.VerifyAll();
    }

    [Test]
    public async Task GetNextStateAsync_HappyPath()
    {
        var id = Guid.NewGuid();
        var boardState = new GameState 
        {
            BoardId = id,
            Board = new byte[2,2],
            Generation = 1,
            Population = 2
        };
        var expectedState = new GameState
        {
            BoardId = id,
            Board = new byte[2, 2],
            Generation = 2,
            Population = 4
        };

        _repositoryMock.Setup(x => x.GetLastBoardStateAsync(id))
            .ReturnsAsync(boardState).Verifiable();
        _repositoryMock.Setup(x => x.SaveBoardStateAsync(expectedState))
            .ReturnsAsync(expectedState).Verifiable();

        _processorMock.Setup(x => x.ProcessNextState(boardState))
            .Returns(expectedState).Verifiable();

        var result = await _manager.GetNextStateAsync(id);

        result.Should().Be(expectedState);
        _repositoryMock.VerifyAll();
        _processorMock.VerifyAll();
    }

    [Test]
    public async Task GetNextStateAsync_BoardNotFound()
    {
        var id = Guid.NewGuid();

        _repositoryMock.Setup(x => x.GetLastBoardStateAsync(id))
            .ReturnsAsync((GameState?)null).Verifiable();

        var result = await _manager.GetNextStateAsync(id);

        result.Should().BeNull();
        _repositoryMock.VerifyAll();
        _processorMock.VerifyAll();
    }

    [Test]
    public async Task GetStatesAheadAsync_HappyPath()
    {
        var id = Guid.NewGuid();
        var count = 3;
        var boardState = new GameState
        {
            BoardId = id,
            Board = new byte[2, 2] {{ 0, 1 },{ 0, 1 }},
            Generation = 1,
            Population = 2
        };
        var expectedState = new GameState
        {
            BoardId = id,
            Board = new byte[2, 2] {{ 0, 0 }, { 0, 0 }},
            Generation = 3,
            Population = 0
        };

        _repositoryMock.Setup(x => x.GetGenerationOrLastBoardStateAsync(id, 3))
            .ReturnsAsync(boardState).Verifiable();
        _repositoryMock.SetupSequence(x => x.SaveBoardStateAsync(It.IsAny<GameState>()))
            .ReturnsAsync(boardState)
            .ReturnsAsync(boardState)
            .ReturnsAsync(expectedState);

        _processorMock.SetupSequence(x => x.ProcessNextState(boardState))
            .Returns(boardState)
            .Returns(boardState)
            .Returns(expectedState);

        var result = await _manager.GetGenerationAsync(id, count);

        result.Should().Be(expectedState);
        _repositoryMock.VerifyAll();
        _processorMock.VerifyAll();
    }

    [Test]
    public async Task GetStatesAheadAsync_BoardNotFound()
    {
        var id = Guid.NewGuid();
        var count = 3;

        _repositoryMock.Setup(x => x.GetGenerationOrLastBoardStateAsync(id, 3))
            .ReturnsAsync((GameState?)null).Verifiable();

        var result = await _manager.GetGenerationAsync(id, count);

        result.Should().BeNull();
        _repositoryMock.VerifyAll();
        _processorMock.VerifyAll();
    }

    [Test]
    public async Task GetFinalStateAsync()
    {
        var id = Guid.NewGuid();
        var maxGenerations = 3;
        var boardState = new GameState
        {
            BoardId = id,
            Board = new byte[2, 2],
            Generation = 1,
            Population = 2
        };
        var expectedState = new GameState
        {
            BoardId = id,
            Board = new byte[2, 2],
            Generation = maxGenerations,
            Population = 4
        };

        _repositoryMock.Setup(x => x.GetLastBoardStateAsync(id))
            .ReturnsAsync(boardState).Verifiable();
        _repositoryMock.SetupSequence(x => x.SaveBoardStateAsync(It.IsAny<GameState>()))
            .ReturnsAsync(boardState)
            .ReturnsAsync(boardState)
            .ReturnsAsync(expectedState);

        _processorMock.SetupSequence(x => x.ProcessNextState(boardState))
            .Returns(boardState)
            .Returns(boardState)
            .Returns(expectedState);

        var result = await _manager.GetFinalStateAsync(id);

        result.Should().Be(expectedState);
        _repositoryMock.VerifyAll();
        _processorMock.VerifyAll();
    }

    [Test]
    public async Task GetFinalStateAsync_BoardNotFound()
    {
        var id = Guid.NewGuid();

        _repositoryMock.Setup(x => x.GetLastBoardStateAsync(id))
            .ReturnsAsync((GameState?)null).Verifiable();

        var result = await _manager.GetFinalStateAsync(id);

        result.Should().BeNull();
        _repositoryMock.VerifyAll();
        _processorMock.VerifyAll();
    }
}