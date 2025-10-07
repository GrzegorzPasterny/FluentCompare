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
                result.AddMismatch(ComparisonMismatches.Doubles.ValuesNotMatching(doubles[i], doubles[i + 1], precision));
            }
        }

        return result;
    }

    public ComparisonResult Compare(double d1, double d2, string d1ExprName, string d2ExprName)
    {
        var result = new ComparisonResult();
        var comparisonType = _configuration.ComparisonType;

        if (_configuration.DoubleConfiguration is null)
        {
            result.AddError(ComparisonErrors.ConfigurationIsMissing(typeof(double)));
            return result;
        }

        var precision = _configuration.DoubleConfiguration.Precision;

        bool matched = Compare(d1, d2, comparisonType, precision);
        if (!matched)
        {
            result.AddMismatch(ComparisonMismatches.Doubles.ValuesNotMatching(d1, d2, d1ExprName, d2ExprName, precision));
        }

        return result;
    }
}
