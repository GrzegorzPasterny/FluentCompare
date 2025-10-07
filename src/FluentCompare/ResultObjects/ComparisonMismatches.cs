/// <summary>
/// Type-specific comparison mismatches repository
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ComparisonMismatches<T>
{
    public static string Namespace = $"FluentCompare.Mismatch.{typeof(T).Name}";

    public static string MismatchDetectedCode => $"{Namespace}.{nameof(MismatchDetected)}";
    internal static ComparisonMismatch MismatchDetected(T v1, T v2, int index, ComparisonType comparisonType, Func<T, string> toStringFunc)
        => new(MismatchDetectedCode, $"Mismatch detected " +
            $"[{typeof(T).Name}FirstValue = {toStringFunc(v1)}, " +
            $"{typeof(T).Name}ValueAtIndex[{index}] = {toStringFunc(v2)}, ComparisonType = {comparisonType}]");
    internal static ComparisonMismatch MismatchDetected(T v1, T v2, string t1ExprName, string t2ExprName, ComparisonType comparisonType, Func<T, string> toStringFunc)
         => new(MismatchDetectedCode, $"Mismatch detected " +
            $"[{typeof(T).Name}FirstValue = {toStringFunc(v1)}, " +
            $"{typeof(T).Name}SecondValue = {toStringFunc(v2)}, ComparisonType = {comparisonType}]");
    internal static ComparisonMismatch MismatchDetected(T v1, T v2, int indexInArrays, int arr1Index, int arr2Index, ComparisonType comparisonType, Func<T, string> toStringFunc)
        => new(MismatchDetectedCode,
            $"Mismatch detected " +
            $"[{typeof(T).Name}Array[{arr1Index}][{indexInArrays}] = {toStringFunc(v1)}, " +
            $"{typeof(T).Name}Array[{arr2Index}][{indexInArrays}] = {toStringFunc(v2)}, ComparisonType = {comparisonType}]",
            verboseMessage:
            $"Mismatch detected between array {arr1Index} and array {arr2Index} at element index {indexInArrays}. " +
            $"Expected both values to be {comparisonType}. " +
            $"Left value: {toStringFunc(v1)} ({typeof(T).Name}) | " +
            $"Right value: {toStringFunc(v2)} ({typeof(T).Name}). ");
    internal static ComparisonMismatch MismatchDetected(T v1, T v2, int indexInArrays, string intArr1ExprName, string intArr2ExprName, ComparisonType comparisonType, Func<T, string> toStringFunc)
        => new(MismatchDetectedCode,
            $"Mismatch detected " +
            $"[{intArr1ExprName}[{indexInArrays}] = {toStringFunc(v1)}, " +
            $"{intArr2ExprName}[{indexInArrays}] = {toStringFunc(v2)}, ComparisonType = {comparisonType}]",
            verboseMessage:
            $"Mismatch detected between array {intArr1ExprName} and array {intArr2ExprName} at element index {indexInArrays}. " +
            $"Expected both values to be {comparisonType}. " +
            $"Left value: {toStringFunc(v1)} ({typeof(T).Name}) | " +
            $"Right value: {toStringFunc(v2)} ({typeof(T).Name}). ");
}

/// <summary>
/// Generic comparison mismatches repository
/// </summary>
public static class ComparisonMismatches
{
    public static string Namespace = $"FluentCompare.Mismatch";

    public static string NullPassedAsArgumentCode => $"{Namespace}.{nameof(NullPassedAsArgument)}";
    internal static ComparisonMismatch NullPassedAsArgument(int index, Type type)
        => new(NullPassedAsArgumentCode,
            $"Null value was provided for comparison [ArgumentIndex = {index}, Type = {type.Name}]");

    /// <summary>
    /// Double-related comparison mismatches repository
    /// </summary>
    public static class Doubles
    {
        public static string Namespace = $"FluentCompare.Mismatch.Double";

        public static string MismatchDetectedCode => $"{Namespace}.{nameof(MismatchDetected)}";
        internal static ComparisonMismatch MismatchDetected(double d1, double d2, int precision)
            => new(MismatchDetectedCode,
                $"Mismatch detected [Double1 = {d1}, Double2 = {d2}, Precision = {precision}]");
        internal static ComparisonMismatch MismatchDetected(double d1, double d2, string d1ExprName, string d2ExprName, int precision)
             => new(MismatchDetectedCode,
                $"Mismatch detected [{d1ExprName} = {d1}, {d2ExprName} = {d2}, Precision = {precision}]");
        internal static ComparisonMismatch MismatchDetected(double d1, double d2, string d1ArrExprName, string d2ArrExprName, int index, int precision)
            => new(MismatchDetectedCode,
                $"Mismatch detected [{d1ArrExprName}[{index}] = {d1}, {d2ArrExprName}[{index}] = {d2}, Precision = {precision}]");

    }
}
