internal abstract class ByteComparisonBase : ComparisonBase<byte>
{
    protected ByteComparisonBase(
        ComparisonConfiguration configuration, ComparisonResult? comparisonResult = null)
        : base(configuration, comparisonResult)
    {
    }
}
