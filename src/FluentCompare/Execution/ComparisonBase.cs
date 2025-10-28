internal abstract class ComparisonBase<T> : IExecuteComparison<T>
{
    internal ComparisonConfiguration _comparisonConfiguration;
    internal ComparisonResult _comparisonResult;

    protected ComparisonBase(ComparisonConfiguration configuration, ComparisonResult? comparisonResult)
    {
        _comparisonConfiguration = configuration;
        _comparisonResult = comparisonResult ?? new ComparisonResult();
    }

    internal Func<T, string> _toStringFunc = i => i.ToString();

    public abstract ComparisonResult Compare(params T[] objects);
    public abstract ComparisonResult Compare(T t1, T t2, string t1ExprName, string t2ExprName);
    public abstract ComparisonResult Compare(params T[][] objects);
    public abstract ComparisonResult Compare(T[] t1, T[] t2, string t1ExprName, string t2ExprName);
}
