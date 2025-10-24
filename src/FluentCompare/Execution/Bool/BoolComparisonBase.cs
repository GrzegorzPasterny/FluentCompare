internal abstract class BoolComparisonBase : ComparisonBase<bool>
{
    protected BoolComparisonBase(
        ComparisonConfiguration configuration, ComparisonResult? comparisonResult = null)
        : base(configuration, comparisonResult)
    {
    }
}
