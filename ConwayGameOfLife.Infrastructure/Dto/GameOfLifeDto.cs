using MongoDB.Bson.Serialization.Attributes;

namespace ConwayGameOfLife.Infrastructure.Dto;

public class GameOfLifeDto
{
    [BsonId]
    public required Guid Id { get; set; }
    [BsonElement("StartingBoard")]
    public required byte[] StartingBoard { get; set; }
    [BsonElement("Rows")]
    public required int Rows { get; set; }
    [BsonElement("Columns")]
    public required int Columns { get; set; }
    [BsonElement("RepeatsFrom")]
    public int? RepeatsFrom { get; set; }
    [BsonElement("RepeatsTo")]
    public int? RepeatsTo { get; set; }
}
