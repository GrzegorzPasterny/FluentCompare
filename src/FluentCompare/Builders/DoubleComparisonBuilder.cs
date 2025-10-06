public class DoubleComparisonBuilder : IComparisonBuilder<double>
{
    private ComparisonConfiguration _configuration;

    public DoubleComparisonBuilder(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ComparisonResult Compare(params double[] t) => throw new NotImplementedException();
    public ComparisonResult Compare(double t1, double t2, string? t1Expr = null, string? t2Expr = null) => throw new NotImplementedException();

    public DoubleComparisonBuilder WithPrecision(int precision)
    {
        _configuration.DoubleConfiguration.Precision = precision;
        return this;
    }

}
