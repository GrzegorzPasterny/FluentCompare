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
    public static string Namespace = "FluentCompare.Mismatch";

    public static string NullPassedAsArgumentCode => $"{Namespace}.{nameof(NullPassedAsArgument)}";
    internal static ComparisonMismatch NullPassedAsArgument(int index, Type type)
        => new(NullPassedAsArgumentCode,
            $"Null value was provided for comparison [ArgumentIndex = {index}, Type = {type.Name}]");

    /// <summary>
    /// Double-related comparison mismatches repository
    /// </summary>
    public static class Doubles
    {
        public static string Namespace = "FluentCompare.Mismatch.Double";

        public static string MismatchDetectedCode => $"{Namespace}.{nameof(MismatchDetected)}";
        internal static ComparisonMismatch MismatchDetected(double d1, double d2, int precision, DoubleToleranceMethods doubleToleranceMethods)
            => new(MismatchDetectedCode,
                $"Mismatch detected [Double1 = {d1}, Double2 = {d2}, Precision = {precision}, ToleranceMethod = {doubleToleranceMethods}]");
        internal static ComparisonMismatch MismatchDetected(double d1, double d2, double precision, DoubleToleranceMethods doubleToleranceMethods)
            => new(MismatchDetectedCode,
                $"Mismatch detected [Double1 = {d1}, Double2 = {d2}, Precision = {precision}, ToleranceMethod = {doubleToleranceMethods}]");
        internal static ComparisonMismatch MismatchDetected(double d1, double d2, string d1ExprName, string d2ExprName, int precision)
             => new(MismatchDetectedCode,
                $"Mismatch detected [{d1ExprName} = {d1}, {d2ExprName} = {d2}, Precision = {precision}]");
        internal static ComparisonMismatch MismatchDetected(double d1, double d2, string d1ArrExprName, string d2ArrExprName, int index, int precision)
            => new(MismatchDetectedCode,
                $"Mismatch detected [{d1ArrExprName}[{index}] = {d1}, {d2ArrExprName}[{index}] = {d2}, Precision = {precision}]");

    }

    /// <summary>
    /// Object-related comparison mismatches repository
    /// </summary>
    public static class Object
    {
        public static string Namespace = "FluentCompare.Mismatch.Object";

        public static string MismatchDetectedByReferenceCode => $"{Namespace}.{nameof(MismatchDetectedByReference)}";
        internal static ComparisonMismatch MismatchDetectedByReference(object o1, object o2, int o1Index, int o2Index)
            => new(MismatchDetectedByReferenceCode,
                $"Mismatch detected by reference [Object1Index = {o1Index}, Object2Index = {o2Index}, Object1 = {o1}, Object2 = {o2}]");
        internal static ComparisonMismatch MismatchDetectedByReference(object o1, object o2, int indexInArray)
            => new(MismatchDetectedByReferenceCode,
                $"Mismatch detected by reference [ObjectsIndex = {indexInArray}, Object1 = {o1}, Object2 = {o2}]");
        internal static ComparisonMismatch MismatchDetectedByReference(object o1, object o2, string o1ExprName, string o2ExprName)
            => new(MismatchDetectedByReferenceCode,
                $"Mismatch detected by reference [Object1: Name = {o1ExprName} Value = {o1}, Object2: Name = {o2ExprName} Value = {o2}]");

        public static string MismatchDetectedByNullCode => $"{Namespace}.{nameof(MismatchDetectedByNull)}";
        internal static ComparisonMismatch MismatchDetectedByNull(object? o1, object? o2, int o1Index, int o2Index)
            => new(MismatchDetectedByNullCode,
                $"Mismatch detected by null [Object1Index = {o1Index}, Object2Index = {o2Index}, Object1 = {o1}, Object2 = {o2}]",
                verboseMessage:
                $"Null mismatch detected between objects at index {o1Index} and {o2Index}. " +
                $"Left value: {(o1 is null ? "null" : o1.ToString())} | " +
                $"Right value: {(o2 is null ? "null" : o2.ToString())}.");

        public static string MismatchDetectedByTypeCode => $"{Namespace}.{nameof(MismatchDetectedByType)}";
        internal static ComparisonMismatch MismatchDetectedByType(object o1, object o2, int o1Index, int o2Index, Type o1Type, Type o2Type)
            => new(MismatchDetectedByTypeCode,
                $"Mismatch detected by type [Object1Index = {o1Index}, Object2Index = {o2Index}, Object1Type = {o1Type.Name}, Object2Type = {o2Type.Name}]",
                verboseMessage:
                $"Type mismatch detected between objects at index {o1Index} and {o2Index}. " +
                $"Left value: {o1} (Type: {o1Type.FullName}) | " +
                $"Right value: {o2} (Type: {o2Type.FullName}).");
        internal static ComparisonMismatch MismatchDetectedByType(object o1, object o2, string o1ExprName, string o2ExprName, Type o1Type, Type o2Type)
            => new(MismatchDetectedByTypeCode,
                $"Mismatch detected by type [Object1: Name = {o1ExprName} Type = {o1Type.Name} Value = {o1}, Object2: Name = {o2ExprName} Type =  {o2Type.Name} Value = {o2}]",
                verboseMessage:
                $"Type mismatch detected between objects {o1ExprName} and {o2ExprName}. " +
                $"Left value: {o1} (Type: {o1Type.FullName}) | " +
                $"Right value: {o2} (Type: {o2Type.FullName}).");
    }
}
