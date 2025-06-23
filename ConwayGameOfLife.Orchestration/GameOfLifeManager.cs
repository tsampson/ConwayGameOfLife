using ConwayGameOfLife.Infrastructure;
using ConwayGameOfLife.Models;
using ConwayGameOfLife.Processing;
using Microsoft.Extensions.Configuration;

namespace ConwayGameOfLife.Orchestration;

public class GameOfLifeManager(
    IGameOfLifeRepository _repository,
    IGameOfLifeProcessor _processor,
    IConfiguration _configuration) 
    : IGameOfLifeManager
{
    private readonly int _maxGenerations = _configuration.GetValue<int>("MaxGenerations", 1000);


    public async Task<GameState?> GetNextStateAsync(Guid boardId)
    {
        var boardState = await _repository.GetLastBoardStateAsync(boardId);
        if (boardState == null)
        {
            return null;
        }

        return await ProcessNextState(boardState);
    }

        public async Task<GameState?> GetGenerationAsync(Guid boardId, int generation)
    {
        var boardState = await _repository.GetGenerationOrLastBoardStateAsync(boardId, generation);
        if (boardState == null)
        {
            return boardState;
        }

        while (boardState.RepeatsFrom is null && boardState.Generation <= generation && boardState.Generation <= _maxGenerations)
        {
            boardState = await ProcessNextState(boardState);
            if (boardState is null || boardState.Generation >= _maxGenerations || boardState.RepeatsFrom is not null) { break; }
        }

        return boardState;
    }

    public async Task<GameState?> GetFinalStateAsync(Guid boardId)
    {
        var boardState = await _repository.GetLastBoardStateAsync(boardId);
        if (boardState == null) { return boardState; }

        while (boardState!.RepeatsFrom is null && boardState!.Generation < _maxGenerations)
        {
            boardState = await ProcessNextState(boardState);
        }

        return boardState;
    }

    public Task<Guid?> UploadBoardAsync(byte[,] grid)
    {
        return _repository.GetOrCreateGameAsync(grid);
    }

    private async Task<GameState?> ProcessNextState(GameState boardState)
    {
        var nextBoardState = _processor.ProcessNextState(boardState);
        if (nextBoardState != null)
        {
            nextBoardState = await _repository.SaveBoardStateAsync(nextBoardState);
        }
        return nextBoardState;
    }
}
