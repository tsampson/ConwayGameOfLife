using MongoDB.Bson.Serialization.Attributes;

namespace ConwayGameOfLife.Infrastructure.Dto;

public class GameStateDto
{
    [BsonElement("GameOfLifeId")]
    public required Guid GameOfLifeId { get; set; }
    [BsonElement("Generation")]
    public required int Generation { get; set; }
    [BsonElement("Population")]
    public required int Population { get; set; }
    [BsonElement("Board")]
    public required byte[] Board { get; set; }  
}
