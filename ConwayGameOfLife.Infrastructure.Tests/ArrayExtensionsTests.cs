using FluentAssertions;

namespace ConwayGameOfLife.Infrastructure.Tests;

[TestFixture]
public class ArrayExtensionsTests
{
    [Test]
    public void ConvertToArray_FlattensArray()
    {
        byte[,] input = new byte[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        var result = input.ConvertToArray();

        result.Should().BeEquivalentTo(new byte[] { 1, 2, 3, 4, 5, 6 }, options => options.WithStrictOrdering());
    }

    [Test]
    public void ConvertToMultiDimensionalArray_Expands()
    {
        byte[] input = [1, 2, 3, 4, 5, 6];
        int rows = 2;
        int columns = 3;

        var result = input.ConvertToMultiDimensionalArray(rows, columns);

        result.Should().BeEquivalentTo(new byte[,] { { 1, 2, 3 }, { 4, 5, 6 } });
    }
}
