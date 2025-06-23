namespace ConwayGameOfLife.Api.Dtos;

public class GameStateResponse
{
    public Guid BoardId { get; set; }
    public int Generation { get; set; }
    public int Population { get; set; }
    public List<List<byte>>? Board { get; set; }
    public int? RepeatsFrom { get; set; }
    public int? RepeatsTo { get; set; }
}
