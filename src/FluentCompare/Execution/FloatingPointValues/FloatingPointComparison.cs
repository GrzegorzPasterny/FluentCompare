using System.Numerics;

internal class FloatingPointComparison<T> : FloatingPointComparisonBase<T> where T : struct, IFloatingPoint<T>
{
    public FloatingPointComparison(
        ComparisonConfiguration configuration)
        : base(configuration)
    { }

    public override ComparisonResult Compare(T[] floats, ComparisonResult result)
    // TODO: Code not reached by unit tests - need to add tests for this case
    {
        if (floats == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(T)));
            return result;
        }

        if (floats.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(floats.Length, typeof(double)));
            return result;
        }

        var comparisonType = _comparisonConfiguration.ComparisonType;

        if (_comparisonConfiguration.FloatConfiguration is null)
        {
            result.AddError(ComparisonErrors.ConfigurationIsMissing(typeof(double)));
            return result;
        }

        for (int i = 0; i < floats.Length - 1; i++)
        {
            Compare(floats[i], floats[i + 1], comparisonType, result);
        }

        return result;
    }

    // TODO: Code not reached by unit tests - need to add tests for this case
    public ComparisonResult CompareNullable(T? d1, T? d2, string d1ExprName, string d2ExprName, ComparisonResult result)
    {
        if (d1 is null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(d1ExprName, typeof(T?)));
            return result;
        }

        if (d2 is null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(d2ExprName, typeof(T?)));
            return result;
        }

        return Compare(d1.Value, d2.Value, d1ExprName, d2ExprName, result);
    }

    private void Compare(T d1, T d2, ComparisonType comparisonType, ComparisonResult result)
    {
        bool matched;

        switch (_comparisonConfiguration.FloatConfiguration.ToleranceMethod)
        {
            case DoubleToleranceMethods.Rounding:
                matched = CompareWithRounding(d1, d2, comparisonType, _comparisonConfiguration.FloatConfiguration.RoundingPrecision);
                break;
            case DoubleToleranceMethods.Epsilon:
                // TODO: Code not reached by unit tests - need to add tests for this case
                matched = CompareWithEpsilon(d1, d2, comparisonType, _comparisonConfiguration.FloatConfiguration.EpsilonPrecision);
                break;
            default:
                throw new NotImplementedException();
        }

        if (!matched)
        {
            result.AddMismatch(ComparisonMismatches.Floats.MismatchDetected(
                _toStringFunc(d1), _toStringFunc(d2), _comparisonConfiguration.FloatConfiguration.EpsilonPrecision, _comparisonConfiguration.FloatConfiguration.ToleranceMethod));
        }
    }

    public override ComparisonResult Compare(T d1, T d2, string d1ExprName, string d2ExprName, ComparisonResult result)
    {
        var comparisonType = _comparisonConfiguration.ComparisonType;

        if (_comparisonConfiguration.FloatConfiguration is null)
        {
            result.AddError(ComparisonErrors.ConfigurationIsMissing(typeof(T)));
            return result;
        }

        Compare(d1, d2, d1ExprName, d2ExprName, comparisonType, result);

        return result;
    }

    // TODO: Code not reached by unit tests - need to add tests for this case
    public ComparisonResult Compare(T?[]? dArr1, T?[]? dArr2, string dArr1ExprName, string dArr2ExprName, ComparisonResult result)
    {
        if (dArr1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(dArr1ExprName, typeof(T?[])));
            return result;
        }

        if (dArr2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(dArr2ExprName, typeof(T?[])));
            return result;
        }

        if (dArr1.Length != dArr2.Length)
        {
            result.AddWarning(ComparisonErrors.InputArrayLengthsDiffer(dArr1.Length, dArr2.Length, dArr1ExprName, dArr2ExprName, typeof(T?[])));
            return result;
        }

        for (int i = 0; i < dArr1.Length; i++)
        {
            if (dArr1[i] is null)
            {
                result.AddError(ComparisonErrors.NullPassedAsArgument($"{dArr1ExprName}[{i}]", typeof(T?)));
                return result;
            }

            if (dArr2[i] is null)
            {
                result.AddError(ComparisonErrors.NullPassedAsArgument($"{dArr2ExprName}[{i}]", typeof(T?)));
                return result;
            }

            Compare(dArr1[i]!.Value, dArr2[i]!.Value, dArr1ExprName, dArr2ExprName, i, _comparisonConfiguration.ComparisonType, result);
        }

        return result;
    }

    // TODO: Code not reached by unit tests - need to add tests for this case
    public ComparisonResult Compare(T?[][]? doubleArrays, ComparisonResult result)
    {
        if (doubleArrays == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(T?[])));
            return result;
        }

        if (doubleArrays.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(doubleArrays.Length, typeof(T?[])));
            return result;
        }

        var normalized = new T[doubleArrays.Length][];
        for (var i = 0; i < doubleArrays.Length; i++)
        {
            var current = doubleArrays[i];
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

    public override ComparisonResult Compare(T[][] doubleArrays, ComparisonResult result)
    {
        if (doubleArrays == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(T[])));
            return result;
        }

        // TODO: Code not reached by unit tests - need to add tests for this case
        if (doubleArrays.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(doubleArrays.Length, typeof(T[])));
            return result;
        }

        // TODO: Code not reached by unit tests - need to add tests for this case
        var first = doubleArrays[0];
        for (int i = 1; i < doubleArrays.Length; i++)
        {
            var mismatch = Compare(first, doubleArrays[i], $"doubles[0]", $"doubles[{i}]", result);
            foreach (var m in mismatch.Mismatches)
            {
                result.AddMismatch(m);
            }
        }

        return result;
    }

    public override ComparisonResult Compare(T[] dArr1, T[] dArr2, string dArr1ExprName, string dArr2ExprName, ComparisonResult result)
    {
        if (ReferenceEquals(dArr1, dArr2))
        {
            return result;
        }

        if (dArr1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(dArr1ExprName, typeof(T[])));
            return result;
        }

        if (dArr2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(dArr2ExprName, typeof(T[])));
            return result;
        }

        if (dArr1.Length != dArr2.Length)
        {
            if (_comparisonConfiguration.AllowArrayComparisonOfDifferentLengths)
            {
                result.AddWarning(ComparisonErrors.InputArrayLengthsDiffer(dArr1.Length, dArr2.Length, dArr1ExprName, dArr2ExprName, typeof(double[])));
            }
            else
            {
                result.AddMismatch(ComparisonMismatches.InputArrayLengthsDiffer(dArr1.Length, dArr2.Length, dArr1ExprName, dArr2ExprName, typeof(double[])));
            }

            // TODO: Perform the comparison in case of warning
            return result;
        }

        for (int i = 0; i < dArr1.Length; i++)
        {
            Compare(dArr1[i], dArr2[i], dArr1ExprName, dArr2ExprName, i, _comparisonConfiguration.ComparisonType, result);
        }

        return result;
    }

    private void Compare(T d1, T d2, string dArr1ExprName, string dArr2ExprName, int index, ComparisonType comparisonType, ComparisonResult result)
    {
        bool matched;

        switch (_comparisonConfiguration.FloatConfiguration.ToleranceMethod)
        {
            case DoubleToleranceMethods.Rounding:
                matched = CompareWithRounding(d1, d2, comparisonType, _comparisonConfiguration.FloatConfiguration.RoundingPrecision);
                break;
            case DoubleToleranceMethods.Epsilon:
                matched = CompareWithEpsilon(d1, d2, comparisonType, _comparisonConfiguration.FloatConfiguration.EpsilonPrecision);
                break;
            default:
                throw new NotImplementedException();
        }

        if (!matched)
        {
            result.AddMismatch(ComparisonMismatches.Floats.MismatchDetected(
                _toStringFunc(d1), _toStringFunc(d2), dArr1ExprName, dArr2ExprName, index,
                _comparisonConfiguration.FloatConfiguration.RoundingPrecision,
                _comparisonConfiguration.FloatConfiguration.ToleranceMethod));
        }
    }

    private void Compare(T d1, T d2, string d1ExprName, string d2ExprName, ComparisonType comparisonType, ComparisonResult result)
    {
        bool matched;

        switch (_comparisonConfiguration.FloatConfiguration.ToleranceMethod)
        {
            case DoubleToleranceMethods.Rounding:
                matched = CompareWithRounding(d1, d2, comparisonType, _comparisonConfiguration.FloatConfiguration.RoundingPrecision);
                if (!matched)
                {
                    result.AddMismatch(ComparisonMismatches.Floats.MismatchDetected(
                        _toStringFunc(d1), _toStringFunc(d2), d1ExprName, d2ExprName, _comparisonConfiguration.FloatConfiguration.RoundingPrecision, _comparisonConfiguration.FloatConfiguration.ToleranceMethod));
                }
                break;
            case DoubleToleranceMethods.Epsilon:
                // TODO: Code not reached by unit tests - need to add tests for this case
                matched = CompareWithEpsilon(d1, d2, comparisonType, _comparisonConfiguration.FloatConfiguration.EpsilonPrecision);
                if (!matched)
                {
                    result.AddMismatch(ComparisonMismatches.Floats.MismatchDetected(
                        _toStringFunc(d1), _toStringFunc(d2), d1ExprName, d2ExprName, _comparisonConfiguration.FloatConfiguration.EpsilonPrecision, _comparisonConfiguration.FloatConfiguration.ToleranceMethod));
                }
                break;
            default:
                throw new NotImplementedException();
        }
    }
}
