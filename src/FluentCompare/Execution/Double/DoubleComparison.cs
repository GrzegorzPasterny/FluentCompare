public class DoubleComparison : DoubleComparisonBase, IExecuteComparison<double>
{
    private ComparisonConfiguration _configuration;

    public DoubleComparison(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ComparisonResult Compare(params double[] objects) => throw new NotImplementedException();
    public ComparisonResult Compare(double t1, double t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
}
