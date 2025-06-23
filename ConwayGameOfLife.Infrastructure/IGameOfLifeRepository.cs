using ConwayGameOfLife.Models;

namespace ConwayGameOfLife.Infrastructure;

public interface IGameOfLifeRepository
{
    Task<GameState?> GetGenerationOrLastBoardStateAsync(Guid id, int generation);
    Task<GameState?> GetLastBoardStateAsync(Guid id);
    Task<Guid?> GetOrCreateGameAsync(byte[,] grid);
    Task<GameState> SaveBoardStateAsync(GameState nextBoardState);
}
