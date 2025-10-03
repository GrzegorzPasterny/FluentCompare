namespace FluentCompare
{
    internal interface IExecuteComparison<T>
    {
        ComparisonResult Compare(params T[] objects);

        ComparisonResult Compare(T t1, T t2, string t1ExprName, string t2ExprName);
    }
}
