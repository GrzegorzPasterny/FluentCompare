namespace FluentCompare
{
    public interface IExecuteComparison<T>
    {
        ComparisonResult Compare(params T[] objects);
    }
}
