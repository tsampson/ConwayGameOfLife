using ConwayGameOfLife.Models;

namespace ConwayGameOfLife.Processing;

public class GameOfLifeProcessor : IGameOfLifeProcessor
{
    private GameRules _gameRules;

    public GameOfLifeProcessor()
    {
        _gameRules = ConwayGameOfLifeRules;
    }

    public static GameRules ConwayGameOfLifeRules => new GameRules
    {
        Rules = new []
        {
            new GameRule
            {
                Qualifier = isLiving => isLiving == 1,
                Filter = livingNeighbors => livingNeighbors < 2,
                Result = 0
            }, //Any live cell with fewer than two live neighbours dies, as if by underpopulation.
            new GameRule
            {
                Qualifier = isLiving => isLiving == 1,
                Filter = livingNeighbors => livingNeighbors == 2 || livingNeighbors == 3,
                Result = 1
            }, //Any live cell with two or three live neighbours lives on to the next generation.
            new GameRule
            {
                Qualifier = isLiving => isLiving == 1,
                Filter = livingNeighbors => livingNeighbors > 3,
                Result = 0
            }, //Any live cell with more than three live neighbours dies, as if by overpopulation.
            new GameRule
            {
                Qualifier = isLiving => isLiving == 0,
                Filter = livingNeighbors => livingNeighbors == 3,
                Result = 1
            } //Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
        }
    };

    public GameState ProcessNextState(GameState boardState)
    {
        var oldBoard = boardState.Board;
        var width = oldBoard.GetLength(0);
        var height = oldBoard.GetLength(1);
        var newBoard = new byte[width, height];

        var newPopulation = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var wasLiving = oldBoard[x, y];
                var livingNeighbors = CountLivingNeighbors(oldBoard, x, y);

                var applicableRule = _gameRules.Rules.FirstOrDefault(rule => 
                    rule.Qualifier(wasLiving) && 
                    rule.Filter(livingNeighbors));

                var willLive = applicableRule?.Result ?? wasLiving;
                if (willLive == 1)
                {
                    ++newPopulation;
                }

                newBoard[x, y] = willLive;
            }
        }

        return new GameState
        {
            BoardId = boardState.BoardId,
            Generation = boardState.Generation + 1,
            Population = newPopulation,
            Board = newBoard
        };
    }

    public int CountLivingNeighbors(byte[,] oldBoard, int x, int y)
    {
        var width = oldBoard.GetLength(0);
        var height = oldBoard.GetLength(1);

        var isTopEdge = y == 0;
        var isRightEdge = x == width - 1;
        var isBottomEdge = y == height - 1;
        var isLeftEdge = x == 0;

        var count = 0;

        if (!isTopEdge && oldBoard[x, y - 1] == 1) ++count;
        if (!isRightEdge && oldBoard[x + 1, y] == 1) ++count;
        if (!isBottomEdge && oldBoard[x, y + 1] == 1) ++count;
        if (!isLeftEdge && oldBoard[x - 1, y] == 1) ++count;
        if (!isTopEdge && !isLeftEdge && oldBoard[x - 1, y - 1] == 1) ++count;
        if (!isTopEdge && !isRightEdge && oldBoard[x + 1, y - 1] == 1) ++count;
        if (!isBottomEdge && !isRightEdge && oldBoard[x + 1, y + 1] == 1) ++count;
        if (!isBottomEdge && !isLeftEdge && oldBoard[x - 1, y + 1] == 1) ++count;

        return count;
    }
}
