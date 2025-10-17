internal class StringArrayComparison : StringComparisonBase, IExecuteComparison<string[]>
{
    public ComparisonResult Compare(params string[][] objects) => throw new NotImplementedException();
    public ComparisonResult Compare(string[] t1, string[] t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
}
