using FluentCompare.Execution.Int;

internal class IntArrayComparison : IntComparisonBase, IExecuteComparison<int[]>
{
    private readonly ComparisonConfiguration _comparisonConfiguration;

    internal IntArrayComparison(ComparisonConfiguration comparisonConfiguration)
    {
        _comparisonConfiguration = comparisonConfiguration;
    }

    public ComparisonResult Compare(params int[][] ints)
    {
        var result = new ComparisonResult();

        if (ints == null || ints.Length < 2)
            return result;

        // All arrays are compared against the first one
        var first = ints[0];

        for (int i = 1; i < ints.Length; i++)
        {
            var current = ints[i];

            if (first == null)
            {
                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(0, typeof(int[])));
                return result;
            }

            if (current == null)
            {
                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(i, typeof(int[])));
                return result;
            }

            if (first.Length != current.Length)
            {
                result.AddError(ComparisonErrors.InputArrayLengthsDiffer(first.Length, current.Length, 0, i, typeof(int[])));
                return result;
            }

            for (int j = 0; j < first.Length; j++)
            {
                if (!Compare(first[j], current[j], _comparisonConfiguration.ComparisonType))
                {
                    result.AddMismatch(ComparisonMismatches<int>.MismatchDetected(
                        first[j], current[j], j, 0, i, _comparisonConfiguration.ComparisonType, _toStringFunc));
                }
            }
        }

        return result;
    }

    public ComparisonResult Compare(int[] intArr1, int[] intArr2, string intArr1ExprName, string intArr2ExprName)
    {
        var result = new ComparisonResult();

        for (int i = 0; i < intArr1.Length; i++)
        {
            if (!Compare(intArr1[i], intArr2[i], _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches<int>.MismatchDetected(
                    intArr1[i], intArr2[i], i, intArr1ExprName, intArr2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
        }

        return result;
    }
}
