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

        if (ints == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(int)));
            return result;
        }

        if (ints.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectToCompare(ints.Length, typeof(int)));
            return result;
        }

        int first = ints[0];
        for (int i = 1; i <= ints.Length; i++)
        {
            if (!Compare(first, ints[i], _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches<int>.MismatchDetected(first, ints[i], i, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
        }

        return result;
    }

    public ComparisonResult Compare(int i1, int i2, string t1ExprName, string t2ExprName)
    {
        var result = new ComparisonResult();

        if (!Compare(i1, i2, _comparisonConfiguration.ComparisonType))
        {
            result.AddMismatch(ComparisonMismatches<int>.MismatchDetected(i1, i2, t1ExprName, t2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
        }

        return result;
    }
}
