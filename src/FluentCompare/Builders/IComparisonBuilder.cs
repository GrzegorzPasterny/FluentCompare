public interface IComparisonBuilder<T>
{
    ComparisonResult Compare(params T[] t);
    ComparisonResult Compare(T t1, T t2,
        string? t1Expr = null,
        string? t2Expr = null);
}
