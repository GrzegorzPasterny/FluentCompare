using FluentCompare.Execution.Int;

internal class NumericComparison<T> : NumericComparisonBase<T>, IExecuteComparison<T> where T : struct, IComparable<T>
{
    internal NumericComparison(
        ComparisonConfiguration comparisonConfiguration, ComparisonResult? comparisonResult = null)
        : base(comparisonConfiguration, comparisonResult)
    {
        _comparisonConfiguration = comparisonConfiguration;

        if (typeof(T) != typeof(short) &&
            typeof(T) != typeof(int) &&
            typeof(T) != typeof(long))
        {
            throw new NotSupportedException($"Type {typeof(T)} is not supported. Only Int16, Int32, Int64 are allowed.");
        }
    }

    public override ComparisonResult Compare(T[] ints)
    {
        var result = new ComparisonResult();

        if (ints == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(int)));
            return result;
        }

        if (ints.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(ints.Length, typeof(int)));
            return result;
        }

        var first = ints[0];
        for (int i = 1; i <= ints.Length; i++)
        {
            if (!Compare(first, ints[i], _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches<T>.MismatchDetected(first, ints[i], i, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
        }

        return result;
    }

    public override ComparisonResult Compare(T i1, T i2, string t1ExprName, string t2ExprName)
    {
        var result = new ComparisonResult();

        if (!Compare(i1, i2, _comparisonConfiguration.ComparisonType))
        {
            result.AddMismatch(ComparisonMismatches<T>.MismatchDetected(i1, i2, t1ExprName, t2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
        }

        return result;
    }

    public override ComparisonResult Compare(T[][] ints)
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
                    result.AddMismatch(ComparisonMismatches<T>.MismatchDetected(
                        first[j], current[j], j, 0, i, _comparisonConfiguration.ComparisonType, _toStringFunc));
                }
            }
        }

        return result;
    }

    public override ComparisonResult Compare(T[] intArr1, T[] intArr2, string intArr1ExprName, string intArr2ExprName)
    {
        var result = new ComparisonResult();

        if (intArr1.Length != intArr2.Length)
        {
            result.AddError(ComparisonErrors.InputArrayLengthsDiffer(
                intArr1.Length, intArr2.Length, intArr1ExprName, intArr2ExprName, typeof(int[])));
            return result;
        }

        for (int i = 0; i < intArr1.Length; i++)
        {
            if (!Compare(intArr1[i], intArr2[i], _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches<T>.MismatchDetected(
                    intArr1[i], intArr2[i], i, intArr1ExprName, intArr2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
        }

        return result;
    }
}
