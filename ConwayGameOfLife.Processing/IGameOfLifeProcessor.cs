using ConwayGameOfLife.Models;

namespace ConwayGameOfLife.Processing;

public interface IGameOfLifeProcessor
{
    GameState ProcessNextState(GameState boardState);
}
