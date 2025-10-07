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

        var first = ints[0];
        for (int i = 1; i < ints.Length; i++)
        {
            var current = ints[i];

            if (first == null || current == null)
            {
                if (first != current)
                {
                    result.AddMismatch(new ComparisonMismatch
                    {
                        Message = $"Array at index 0 and {i} do not match: one is null, the other is not."
                    });
                }
                continue;
            }

            if (first.Length != current.Length)
            {
                result.AddMismatch(new ComparisonMismatch
                {
                    Message = $"Array length mismatch at index 0 and {i}: {first.Length} vs {current.Length}."
                });
                continue;
            }

            for (int j = 0; j < first.Length; j++)
            {
                if (!Compare(first[j], current[j], _comparisonConfiguration.ComparisonType))
                {
                    result.AddMismatch(new ComparisonMismatch
                    {
                        Message = $"Value mismatch at index {j} of arrays 0 and {i}: {first[j]} vs {current[j]}."
                    });
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
                result.AddMismatch(new ComparisonMismatch
                {
                    Message = $"Value mismatch at index {i} of arrays {intArr1ExprName} and {intArr2ExprName}: " +
                    $"{intArr1[i]} vs {intArr2[i]}."
                });
            }
        }

        return result;
    }
}
