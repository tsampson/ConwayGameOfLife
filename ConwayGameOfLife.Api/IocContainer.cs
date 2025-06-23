using AutoMapper;
using ConwayGameOfLife.Api.Mapping;
using ConwayGameOfLife.Api.Validation;
using ConwayGameOfLife.Infrastructure;
using ConwayGameOfLife.Orchestration;
using ConwayGameOfLife.Processing;
using Microsoft.EntityFrameworkCore;

namespace ConwayGameOfLife.Api;

public static class IocContainer
{
    public static IServiceCollection AddGameOfLifeRegistrations(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GameOfLifeMappingProfile>();
        });
        services.AddSingleton(config.CreateMapper());
        services.AddSingleton<GridValidator>();

        services.AddScoped<IGameOfLifeManager, GameOfLifeManager>();
        services.AddScoped<IGameOfLifeProcessor, GameOfLifeProcessor>();
        services.AddScoped<IGameOfLifeRepository, GameOfLifeRepository>();

        var connectionString = configuration.GetConnectionString("GameOfLifeMongoDb");
        var databaseName = "GameOfLife";

        services.AddSingleton(new MongoDB.Driver.MongoClient(connectionString));
        services.AddScoped(provider => 
        {
            var client = provider.GetRequiredService<MongoDB.Driver.MongoClient>();
            var builder = new DbContextOptionsBuilder<GameOfLifeDbContext>()
            .UseMongoDB(client, databaseName);
            return builder.UseMongoDB(client, databaseName).Options;
        });
        services.AddScoped<IDbContextFactory<GameOfLifeDbContext>, GameOfLifeDbContextFactory>();

        return services;
    }
}
