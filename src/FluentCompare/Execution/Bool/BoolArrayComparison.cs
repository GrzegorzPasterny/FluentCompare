internal class BoolArrayComparison : BoolComparisonBase, IExecuteComparison<bool[]>
{
    private ComparisonConfiguration _configuration;

    public BoolArrayComparison(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ComparisonResult Compare(params bool[][] objects) => throw new NotImplementedException();
    public ComparisonResult Compare(bool[] t1, bool[] t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
}
