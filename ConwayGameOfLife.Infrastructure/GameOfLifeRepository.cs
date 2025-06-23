using ConwayGameOfLife.Infrastructure.Dto;
using ConwayGameOfLife.Models;
using Microsoft.EntityFrameworkCore;

namespace ConwayGameOfLife.Infrastructure;

public class GameOfLifeRepository(IDbContextFactory<GameOfLifeDbContext> _contextFactory) 
    : IGameOfLifeRepository
{
    public async Task<Guid?> GetOrCreateGameAsync(byte[,] grid)
    {
        var rows = grid.GetLength(0);
        var columns = grid.GetLength(1);
        var gridArray = grid.ConvertToArray();

        using var context = _contextFactory.CreateDbContext();
        var game = await context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.StartingBoard == gridArray);

        if (game is null)
        {
            game = new GameOfLifeDto
            {
                Id = Guid.NewGuid(),
                StartingBoard = gridArray,
                Rows = rows,
                Columns = columns,
            };
            context.Games.Add(game);

            var gameState = new GameStateDto
            {
                GameOfLifeId = game.Id,
                Generation = 0,
                Population = gridArray.Count(x => x == 1),
                Board = gridArray,
            };
            context.GameStates.Add(gameState);

            await context.SaveChangesAsync();
        }

        return game.Id;
    }

    public async Task<GameState?> GetGenerationOrLastBoardStateAsync(Guid id, int generation)
    {
        using var context = _contextFactory.CreateDbContext();

        var game = await context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);

        var lastGameState = await context.GameStates
            .AsNoTracking()
            .OrderByDescending(gs => gs.Generation)
            .FirstOrDefaultAsync(gs => gs.GameOfLifeId == id);

        if (lastGameState?.Generation > generation) 
        {
            lastGameState = await context.GameStates
                .AsNoTracking()
                .FirstOrDefaultAsync(gs => gs.GameOfLifeId == id && gs.Generation == generation);
        }

        return lastGameState is null ? null : new GameState
        {
            BoardId = id,
            Generation = lastGameState.Generation,
            Population = lastGameState.Population,
            Board = lastGameState.Board.ConvertToMultiDimensionalArray(game!.Rows, game!.Columns),
            RepeatsFrom = game!.RepeatsFrom,
            RepeatsTo = game!.RepeatsTo
        };
    }

    public async Task<GameState?> GetLastBoardStateAsync(Guid id)
    {
        using var context = _contextFactory.CreateDbContext();

        var game = await context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);

        var lastGameState = await context.GameStates
            .AsNoTracking()
            .OrderByDescending(gs => gs.Generation)
            .FirstOrDefaultAsync(gs => gs.GameOfLifeId == id);

        return lastGameState is null ? null : new GameState
        {
            BoardId = id,
            Generation = lastGameState.Generation,
            Population = lastGameState.Population,
            Board = lastGameState.Board.ConvertToMultiDimensionalArray(game!.Rows, game!.Columns),
            RepeatsFrom = game!.RepeatsFrom,
            RepeatsTo = game!.RepeatsTo
        };
    }

    public async Task<GameState> SaveBoardStateAsync(GameState nextBoardState)
    {
        var nextBoardStateDto = new GameStateDto
        {
            GameOfLifeId = nextBoardState.BoardId,
            Generation = nextBoardState.Generation,
            Population = nextBoardState.Population,
            Board = nextBoardState.Board.ConvertToArray()
        };

        using var context = _contextFactory.CreateDbContext();

        var dupicateBoard = context.GameStates
            .FirstOrDefault(gs => gs.GameOfLifeId == nextBoardState.BoardId && gs.Board == nextBoardStateDto.Board);

        if (dupicateBoard != null)
        {
            var game = context.Games
                .FirstOrDefault(g => g.Id == nextBoardState.BoardId);

            game!.RepeatsFrom = dupicateBoard.Generation;
            game.RepeatsTo = nextBoardState.Generation - 1;

            context.Games.Update(game);
            await context.SaveChangesAsync();

            return nextBoardState with { RepeatsFrom = game.RepeatsFrom, RepeatsTo = game.RepeatsTo };
        }

        context.GameStates.Add(nextBoardStateDto);
        await context.SaveChangesAsync();

        return nextBoardState;
    }


}
