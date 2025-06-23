namespace ConwayGameOfLife.Infrastructure;

public static class ArrayExtensions
{
    public static byte[] ConvertToArray(this byte[,] src)
    {
        byte[] array = new byte[src.Length];
        Buffer.BlockCopy(src, 0, array, 0, src.Length);
        return array;
    }

    public static byte[,] ConvertToMultiDimensionalArray(this byte[] src, int rows, int columns)
    {
        byte[,] array = new byte[rows, columns];
        Buffer.BlockCopy(src, 0, array, 0, src.Length);
        return array;
    }
}
