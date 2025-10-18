internal class DoubleArrayComparison : DoubleComparisonBase, IExecuteComparison<double[]>
{
    private ComparisonConfiguration _configuration;

    public DoubleArrayComparison(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ComparisonResult Compare(params double[][] doubleArrays)
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

    public ComparisonResult Compare(double[] dArr1, double[] dArr2, string dArr1ExprName, string dArr2ExprName)
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
            Compare(dArr1[i], dArr2[i], dArr1ExprName, dArr2ExprName, i, _configuration.ComparisonType, result);
        }

        return result;
    }

    private void Compare(double d1, double d2, string dArr1ExprName, string dArr2ExprName, int index, ComparisonType comparisonType, ComparisonResult result)
    {
        bool matched;

        switch (_configuration.DoubleConfiguration.ToleranceMethod)
        {
            case DoubleToleranceMethods.Rounding:
                matched = CompareWithRounding(d1, d2, comparisonType, _configuration.DoubleConfiguration.RoundingPrecision);
                break;
            case DoubleToleranceMethods.Epsilon:
                matched = CompareWithEpsilon(d1, d2, comparisonType, _configuration.DoubleConfiguration.EpsilonPrecision);
                break;
            default:
                throw new NotImplementedException();
        }

        if (!matched)
        {
            result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                d1, d2, dArr1ExprName, dArr2ExprName, index,
                _configuration.DoubleConfiguration.RoundingPrecision,
                _configuration.DoubleConfiguration.ToleranceMethod));
        }
    }
}
