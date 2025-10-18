internal class BoolComparison : BoolComparisonBase, IExecuteComparison<bool>
{
    private readonly ComparisonConfiguration _comparisonConfiguration;

    public BoolComparison(ComparisonConfiguration comparisonConfiguration)
    {
        _comparisonConfiguration = comparisonConfiguration;
    }

    public ComparisonResult Compare(params bool[] objects) => throw new NotImplementedException();
    public ComparisonResult Compare(bool t1, bool t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
}
