namespace FluentCompare
{
    internal interface IExecuteNullableComparison<T>
        where T : struct
    {
        ComparisonResult Compare(T? t1, T? t2, string t1ExprName, string t2ExprName, ComparisonResult result);
        ComparisonResult Compare(T?[]? t1, T?[]? t2, string t1ExprName, string t2ExprName, ComparisonResult result);
        ComparisonResult Compare(T?[][]? objects, ComparisonResult result);
    }
}
