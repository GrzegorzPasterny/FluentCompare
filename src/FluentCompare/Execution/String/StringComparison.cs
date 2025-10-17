internal class StringComparison : StringComparisonBase, IExecuteComparison<string>
{
    private ComparisonConfiguration _configuration;

    public StringComparison(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ComparisonResult Compare(params string[] objects) => throw new NotImplementedException();
    public ComparisonResult Compare(string t1, string t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
}
