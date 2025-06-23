using ConwayGameOfLife.Models;

namespace ConwayGameOfLife.Orchestration;

public interface IGameOfLifeManager
{
    public Task<Guid?> UploadBoardAsync(byte[,] grid);
    public Task<GameState?> GetNextStateAsync(Guid boardId);
    public Task<GameState?> GetGenerationAsync(Guid boardId, int count);
    public Task<GameState?> GetFinalStateAsync(Guid boardId);
}