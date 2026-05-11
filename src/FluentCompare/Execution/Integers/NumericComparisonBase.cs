internal abstract class NumericComparisonBase<T> : ComparisonBase<T> where T : struct, IComparable<T>
{
    protected NumericComparisonBase(
        ComparisonConfiguration comparisonConfiguration)
        : base(comparisonConfiguration)
    { }

    internal new Func<T, string> _toStringFunc = i => i.ToString()!;

    internal bool Compare(T valueA, T valueB, ComparisonType comparisonType)
    {
        // Use CompareTo instead of operators to support all T: Int16, Int32, Int64
        int cmp = valueA.CompareTo(valueB);
        return comparisonType switch
        {
            ComparisonType.EqualTo => cmp == 0,
            // TODO: Code not reached by unit tests - need to add tests for this case
            ComparisonType.NotEqualTo => cmp != 0,
            // TODO: Code not reached by unit tests - need to add tests for this case
            ComparisonType.GreaterThan => cmp > 0,
            // TODO: Code not reached by unit tests - need to add tests for this case
            ComparisonType.LessThan => cmp < 0,
            // TODO: Code not reached by unit tests - need to add tests for this case
            ComparisonType.GreaterThanOrEqualTo => cmp >= 0,
            // TODO: Code not reached by unit tests - need to add tests for this case
            ComparisonType.LessThanOrEqualTo => cmp <= 0,
            _ => throw new ArgumentOutOfRangeException(nameof(comparisonType), comparisonType, null)
        };
    }
}
