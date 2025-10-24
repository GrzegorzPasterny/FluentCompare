internal class DoubleComparison : DoubleComparisonBase
{
    public DoubleComparison(
        ComparisonConfiguration configuration, ComparisonResult? comparisonResult = null)
        : base(configuration, comparisonResult)
    { }

    public override ComparisonResult Compare(params double[] doubles)
    {
        var result = new ComparisonResult();

        if (doubles == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(double)));
            return result;
        }

        if (doubles.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectToCompare(doubles.Length, typeof(double)));
            return result;
        }

        var comparisonType = _comparisonConfiguration.ComparisonType;

        if (_comparisonConfiguration.DoubleConfiguration is null)
        {
            result.AddError(ComparisonErrors.ConfigurationIsMissing(typeof(double)));
            return result;
        }

        for (int i = 0; i < doubles.Length - 1; i++)
        {
            Compare(doubles[i], doubles[i + 1], comparisonType, result);
        }

        return result;
    }

    private void Compare(double d1, double d2, ComparisonType comparisonType, ComparisonResult result)
    {
        bool matched;

        switch (_comparisonConfiguration.DoubleConfiguration.ToleranceMethod)
        {
            case DoubleToleranceMethods.Rounding:
                matched = CompareWithRounding(d1, d2, comparisonType, _comparisonConfiguration.DoubleConfiguration.RoundingPrecision);
                break;
            case DoubleToleranceMethods.Epsilon:
                matched = CompareWithEpsilon(d1, d2, comparisonType, _comparisonConfiguration.DoubleConfiguration.EpsilonPrecision);
                break;
            default:
                throw new NotImplementedException();
        }

        if (!matched)
        {
            result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                d1, d2, _comparisonConfiguration.DoubleConfiguration.EpsilonPrecision, _comparisonConfiguration.DoubleConfiguration.ToleranceMethod));
        }
    }

    public override ComparisonResult Compare(double d1, double d2, string d1ExprName, string d2ExprName)
    {
        var result = new ComparisonResult();
        var comparisonType = _comparisonConfiguration.ComparisonType;

        if (_comparisonConfiguration.DoubleConfiguration is null)
        {
            result.AddError(ComparisonErrors.ConfigurationIsMissing(typeof(double)));
            return result;
        }

        Compare(d1, d2, d1ExprName, d2ExprName, comparisonType, result);

        return result;
    }

    public override ComparisonResult Compare(params double[][] doubleArrays)
    {
        var result = new ComparisonResult();

        if (doubleArrays == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(double[])));
            return result;
        }

        if (doubleArrays.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectToCompare(doubleArrays.Length, typeof(double[])));
            return result;
        }

        var first = doubleArrays[0];
        for (int i = 1; i < doubleArrays.Length; i++)
        {
            var mismatch = Compare(first, doubleArrays[i], $"doubles[0]", $"doubles[{i}]");
            foreach (var m in mismatch.Mismatches)
            {
                result.AddMismatch(m);
            }
        }

        return result;
    }

    public override ComparisonResult Compare(double[] dArr1, double[] dArr2, string dArr1ExprName, string dArr2ExprName)
    {
        var result = new ComparisonResult();

        if (ReferenceEquals(dArr1, dArr2))
        {
            return result;
        }

        if (dArr1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(dArr1ExprName, typeof(double[])));
            return result;
        }

        if (dArr2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(dArr2ExprName, typeof(double[])));
            return result;
        }

        if (dArr1.Length != dArr2.Length)
        {
            // TODO: Make it configurable to add warning, or error
            result.AddWarning(ComparisonErrors.InputArrayLengthsDiffer(dArr1.Length, dArr2.Length, dArr1ExprName, dArr2ExprName, typeof(double[])));

            // TODO: Perform the comparison in case of warning
            return result;
        }

        for (int i = 0; i < dArr1.Length; i++)
        {
            Compare(dArr1[i], dArr2[i], dArr1ExprName, dArr2ExprName, i, _comparisonConfiguration.ComparisonType, result);
        }

        return result;
    }

    private void Compare(double d1, double d2, string dArr1ExprName, string dArr2ExprName, int index, ComparisonType comparisonType, ComparisonResult result)
    {
        bool matched;

        switch (_comparisonConfiguration.DoubleConfiguration.ToleranceMethod)
        {
            case DoubleToleranceMethods.Rounding:
                matched = CompareWithRounding(d1, d2, comparisonType, _comparisonConfiguration.DoubleConfiguration.RoundingPrecision);
                break;
            case DoubleToleranceMethods.Epsilon:
                matched = CompareWithEpsilon(d1, d2, comparisonType, _comparisonConfiguration.DoubleConfiguration.EpsilonPrecision);
                break;
            default:
                throw new NotImplementedException();
        }

        if (!matched)
        {
            result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                d1, d2, dArr1ExprName, dArr2ExprName, index,
                _comparisonConfiguration.DoubleConfiguration.RoundingPrecision,
                _comparisonConfiguration.DoubleConfiguration.ToleranceMethod));
        }
    }

    private void Compare(double d1, double d2, string d1ExprName, string d2ExprName, ComparisonType comparisonType, ComparisonResult result)
    {
        bool matched;

        switch (_comparisonConfiguration.DoubleConfiguration.ToleranceMethod)
        {
            case DoubleToleranceMethods.Rounding:
                matched = CompareWithRounding(d1, d2, comparisonType, _comparisonConfiguration.DoubleConfiguration.RoundingPrecision);
                if (!matched)
                {
                    result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                        d1, d2, d1ExprName, d2ExprName, _comparisonConfiguration.DoubleConfiguration.RoundingPrecision, _comparisonConfiguration.DoubleConfiguration.ToleranceMethod));
                }
                break;
            case DoubleToleranceMethods.Epsilon:
                matched = CompareWithEpsilon(d1, d2, comparisonType, _comparisonConfiguration.DoubleConfiguration.EpsilonPrecision);
                if (!matched)
                {
                    result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                        d1, d2, d1ExprName, d2ExprName, _comparisonConfiguration.DoubleConfiguration.EpsilonPrecision, _comparisonConfiguration.DoubleConfiguration.ToleranceMethod));
                }
                break;
            default:
                throw new NotImplementedException();
        }
    }
}
