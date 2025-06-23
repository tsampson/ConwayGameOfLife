namespace ConwayGameOfLife.Models;

public record GameState
{
    public required Guid BoardId { get; set; }
    public required int Generation { get; set; }
    public required int Population { get; set; }
    public required byte[,] Board { get; set; }
    public int? RepeatsFrom { get; set; }
    public int? RepeatsTo { get; set; }
}