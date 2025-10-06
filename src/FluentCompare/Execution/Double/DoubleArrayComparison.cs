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
        if (doubleArrays == null || doubleArrays.Length < 2)
        {
            result.AddError(new ComparisonError("At least two arrays are required for comparison."));
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

    public ComparisonResult Compare(double[] t1, double[] t2, string t1ExprName, string t2ExprName)
    {
        var result = new ComparisonResult();

        if (ReferenceEquals(t1, t2))
        {
            return result;
        }

        if (t1 == null || t2 == null)
        {
            result.AddError(new ComparisonError($"{t1ExprName} or {t2ExprName} is null."));

            return result;
        }

        if (t1.Length != t2.Length)
        {
            result.AddWarning(new ComparisonError($"Array lengths differ: {t1ExprName}.Length={t1.Length}, {t2ExprName}.Length={t2.Length}."));

            // TODO: Perform the comparison
            return result;
        }

        for (int i = 0; i < t1.Length; i++)
        {
            bool matched = Compare(
                t1[i],
                t2[i],
                _configuration.ComparisonType,
                _configuration.DoubleConfiguration.Precision
            );

            if (!matched)
            {
                result.AddMismatch(new ComparisonMismatch
                {
                    Message = $"Mismatch at index {i}: {t1ExprName}[{i}]={t1[i]}, {t2ExprName}[{i}]={t2[i]}."
                });
            }
        }

        return result;
    }
}
