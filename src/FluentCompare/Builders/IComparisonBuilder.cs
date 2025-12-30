using System.Runtime.CompilerServices;

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
        string? t1Expr = null,
        string? t2Expr = null);

    //ComparisonResult Compare<T>(T[][] t);

    //ComparisonResult Compare<T>(T[] tArr1, T[] tArr2,
    //    string? tArr1Expr = null,
    //    string? tArr2Expr = null);
}
