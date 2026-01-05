internal abstract class BoolComparisonBase : ComparisonBase<bool>
{
    protected BoolComparisonBase(
        ComparisonConfiguration configuration)
        : base(configuration)
    {
    }

    // True value (1) is considered greater than False value (0)
    internal bool Compare(bool valueA, bool valueB, ComparisonType comparisonType)
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
