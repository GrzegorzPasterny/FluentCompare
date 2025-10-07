public class DoubleComparison : DoubleComparisonBase, IExecuteComparison<double>
{
    private ComparisonConfiguration _configuration;

    public DoubleComparison(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ComparisonResult Compare(params double[] doubles)
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

        var comparisonType = _configuration.ComparisonType;

        if (_configuration.DoubleConfiguration is null)
        {
            result.AddError(ComparisonErrors.ConfigurationIsMissing(typeof(double)));
            return result;
        }

        var precision = _configuration.DoubleConfiguration.Precision;

        for (int i = 0; i < doubles.Length - 1; i++)
        {
            bool matched = Compare(doubles[i], doubles[i + 1], comparisonType, precision);
            if (!matched)
            {
                result.AddMismatch(ComparisonMismatches<double>.ValuesNotMatching(doubles[i], doubles[i + 1], precision));
            }
        }

        return result;
    }

    public ComparisonResult Compare(double t1, double t2, string t1ExprName, string t2ExprName)
    {
        var result = new ComparisonResult();
        var comparisonType = _configuration.ComparisonType;

        if (_configuration.DoubleConfiguration is null)
        {
            result.AddError(new ComparisonError("Double comparison configuration is missing"));
            return result;
        }

        var precision = _configuration.DoubleConfiguration.Precision;

        bool matched = Compare(t1, t2, comparisonType, precision);
        if (!matched)
        {
            result.AddMismatch(new ComparisonMismatch
            {
                Message = $"Comparison failed between {t1ExprName} ({t1}) and {t2ExprName} ({t2}) with type {comparisonType} and precision {precision}."
            });
        }

        return result;
    }
}
