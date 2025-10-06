namespace FluentCompare.Execution.Object;
internal class ObjectArrayComparison : IExecuteComparison<int[]>
{
    public ComparisonResult Compare(params int[][] objects) => throw new NotImplementedException();
    public ComparisonResult Compare(int[] t1, int[] t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
}
