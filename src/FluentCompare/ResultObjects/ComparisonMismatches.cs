
/// <summary>
/// Type-specific comparison mismatches repository
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ComparisonMismatches<T>
{
    public static string Namespace = $"FluentCompare.Mismatch.{typeof(T).Name}";

    public static string ValuesNotMatchingCode => $"{Namespace}.{nameof(ValuesNotMatching)}";
    internal static ComparisonMismatch ValuesNotMatching(double d1, double d2, int precision)
        => new(ValuesNotMatchingCode,
            $"Values are not matching [Double1 = {d1}, Double2 = {d2}, Precision = {precision}]");
}
