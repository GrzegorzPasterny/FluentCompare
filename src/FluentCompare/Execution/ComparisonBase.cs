internal abstract class ComparisonBase<T> : IExecuteComparison<T>
{
    internal ComparisonConfiguration _comparisonConfiguration;

    protected ComparisonBase(ComparisonConfiguration configuration)
    {
        _comparisonConfiguration = configuration;
    }

    // boxing possible value types to object? to handle nullability without exceptions
    // boxing overhead should be negligible
    internal Func<T, string> _toStringFunc = i => ((object?)i)?.ToString() ?? "null";

    public abstract ComparisonResult Compare(T[] objects, ComparisonResult result);
    public abstract ComparisonResult Compare(T t1, T t2, string t1ExprName, string t2ExprName, ComparisonResult result);
    public abstract ComparisonResult Compare(T[][] objects, ComparisonResult result);
    public abstract ComparisonResult Compare(T[] t1, T[] t2, string t1ExprName, string t2ExprName, ComparisonResult result);
}
