internal class BoolComparison : BoolComparisonBase, IExecuteComparison<bool>
{
    public BoolComparison(
        ComparisonConfiguration comparisonConfiguration)
        : base(comparisonConfiguration)
    { }

    public override ComparisonResult Compare(bool[] bools, ComparisonResult result)
    {
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
            if (_comparisonConfiguration.FinishComparisonOnFirstMismatch && result.MismatchCount > 0)
            {
                return result;
            }

            var current = bools[i];
            if (!Compare(first, current, _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches.Bool.MismatchDetected(first, current, i));
            }
        }

        return result;
    }

    public override ComparisonResult CompareNullable(bool? b1, bool? b2, string t1ExprName, string t2ExprName, ComparisonResult result)
    {
        if (b1 is null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t1ExprName, typeof(bool?)));
            return result;
        }

        if (b2 is null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t2ExprName, typeof(bool?)));
            return result;
        }

        return Compare(b1.Value, b2.Value, t1ExprName, t2ExprName, result);
    }

    public override ComparisonResult Compare(bool b1, bool b2, string t1ExprName, string t2ExprName, ComparisonResult result)
    {
        if (_comparisonConfiguration.FinishComparisonOnFirstMismatch && result.MismatchCount > 0)
        {
            return result;
        }

        if (!Compare(b1, b2, _comparisonConfiguration.ComparisonType))
        {
            result.AddMismatch(
                ComparisonMismatches.Bool.MismatchDetected(b1, b2, t1ExprName, t2ExprName));
        }

        return result;
    }

    public override ComparisonResult Compare(bool[][] bools, ComparisonResult result)
    {
        if (bools == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(bool[])));
            return result;
        }

        if (bools.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(bools.Length, typeof(bool[])));
            return result;
        }

        // All arrays are compared against the first one
        var first = bools[0];

        if (first == null)
        {
            if (_comparisonConfiguration.AllowNullComparison == false)
            {
                result.AddError(ComparisonErrors.OneOfTheObjectsIsNull<bool[]>());
                return result;
            }

            result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(0, typeof(bool[])));
            return result;
        }

        for (int i = 1; i < bools.Length; i++)
        {
            var current = bools[i];

            if (current == null)
            {
                if (_comparisonConfiguration.AllowNullComparison == false)
                {
                    result.AddError(ComparisonErrors.OneOfTheObjectsIsNull<bool[]>());
                    return result;
                }

                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(i, typeof(bool[])));
                return result;
            }

            if (first.Length != current.Length)
            {
                if (_comparisonConfiguration.AllowArrayComparisonOfDifferentLengths)
                {
                    result.AddWarning(ComparisonErrors.InputArrayLengthsDiffer(first.Length, current.Length, 0, i, typeof(bool[])));
                }
                else
                {
                    result.AddError(ComparisonErrors.InputArrayLengthsDiffer(first.Length, current.Length, 0, i, typeof(bool[])));
                }
                return result;
            }

            for (int j = 0; j < first.Length; j++)
            {
                if (_comparisonConfiguration.FinishComparisonOnFirstMismatch && result.MismatchCount > 0)
                {
                    return result;
                }

                if (!Compare(first[j], current[j], _comparisonConfiguration.ComparisonType))
                {
                    result.AddMismatch(ComparisonMismatches.Bool.MismatchDetected(
                        first[j], current[j], j, 0, i, _comparisonConfiguration.ComparisonType, _toStringFunc));
                }
            }
        }

        return result;
    }

    public override ComparisonResult Compare(bool[] b1, bool[] b2, string t1ExprName, string t2ExprName, ComparisonResult result)
    {
        if (b1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t1ExprName, typeof(bool[])));
            return result;
        }

        if (b2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t2ExprName, typeof(bool[])));
            return result;
        }

        if (b1.Length != b2.Length)
        {
            if (_comparisonConfiguration.AllowArrayComparisonOfDifferentLengths)
            {
                result.AddWarning(ComparisonErrors.InputArrayLengthsDiffer(b1.Length, b2.Length, t1ExprName, t2ExprName, typeof(bool[])));
            }
            else
            {
                result.AddError(ComparisonErrors.InputArrayLengthsDiffer(b1.Length, b2.Length, t1ExprName, t2ExprName, typeof(bool[])));
            }
            return result;
        }

        for (int i = 0; i < b1.Length; i++)
        {
            if (_comparisonConfiguration.FinishComparisonOnFirstMismatch && result.MismatchCount > 0)
            {
                return result;
            }

            if (!Compare(b1[i], b2[i], _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches.Bool.MismatchDetected(
                    b1[i], b2[i], i, t1ExprName, t2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
        }

        return result;
    }

    public override ComparisonResult Compare(bool?[]? boolArr1, bool?[]? boolArr2, string boolArr1ExprName, string boolArr2ExprName, ComparisonResult result)
    {
        if (boolArr1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(boolArr1ExprName, typeof(bool?[])));
            return result;
        }

        if (boolArr2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(boolArr2ExprName, typeof(bool?[])));
            return result;
        }

        if (boolArr1.Length != boolArr2.Length)
        {
            if (_comparisonConfiguration.AllowArrayComparisonOfDifferentLengths)
            {
                result.AddWarning(ComparisonErrors.InputArrayLengthsDiffer(
                    boolArr1.Length, boolArr2.Length, boolArr1ExprName, boolArr2ExprName, typeof(bool?[])));
            }
            else
            {
                result.AddError(ComparisonErrors.InputArrayLengthsDiffer(
                    boolArr1.Length, boolArr2.Length, boolArr1ExprName, boolArr2ExprName, typeof(bool?[])));
            }
            return result;
        }

        for (var i = 0; i < boolArr1.Length; i++)
        {
            if (boolArr1[i] is null)
            {
                result.AddError(ComparisonErrors.NullPassedAsArgument($"{boolArr1ExprName}[{i}]", typeof(bool?)));
                return result;
            }

            if (boolArr2[i] is null)
            {
                result.AddError(ComparisonErrors.NullPassedAsArgument($"{boolArr2ExprName}[{i}]", typeof(bool?)));
                return result;
            }
        }

        var normalizedBoolArr1 = boolArr1.Select(value => value!.Value).ToArray();
        var normalizedBoolArr2 = boolArr2.Select(value => value!.Value).ToArray();

        return Compare(normalizedBoolArr1, normalizedBoolArr2, boolArr1ExprName, boolArr2ExprName, result);
    }

    public override ComparisonResult Compare(bool?[][]? bools, ComparisonResult result)
    {
        if (bools == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(bool?[])));
            return result;
        }

        if (bools.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(bools.Length, typeof(bool?[])));
            return result;
        }

        var normalized = new bool[bools.Length][];

        for (var i = 0; i < bools.Length; i++)
        {
            var current = bools[i];
            if (current == null)
            {
                if (_comparisonConfiguration.AllowNullComparison == false)
                {
                    result.AddError(ComparisonErrors.OneOfTheObjectsIsNull<bool?[]>());
                    return result;
                }

                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(i, typeof(bool?[])));
                return result;
            }

            normalized[i] = new bool[current.Length];
            for (var j = 0; j < current.Length; j++)
            {
                if (current[j] is null)
                {
                    result.AddError(ComparisonErrors.NullPassedAsArgument($"Array[{i}][{j}]", typeof(bool?)));
                    return result;
                }

                normalized[i][j] = current[j]!.Value;
            }
        }

        return Compare(normalized, result);
    }
}
