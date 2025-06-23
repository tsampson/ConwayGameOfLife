using FluentValidation;

namespace ConwayGameOfLife.Api.Validation;

public class GridValidator : AbstractValidator<List<List<byte>>> 
{
    public GridValidator() 
    {
        RuleFor(grid => grid).NotNull().NotEmpty().WithMessage("Grid must be passed and have data.");
        RuleFor(grid => grid).Must(grid => grid.All(row => row.Count == grid[0].Count))
            .WithMessage("Grid must be a 2D array (not jagged).");
        RuleFor(grid => grid).Must(grid => grid.All(row => row.All(x => x == 0 || x== 1)))
            .WithMessage("Grid must only contain 0's and 1's.");
    }
}
