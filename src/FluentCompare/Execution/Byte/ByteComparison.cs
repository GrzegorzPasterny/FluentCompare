internal class ByteComparison : ByteComparisonBase, IExecuteComparison<byte>
{
    public ByteComparison(
        ComparisonConfiguration configuration, ComparisonResult? comparisonResult = null)
        : base(configuration, comparisonResult)
    { }

    public override ComparisonResult Compare(params byte[] objects) => throw new NotImplementedException();
    public override ComparisonResult Compare(byte t1, byte t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
    public override ComparisonResult Compare(params byte[][] objects) => throw new NotImplementedException();
    public override ComparisonResult Compare(byte[] t1, byte[] t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
}
