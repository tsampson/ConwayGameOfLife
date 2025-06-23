namespace ConwayGameOfLife.Processing;

public class GameRule 
{ 
    public required Predicate<byte> Qualifier { get; set; }
    public required Predicate<int> Filter { get; set; }
    public required byte Result { get; set; }
}
