internal abstract class ByteComparisonBase : ComparisonBase<byte>
{
    protected ByteComparisonBase(
        ComparisonConfiguration configuration, ComparisonResult? comparisonResult = null)
        : base(configuration, comparisonResult)
    {
    }

    internal bool Compare(byte valueA, byte valueB, ComparisonType comparisonType)
    {
        int cmp = valueA.CompareTo(valueB);
        return comparisonType switch
        {
            ComparisonType.EqualTo => cmp == 0,
            ComparisonType.NotEqualTo => cmp != 0,
            ComparisonType.GreaterThan => cmp > 0,
            ComparisonType.LessThan => cmp < 0,
            ComparisonType.GreaterThanOrEqualTo => cmp >= 0,
            ComparisonType.LessThanOrEqualTo => cmp <= 0,
            _ => throw new ArgumentOutOfRangeException(nameof(comparisonType), comparisonType, null)
        };
    }
}
