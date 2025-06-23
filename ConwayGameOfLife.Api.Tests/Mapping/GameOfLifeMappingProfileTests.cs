using AutoMapper;
using ConwayGameOfLife.Api.Mapping;
using ConwayGameOfLife.Models;
using FluentAssertions;

namespace ConwayGameOfLife.Api.Tests.Mapping;

[TestFixture]
public class GameOfLifeMappingProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GameOfLifeMappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [Test]
    public void MapListToMultiDimensionalArray()
    {
        var input = new List<List<byte>>
        {
            new() { 1, 0, 1 },
            new() { 0, 1, 0 }
        };

        var result = _mapper.Map<byte[,]>(input);

        result.Should().NotBeNull();
        result.GetLength(0).Should().Be(2);
        result.GetLength(1).Should().Be(3);

        result[0, 0].Should().Be(1);
        result[0, 1].Should().Be(0);
        result[0, 2].Should().Be(1);
        result[1, 0].Should().Be(0);
        result[1, 1].Should().Be(1);
        result[1, 2].Should().Be(0);
    }

    [Test]
    public void MapMultiDimensionalArrayToList()
    {
        var input = new byte[,]
        {
            { 1, 0, 1 },
            { 0, 1, 0 }
        };

        var result = _mapper.Map<List<List<byte>>>(input);

        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        result[0].Should().BeEquivalentTo(new List<byte> { 1, 0, 1 });
        result[1].Should().BeEquivalentTo(new List<byte> { 0, 1, 0 });
    }

    [Test]
    public void MapBoardStateToBoardStateResponse()
    {
        var boardState = new GameState
        {
            BoardId = Guid.NewGuid(),
            Population = 2,
            Generation = 1,
            Board = new byte[,]
            {
                { 1, 0 },
                { 0, 1 }
            }
        };

        var result = _mapper.Map<Dtos.GameStateResponse>(boardState);

        result.Should().NotBeNull();
        result.Board.Should().NotBeNull();
        result.BoardId.Should().Be(boardState.BoardId);
        result.Population.Should().Be(boardState.Population);
        result.Generation.Should().Be(1);
        result.Board[0][0].Should().Be(1);
        result.Board[0][1].Should().Be(0);
        result.Board[1][0].Should().Be(0);
        result.Board[1][1].Should().Be(1);
    }
}
