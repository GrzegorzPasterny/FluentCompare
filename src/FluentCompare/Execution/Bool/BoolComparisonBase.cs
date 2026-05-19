internal abstract class BoolComparisonBase : ComparisonBase<bool>, IExecuteNullableComparison<bool>
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

    public abstract ComparisonResult CompareNullable(bool? t1, bool? t2, string t1ExprName, string t2ExprName, ComparisonResult result);
    public abstract ComparisonResult Compare(bool?[]? t1, bool?[]? t2, string t1ExprName, string t2ExprName, ComparisonResult result);
    public abstract ComparisonResult Compare(bool?[][]? objects, ComparisonResult result);
}
