internal class ByteComparison : ByteComparisonBase, IExecuteComparison<byte>
{
    private ComparisonConfiguration _configuration;

    public ByteComparison(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ComparisonResult Compare(params byte[] objects) => throw new NotImplementedException();
    public ComparisonResult Compare(byte t1, byte t2, string t1ExprName, string t2ExprName) => throw new NotImplementedException();
}
