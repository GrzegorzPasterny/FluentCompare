public class DoubleComparisonBuilder : IComparisonBuilder<double>
{
    private ComparisonConfiguration _configuration;

    public DoubleComparisonBuilder(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ComparisonResult Compare(params double[] d)
    {
        return new DoubleComparison(_configuration)
            .Compare(d);
    }

    public ComparisonResult Compare(params double[][] d)
    {
        return new DoubleArrayComparison(_configuration)
            .Compare(d);
    }

    public ComparisonResult Compare(double d1, double d2, string? d1Expr = null, string? d2Expr = null)
    {
        return new DoubleComparison(_configuration)
            .Compare(d1, d2, d1Expr ?? "double1", d2Expr ?? "double2");
    }

    public ComparisonResult Compare(
        double[] tArr1, double[] tArr2, string? tArr1Expr = null, string? tArr2Expr = null)
    {
        return new DoubleArrayComparison(_configuration)
            .Compare(tArr1, tArr2, tArr1Expr ?? "doubleArray1", tArr2Expr ?? "doubleArray2");
    }

    public DoubleComparisonBuilder WithPrecision(int roundingPrecision)
    {
        _configuration.DoubleConfiguration.RoundingPrecision = roundingPrecision;
        _configuration.DoubleConfiguration.ToleranceMethod = DoubleToleranceMethods.Rounding;
        return this;
    }

    public DoubleComparisonBuilder WithPrecision(double epsilonPrecision)
    {
        _configuration.DoubleConfiguration.EpsilonPrecision = epsilonPrecision;
        _configuration.DoubleConfiguration.ToleranceMethod = DoubleToleranceMethods.Epsilon;
        return this;
    }

    public DoubleComparisonBuilder UseToleranceMethod(DoubleToleranceMethods doubleToleranceMethod)
    {
        _configuration.DoubleConfiguration.ToleranceMethod = doubleToleranceMethod;
        return this;
    }
}
