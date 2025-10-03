using FluentCompare.Configuration;
using FluentCompare.Execution.Int;

public class IntComparison : IntComparisonBase, IExecuteComparison<int>
{
    private readonly ComparisonConfiguration _comparisonConfiguration;

    internal IntComparison(ComparisonConfiguration comparisonConfiguration)
    {
        _comparisonConfiguration = comparisonConfiguration;
    }

    public ComparisonResult Compare(params int[] ints)
    {
        var result = new ComparisonResult();
        if (ints == null || ints.Length < 2)
        {
            return result;
        }

        for (int i = 0; i < ints.Length - 1; i++)
        {
            if (!Compare(ints[i], ints[i + 1], _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(new ComparisonMismatch
                {
                    Message = $"Comparison failed between {ints[i]} and {ints[i + 1]} using {_comparisonConfiguration.ComparisonType}."
                });
            }
        }

        return result;
    }

    public ComparisonResult Compare(int t1, int t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
}
