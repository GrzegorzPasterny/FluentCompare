using System.Runtime.CompilerServices;

namespace FluentCompare;

public interface IComparisonBuilder
{
    ComparisonResult Compare<T>(T[] t);

    public ComparisonResult Compare(object o1, object o2,
        [CallerArgumentExpression(nameof(o1))] string? o1Expr = null,
        [CallerArgumentExpression(nameof(o2))] string? o2Expr = null);

    public ComparisonResult Compare(object[] o1Arr, object[] o2Arr,
        [CallerArgumentExpression(nameof(o1Arr))] string? o1ArrExpr = null,
        [CallerArgumentExpression(nameof(o2Arr))] string? o2ArrExpr = null);

    ComparisonResult Compare<T>(T t1, T t2,
        [CallerArgumentExpression(nameof(t1))] string? t1Expr = null,
        [CallerArgumentExpression(nameof(t2))] string? t2Expr = null);
}
