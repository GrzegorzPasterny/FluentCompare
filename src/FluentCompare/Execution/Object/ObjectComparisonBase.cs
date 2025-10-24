internal abstract class ObjectComparisonBase : ComparisonBase<object>
{
    protected ObjectComparisonBase(
        ComparisonConfiguration configuration, ComparisonResult? comparisonResult = null)
        : base(configuration, comparisonResult)
    {
    }
}
