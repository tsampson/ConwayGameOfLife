using FluentValidation.TestHelper;
using ConwayGameOfLife.Api.Validation;

namespace ConwayGameOfLife.Api.Tests.Validation;

[TestFixture]
public class GridValidatorTests
{
    private GridValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GridValidator();
    }

    [Test]
    public void GridValidation_NullOrEmpty()
    {
        List<List<byte>>? grid = new List<List<byte>>();
        var result = _validator.TestValidate(grid);
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Grid must be passed and have data.");
    }

    [Test]
    public void GridValidation_NotJaggedArray()
    {
        var grid = new List<List<byte>>
        {
            new() { 0, 1 },
            new() { 1 }
        };
        var result = _validator.TestValidate(grid);
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Grid must be a 2D array (not jagged).");
    }

    [Test]
    public void GridValidation_CorrectValues()
    {
        var grid = new List<List<byte>>
        {
            new() { 0, 2 },
            new() { 1, 0 }
        };
        var result = _validator.TestValidate(grid);
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Grid must only contain 0's and 1's.");
    }

    [Test]
    public void GridValidation_HappyPath()
    {
        var grid = new List<List<byte>>
        {
            new() { 0, 1 },
            new() { 1, 0 }
        };
        var result = _validator.TestValidate(grid);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
