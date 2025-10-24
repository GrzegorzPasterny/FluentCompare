internal class BoolComparison : BoolComparisonBase, IExecuteComparison<bool>
{
    public BoolComparison(
        ComparisonConfiguration comparisonConfiguration, ComparisonResult? comparisonResult = null)
        : base(comparisonConfiguration, comparisonResult)
    { }

    public override ComparisonResult Compare(params bool[] objects) => throw new NotImplementedException();
    public override ComparisonResult Compare(bool t1, bool t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
    public override ComparisonResult Compare(params bool[][] objects) => throw new NotImplementedException();
    public override ComparisonResult Compare(bool[] t1, bool[] t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
}
