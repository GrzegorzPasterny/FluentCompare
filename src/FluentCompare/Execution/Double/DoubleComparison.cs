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
            Compare(doubles[i], doubles[i + 1], comparisonType, result);
        }

        return result;
    }

    private void Compare(double d1, double d2, ComparisonType comparisonType, ComparisonResult result)
    {
        bool matched;

        switch (_configuration.DoubleConfiguration.ToleranceMethod)
        {
            case DoubleToleranceMethods.Rounding:
                matched = CompareWithRounding(d1, d2, comparisonType, _configuration.DoubleConfiguration.RoundingPrecision);
                if (!matched)
                {
                    result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                        d1, d2, _configuration.DoubleConfiguration.RoundingPrecision, _configuration.DoubleConfiguration.ToleranceMethod));
                }
                break;
            case DoubleToleranceMethods.Epsilon:
                matched = CompareWithEpsilon(d1, d2, comparisonType, _configuration.DoubleConfiguration.EpsilonPrecision);
                if (!matched)
                {
                    result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                        d1, d2, _configuration.DoubleConfiguration.EpsilonPrecision, _configuration.DoubleConfiguration.ToleranceMethod));
                }
                break;
            default:
                throw new NotImplementedException();
        }
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

        Compare(d1, d2, d1ExprName, d2ExprName, comparisonType, result);

        return result;
    }

    private void Compare(double d1, double d2, string d1ExprName, string d2ExprName, ComparisonType comparisonType, ComparisonResult result)
    {
        bool matched;

        switch (_configuration.DoubleConfiguration.ToleranceMethod)
        {
            case DoubleToleranceMethods.Rounding:
                matched = CompareWithRounding(d1, d2, comparisonType, _configuration.DoubleConfiguration.RoundingPrecision);
                if (!matched)
                {
                    result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                        d1, d2, d1ExprName, d2ExprName, _configuration.DoubleConfiguration.RoundingPrecision, _configuration.DoubleConfiguration.ToleranceMethod));
                }
                break;
            case DoubleToleranceMethods.Epsilon:
                matched = CompareWithEpsilon(d1, d2, comparisonType, _configuration.DoubleConfiguration.EpsilonPrecision);
                if (!matched)
                {
                    result.AddMismatch(ComparisonMismatches.Doubles.MismatchDetected(
                        d1, d2, d1ExprName, d2ExprName, _configuration.DoubleConfiguration.EpsilonPrecision, _configuration.DoubleConfiguration.ToleranceMethod));
                }
                break;
            default:
                throw new NotImplementedException();
        }
    }
}
