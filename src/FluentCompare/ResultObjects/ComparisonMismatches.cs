




/// <summary>
/// Type-specific comparison mismatches repository
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ComparisonMismatches<T>
{
    public static string Namespace = $"FluentCompare.Mismatch.{typeof(T).Name}";

}

/// <summary>
/// Generic comparison mismatches repository
/// </summary>
public static class ComparisonMismatches
{
    public static string Namespace = $"FluentCompare.Mismatch";

    /// <summary>
    /// Double-related comparison mismatches repository
    /// </summary>
    public static class Doubles
    {
        public static string Namespace = $"FluentCompare.Mismatch.Double";

        public static string ValuesNotMatchingCode => $"{Namespace}.{nameof(ValuesNotMatching)}";
        internal static ComparisonMismatch ValuesNotMatching(double d1, double d2, int precision)
            => new(ValuesNotMatchingCode,
                $"Values are not matching [Double1 = {d1}, Double2 = {d2}, Precision = {precision}]");
        internal static ComparisonMismatch ValuesNotMatching(double d1, double d2, string d1ExprName, string d2ExprName, int precision)
             => new(ValuesNotMatchingCode,
                $"Values are not matching [{d1ExprName} = {d1}, {d2ExprName} = {d2}, Precision = {precision}]");
        internal static ComparisonMismatch ValuesNotMatching(double d1, double d2, string d1ArrExprName, string d2ArrExprName, int index, int precision)
            => new(ValuesNotMatchingCode,
                $"Values are not matching [{d1ArrExprName}[{index}] = {d1}, {d2ArrExprName}[{index}] = {d2}, Precision = {precision}]");

    }
}
