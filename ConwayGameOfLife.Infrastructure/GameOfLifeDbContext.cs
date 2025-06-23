using ConwayGameOfLife.Infrastructure.Dto;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ConwayGameOfLife.Infrastructure;

public class GameOfLifeDbContext : DbContext
{
    public GameOfLifeDbContext(DbContextOptions<GameOfLifeDbContext> options) 
        : base(options) 
    {
        this.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }

    public DbSet<GameOfLifeDto> Games { get; init; }
    public DbSet<GameStateDto> GameStates { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GameOfLifeDto>(entity => 
        {
            entity.HasKey(g => g.Id);
            entity.HasIndex(g => g.StartingBoard)
                .IsUnique();
            entity.ToCollection("Games");
        });

        modelBuilder.Entity<GameStateDto>(entity =>
        {
            entity.HasKey(nameof(GameStateDto.GameOfLifeId), nameof(GameStateDto.Board));
            entity.ToCollection("GameStates");
        });
    }
}

public class GameOfLifeDbContextFactory(DbContextOptions<GameOfLifeDbContext> _options)
    : IDbContextFactory<GameOfLifeDbContext>
{
    public GameOfLifeDbContext CreateDbContext()
    {
        return new GameOfLifeDbContext(_options);
    }
}