internal class ObjectArrayComparison : IExecuteComparison<object[]>
{
    private ComparisonConfiguration _configuration;

    public ObjectArrayComparison(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ComparisonResult Compare(params object[][] objects) => throw new NotImplementedException();
    public ComparisonResult Compare(object[] t1, object[] t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
}
