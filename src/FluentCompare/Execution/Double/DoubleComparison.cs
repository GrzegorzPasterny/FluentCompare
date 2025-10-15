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

        for (int i = 0; i < doubles.Length - 1; i++)
        {
            bool matched;

            switch (_configuration.DoubleConfiguration.DoubleToleranceMethod)
            {
                case DoubleToleranceMethods.Rounding:
                    matched = CompareWithRounding(doubles[i], doubles[i + 1], comparisonType, _configuration.DoubleConfiguration.RoundingPrecision);
                    if (!matched)
                    {
                        result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                            doubles[i], doubles[i + 1], _configuration.DoubleConfiguration.RoundingPrecision));
                    }
                    break;
                case DoubleToleranceMethods.Epsilon:
                    matched = CompareWithEpsilon(doubles[i], doubles[i + 1], comparisonType, _configuration.DoubleConfiguration.EpsilonPrecision);
                    if (!matched)
                    {
                        result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                            doubles[i], doubles[i + 1], _configuration.DoubleConfiguration.EpsilonPrecision));
                    }
                    break;
                default:
                    throw new NotImplementedException();
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

        var precision = _configuration.DoubleConfiguration.RoundingPrecision;

        bool matched = CompareWithEpsilon(d1, d2, comparisonType, precision);
        if (!matched)
        {
            result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(d1, d2, d1ExprName, d2ExprName, precision));
        }

        return result;
    }
}
