internal class BoolComparison : BoolComparisonBase, IExecuteComparison<bool>
{
    public BoolComparison(
        ComparisonConfiguration comparisonConfiguration, ComparisonResult? comparisonResult = null)
        : base(comparisonConfiguration, comparisonResult)
    { }

    public override ComparisonResult Compare(bool[] bools)
    {
        var result = new ComparisonResult();

        if (bools == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(bool)));
            return result;
        }

        if (bools.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(bools.Length, typeof(bool)));
            return result;
        }

        var first = bools[0];

        for (int i = 1; i < bools.Length; i++)
        {
            var current = bools[i];
            if (!Compare(first, current, _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches.Bool.MismatchDetected(first, current, i));
            }
        }

        return result;
    }

    public override ComparisonResult Compare(bool b1, bool b2, string t1ExprName, string t2ExprName)
    {
        var result = new ComparisonResult();

        if (!Compare(b1, b2, _comparisonConfiguration.ComparisonType))
        {
            result.AddMismatch(
                ComparisonMismatches.Bool.MismatchDetected(b1, b2, t1ExprName, t2ExprName));
        }

        return result;
    }

    public override ComparisonResult Compare(bool[][] bools)
    {
        var result = new ComparisonResult();

        if (bools == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(bool)));
            return result;
        }

        if (bools.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(bools.Length, typeof(bool)));
            return result;
        }

        // All arrays are compared against the first one
        var first = bools[0];

        for (int i = 1; i < bools.Length; i++)
        {
            var current = bools[i];

            if (first == null)
            {
                if (_comparisonConfiguration.AllowNullComparison == false)
                {
                    result.AddError(ComparisonErrors.OneOfTheObjectsIsNull<bool>());
                    return result;
                }

                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(0, typeof(bool[])));
                return result;
            }

            if (current == null)
            {
                if (_comparisonConfiguration.AllowNullComparison == false)
                {
                    result.AddError(ComparisonErrors.OneOfTheObjectsIsNull<bool>());
                    return result;
                }

                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(i, typeof(bool[])));
                return result;
            }

            if (first.Length != current.Length)
            {
                result.AddError(ComparisonErrors.InputArrayLengthsDiffer(first.Length, current.Length, 0, i, typeof(bool[])));
                return result;
            }

            for (int j = 0; j < first.Length; j++)
            {
                if (!Compare(first[j], current[j], _comparisonConfiguration.ComparisonType))
                {
                    result.AddMismatch(ComparisonMismatches.Bool.MismatchDetected(
                        first[j], current[j], j, 0, i, _comparisonConfiguration.ComparisonType, _toStringFunc));
                }
            }
        }

        return result;
    }

    public override ComparisonResult Compare(bool[] b1, bool[] b2, string t1ExprName, string t2ExprName)
    {
        var result = new ComparisonResult();

        if (b1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(bool[])));
            return result;
        }

        if (b2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(bool[])));
            return result;
        }

        if (b1.Length != b2.Length)
        {
            result.AddError(ComparisonErrors.InputArrayLengthsDiffer(b1.Length, b2.Length, t1ExprName, t2ExprName, typeof(bool[])));
            return result;
        }

        for (int i = 0; i < b1.Length; i++)
        {
            if (!Compare(b1[i], b2[i], _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches.Bool.MismatchDetected(
                    b1[i], b2[i], i, t1ExprName, t2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
        }

        return result;
    }
}
