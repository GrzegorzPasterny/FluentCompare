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
            bool matched = CompareWithEpsilon(
                dArr1[i],
                dArr2[i],
                _configuration.ComparisonType,
                _configuration.DoubleConfiguration.RoundingPrecision
            );

            if (!matched)
            {
                result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                    dArr1[i], dArr2[i], dArr1ExprName, dArr2ExprName, i,
                    _configuration.DoubleConfiguration.RoundingPrecision,
                    _configuration.DoubleConfiguration.ToleranceMethod));
            }
        }

        return result;
    }
}
