internal class NumericComparison<T> : NumericComparisonBase<T>, IExecuteComparison<T> where T : struct, IComparable<T>
{
    internal NumericComparison(
        ComparisonConfiguration comparisonConfiguration)
        : base(comparisonConfiguration)
    {
        _comparisonConfiguration = comparisonConfiguration;

        if (typeof(T) != typeof(short) &&
            typeof(T) != typeof(int) &&
            typeof(T) != typeof(long))
        {
            throw new NotSupportedException($"Type {typeof(T)} is not supported. Only Int16, Int32, Int64 are allowed.");
        }
    }

    public override ComparisonResult Compare(T[] ints, ComparisonResult result)
    {
        if (ints == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(T)));
            return result;
        }

        if (ints.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(ints.Length, typeof(T)));
            return result;
        }

        var first = ints[0];
        for (int i = 1; i < ints.Length; i++)
        {
            if (_comparisonConfiguration.FinishComparisonOnFirstMismatch && result.MismatchCount > 0)
            {
                return result;
            }

            if (!Compare(first, ints[i], _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches<T>.MismatchDetected(first, ints[i], i, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
        }

        return result;
    }

    public override ComparisonResult Compare(T i1, T i2, string t1ExprName, string t2ExprName, ComparisonResult result)
    {
        if (_comparisonConfiguration.FinishComparisonOnFirstMismatch && result.MismatchCount > 0)
        {
            return result;
        }

        if (!Compare(i1, i2, _comparisonConfiguration.ComparisonType))
        {
            result.AddMismatch(ComparisonMismatches<T>.MismatchDetected(i1, i2, t1ExprName, t2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
        }

        return result;
    }

    public override ComparisonResult CompareNullable(T? i1, T? i2, string t1ExprName, string t2ExprName, ComparisonResult result)
    {
        if (i1 is null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t1ExprName, typeof(T?)));
            return result;
        }

        if (i2 is null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t2ExprName, typeof(T?)));
            return result;
        }

        return Compare(i1.Value, i2.Value, t1ExprName, t2ExprName, result);
    }

    public override ComparisonResult Compare(T[][] ints, ComparisonResult result)
    {
        if (ints == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(T[])));
            return result;
        }

        if (ints.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(ints.Length, typeof(T[])));
            return result;
        }

        // All arrays are compared against the first one
        var first = ints[0];

        if (first == null)
        {
            if (_comparisonConfiguration.AllowNullComparison == false)
            {
                result.AddError(ComparisonErrors.OneOfTheObjectsIsNull<T[]>());
                return result;
            }

            result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(0, typeof(T[])));
            return result;
        }

        for (int i = 1; i < ints.Length; i++)
        {
            var current = ints[i];

            if (current == null)
            {
                if (_comparisonConfiguration.AllowNullComparison == false)
                {
                    result.AddError(ComparisonErrors.OneOfTheObjectsIsNull<T[]>());
                    return result;
                }

                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(i, typeof(T[])));
                return result;
            }

            if (first.Length != current.Length)
            {
                if (_comparisonConfiguration.AllowArrayComparisonOfDifferentLengths)
                {
                    result.AddWarning(ComparisonErrors.InputArrayLengthsDiffer(first.Length, current.Length, 0, i, typeof(T[])));
                }
                else
                {
                    result.AddError(ComparisonErrors.InputArrayLengthsDiffer(first.Length, current.Length, 0, i, typeof(T[])));
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
                    result.AddMismatch(ComparisonMismatches<T>.MismatchDetected(
                        first[j], current[j], j, 0, i, _comparisonConfiguration.ComparisonType, _toStringFunc));
                }
            }
        }

        return result;
    }

    public override ComparisonResult Compare(T[] intArr1, T[] intArr2, string intArr1ExprName, string intArr2ExprName, ComparisonResult result)
    {
        if (intArr1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(intArr1ExprName, typeof(T[])));
            return result;
        }

        if (intArr2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(intArr2ExprName, typeof(T[])));
            return result;
        }

        if (intArr1.Length != intArr2.Length)
        {
            if (_comparisonConfiguration.AllowArrayComparisonOfDifferentLengths)
            {
                result.AddWarning(ComparisonErrors.InputArrayLengthsDiffer(
                    intArr1.Length, intArr2.Length, intArr1ExprName, intArr2ExprName, typeof(T[])));
            }
            else
            {
                result.AddError(ComparisonErrors.InputArrayLengthsDiffer(
                    intArr1.Length, intArr2.Length, intArr1ExprName, intArr2ExprName, typeof(T[])));
            }
            return result;
        }

        for (int i = 0; i < intArr1.Length; i++)
        {
            if (_comparisonConfiguration.FinishComparisonOnFirstMismatch && result.MismatchCount > 0)
            {
                return result;
            }

            if (!Compare(intArr1[i], intArr2[i], _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches<T>.MismatchDetected(
                    intArr1[i], intArr2[i], i, intArr1ExprName, intArr2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
        }

        return result;
    }

    public override ComparisonResult Compare(T?[]? intArr1, T?[]? intArr2, string intArr1ExprName, string intArr2ExprName, ComparisonResult result)
    {
        if (intArr1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(intArr1ExprName, typeof(T?[])));
            return result;
        }

        if (intArr2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(intArr2ExprName, typeof(T?[])));
            return result;
        }

        if (intArr1.Length != intArr2.Length)
        {
            if (_comparisonConfiguration.AllowArrayComparisonOfDifferentLengths)
            {
                result.AddWarning(ComparisonErrors.InputArrayLengthsDiffer(
                    intArr1.Length, intArr2.Length, intArr1ExprName, intArr2ExprName, typeof(T?[])));
            }
            else
            {
                result.AddError(ComparisonErrors.InputArrayLengthsDiffer(
                    intArr1.Length, intArr2.Length, intArr1ExprName, intArr2ExprName, typeof(T?[])));
            }
            return result;
        }

        for (var i = 0; i < intArr1.Length; i++)
        {
            if (intArr1[i] is null)
            {
                result.AddError(ComparisonErrors.NullPassedAsArgument($"{intArr1ExprName}[{i}]", typeof(T?)));
                return result;
            }

            if (intArr2[i] is null)
            {
                result.AddError(ComparisonErrors.NullPassedAsArgument($"{intArr2ExprName}[{i}]", typeof(T?)));
                return result;
            }
        }

        var normalizedIntArr1 = intArr1.Select(value => value!.Value).ToArray();
        var normalizedIntArr2 = intArr2.Select(value => value!.Value).ToArray();

        return Compare(normalizedIntArr1, normalizedIntArr2, intArr1ExprName, intArr2ExprName, result);
    }

    public override ComparisonResult Compare(T?[][]? ints, ComparisonResult result)
    {
        if (ints == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(T?[])));
            return result;
        }

        if (ints.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(ints.Length, typeof(T?[])));
            return result;
        }

        var normalized = new T[ints.Length][];

        for (var i = 0; i < ints.Length; i++)
        {
            var current = ints[i];
            if (current == null)
            {
                if (_comparisonConfiguration.AllowNullComparison == false)
                {
                    result.AddError(ComparisonErrors.OneOfTheObjectsIsNull<T?[]>());
                    return result;
                }

                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(i, typeof(T?[])));
                return result;
            }

            normalized[i] = new T[current.Length];
            for (var j = 0; j < current.Length; j++)
            {
                if (current[j] is null)
                {
                    result.AddError(ComparisonErrors.NullPassedAsArgument($"Array[{i}][{j}]", typeof(T?)));
                    return result;
                }

                normalized[i][j] = current[j]!.Value;
            }
        }

        return Compare(normalized, result);
    }
}
