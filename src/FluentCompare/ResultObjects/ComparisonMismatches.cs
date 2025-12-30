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
            $"{t1ExprName} [{typeof(T).Name}] = {toStringFunc(v1)}, " +
            $"{t2ExprName} [{typeof(T).Name}] = {toStringFunc(v2)}, ComparisonType = {comparisonType}]");
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
    internal static ComparisonMismatch NullPassedAsArgument(string t1ExprName, string t2ExprName, Type type)
        => new(NullPassedAsArgumentCode,
            $"Null value was provided for comparison [Variable1Name = {t1ExprName}, Variable2Name = {t2ExprName}, Type = {type.Name}]");
    internal static ComparisonError NullPassedAsArgument(string t2ExprName, Type type)
        => new(NullPassedAsArgumentCode,
            $"Null value was provided for comparison [VariableName = {t2ExprName}, Type = {type.Name}]");

    /// <summary>
    /// Double-related comparison mismatches repository
    /// </summary>
    public static class Doubles
    {
        public static string Namespace = "FluentCompare.Mismatch.Double";

        public static string MismatchDetectedCode => $"{Namespace}.{nameof(MismatchDetected)}";
        internal static ComparisonMismatch MismatchDetected(double d1, double d2, int precision, DoubleToleranceMethods doubleToleranceMethods)
            => new(MismatchDetectedCode,
                $"Mismatch detected [Double1 = {d1}, Double2 = {d2}, RoundingPrecision = {precision}, ToleranceMethod = {doubleToleranceMethods}]");
        internal static ComparisonMismatch MismatchDetected(double d1, double d2, double precision, DoubleToleranceMethods doubleToleranceMethods)
            => new(MismatchDetectedCode,
                $"Mismatch detected [Double1 = {d1}, Double2 = {d2}, EpsilonPrecision = {precision}, ToleranceMethod = {doubleToleranceMethods}]");
        internal static ComparisonMismatch MismatchDetected(
            double d1, double d2, string d1ExprName, string d2ExprName, int precision, DoubleToleranceMethods doubleToleranceMethods)
             => new(MismatchDetectedCode,
                $"Mismatch detected [{d1ExprName} = {d1}, {d2ExprName} = {d2}, RoundingPrecision = {precision}, ToleranceMethod = {doubleToleranceMethods}]");
        internal static ComparisonMismatch MismatchDetected(
            double d1, double d2, string d1ExprName, string d2ExprName, double precision, DoubleToleranceMethods doubleToleranceMethods)
             => new(MismatchDetectedCode,
                $"Mismatch detected [{d1ExprName} = {d1}, {d2ExprName} = {d2}, EpsilonPrecision = {precision}, ToleranceMethod = {doubleToleranceMethods}]");
        internal static ComparisonMismatch MismatchDetected(
            double d1, double d2, string d1ArrExprName, string d2ArrExprName, int index, int precision, DoubleToleranceMethods doubleToleranceMethods)
            => new(MismatchDetectedCode,
                $"Mismatch detected [{d1ArrExprName}[{index}] = {d1}, {d2ArrExprName}[{index}] = {d2}, RoundingPrecision = {precision}, ToleranceMethod = {doubleToleranceMethods}]");
        internal static ComparisonMismatch MismatchDetected(
            double d1, double d2, string d1ArrExprName, string d2ArrExprName, int index, double precision, DoubleToleranceMethods doubleToleranceMethods)
            => new(MismatchDetectedCode,
                $"Mismatch detected [{d1ArrExprName}[{index}] = {d1}, {d2ArrExprName}[{index}] = {d2}, EpsilonPrecision = {precision}, ToleranceMethod = {doubleToleranceMethods}]");

    }

    // TODO: Add information about performed bitwise transformations
    /// <summary>
    /// Byte-related comparison mismatches repository
    /// </summary>
    public class Byte
    {
        public static string Namespace = "FluentCompare.Mismatch.Byte";
        public static string MismatchDetectedCode => $"{Namespace}.{nameof(MismatchDetected)}";
        internal static ComparisonMismatch MismatchDetected(byte b1, byte b2, byte bTransformed1, byte bTransformed2, int index, ComparisonType comparisonType, Func<byte, string> toStringFunc)
            => new(MismatchDetectedCode,
                $"Mismatch detected " +
                $"[ByteFirstValue = {toStringFunc(b1)}, AfterTransformation = {toStringFunc(bTransformed1)}, " +
                $"ByteValueAtIndex[{index}] = {toStringFunc(b2)}, AfterTransformation = {toStringFunc(bTransformed2)}, " +
                $"ComparisonType = {comparisonType}]");
        internal static ComparisonMismatch MismatchDetected(byte b1, byte b2, byte bTransformed1, byte bTransformed2, string t1ExprName, string t2ExprName, ComparisonType comparisonType, Func<byte, string> toStringFunc)
             => new(MismatchDetectedCode,
                $"Mismatch detected " +
                $"{t1ExprName} [Byte] = {toStringFunc(b1)}, After bitwise operation = {toStringFunc(bTransformed1)}, " +
                $"{t2ExprName} [Byte] = {toStringFunc(b2)}, After bitwise operation = {toStringFunc(bTransformed2)}, " +
                 $"ComparisonType = {comparisonType}]");
        internal static ComparisonMismatch MismatchDetected(byte b1, byte b2, byte bTransformed1, byte bTransformed2, int indexInArrays, int arr1Index, int arr2Index, ComparisonType comparisonType, Func<byte, string> toStringFunc)
        => new(MismatchDetectedCode,
            $"Mismatch detected " +
            $"[{typeof(byte).Name}Array[{arr1Index}][{indexInArrays}] = {toStringFunc(b1)}, After transformation = {toStringFunc(bTransformed1)}, " +
            $"{typeof(byte).Name}Array[{arr2Index}][{indexInArrays}] = {toStringFunc(b2)}, After transformation = {toStringFunc(bTransformed2)}, " +
            $"ComparisonType = {comparisonType}]",
            verboseMessage:
            $"Mismatch detected between array {arr1Index} and array {arr2Index} at element index {indexInArrays}. " +
            $"Expected both values to be {comparisonType}. " +
            $"Left value: {toStringFunc(b1)} ({typeof(byte).Name}) | " +
            $"Right value: {toStringFunc(b2)} ({typeof(byte).Name}). ");
        internal static ComparisonMismatch MismatchDetected(byte b1, byte b2, byte bTransformed1, byte bTransformed2, int indexInArrays, string intArr1ExprName, string intArr2ExprName, ComparisonType comparisonType, Func<byte, string> toStringFunc)
        => new(MismatchDetectedCode,
            $"Mismatch detected " +
            $"[{intArr1ExprName}[{indexInArrays}] = {toStringFunc(b1)}, After transformation = {toStringFunc(bTransformed1)}, " +
            $"{intArr2ExprName}[{indexInArrays}] = {toStringFunc(b2)}, After transformation = {toStringFunc(bTransformed2)}, " +
            $"ComparisonType = {comparisonType}]",
            verboseMessage:
            $"Mismatch detected between array {intArr1ExprName} and array {intArr2ExprName} at element index {indexInArrays}. " +
            $"Expected both values to be {comparisonType}. " +
            $"Left value: {toStringFunc(b1)} ({typeof(byte).Name}) | " +
            $"Right value: {toStringFunc(b2)} ({typeof(byte).Name}). ");
    }

    public static class Bool
    {
        public static string Namespace = "FluentCompare.Mismatch.Bool";
        public static string MismatchDetectedCode => $"{Namespace}.{nameof(MismatchDetected)}";
        internal static ComparisonMismatch MismatchDetected(bool b1, bool b2, int index)
            => new(MismatchDetectedCode,
                $"Mismatch detected [BoolFirstValue = {b1}, BoolValueAtIndex[{index}] = {b2}]");
        internal static ComparisonMismatch MismatchDetected(bool b1, bool b2, string t1ExprName, string t2ExprName)
             => new(MismatchDetectedCode,
                $"Mismatch detected [{t1ExprName} = {b1}, {t2ExprName} = {b2}]");
        internal static ComparisonMismatch MismatchDetected(
            bool b1, bool b2, int indexInArrays, int arr1Index, int arr2Index, ComparisonType comparisonType, Func<bool, string> toStringFunc)
        => new(MismatchDetectedCode,
            $"Mismatch detected " +
            $"[{typeof(bool).Name}Array[{arr1Index}][{indexInArrays}] = {toStringFunc(b1)}, " +
            $"{typeof(bool).Name}Array[{arr2Index}][{indexInArrays}] = {toStringFunc(b2)}, " +
            $"ComparisonType = {comparisonType}]",
            verboseMessage:
            $"Mismatch detected between array {arr1Index} and array {arr2Index} at element index {indexInArrays}. " +
            $"Expected both values to be {comparisonType}. " +
            $"Left value: {toStringFunc(b1)} ({typeof(bool).Name}) | " +
            $"Right value: {toStringFunc(b2)} ({typeof(bool).Name}). ");
        internal static ComparisonMismatch MismatchDetected(bool b1, bool b2, int indexInArrays, string intArr1ExprName, string intArr2ExprName, ComparisonType comparisonType, Func<bool, string> toStringFunc)
        => new(MismatchDetectedCode,
            $"Mismatch detected " +
            $"[{intArr1ExprName}[{indexInArrays}] = {toStringFunc(b1)}, " +
            $"{intArr2ExprName}[{indexInArrays}] = {toStringFunc(b2)}, " +
            $"ComparisonType = {comparisonType}]",
            verboseMessage:
            $"Mismatch detected between array {intArr1ExprName} and array {intArr2ExprName} at element index {indexInArrays}. " +
            $"Expected both values to be {comparisonType}. " +
            $"Left value: {toStringFunc(b1)} ({typeof(bool).Name}) | " +
            $"Right value: {toStringFunc(b2)} ({typeof(bool).Name}). ");
    }

    /// <summary>
    /// Object-related comparison mismatches repository
    /// </summary>
    public static class Object
    {
        public static string Namespace = "FluentCompare.Mismatch.Object";

        public static string MismatchDetectedByReferenceCode => $"{Namespace}.{nameof(MismatchDetectedByReference)}";
        internal static ComparisonMismatch MismatchDetectedByReference(object? o1, object? o2, int o1Index, int o2Index)
            => new(MismatchDetectedByReferenceCode,
                $"Mismatch detected by reference [Object1Index = {o1Index}, Object2Index = {o2Index}, Object1 = {o1}, Object2 = {o2}]");
        internal static ComparisonMismatch MismatchDetectedByReference(object o1, object o2, int indexInArray)
            => new(MismatchDetectedByReferenceCode,
                $"Mismatch detected by reference [ObjectsIndex = {indexInArray}, Object1 = {o1}, Object2 = {o2}]");
        internal static ComparisonMismatch MismatchDetectedByReference(object o1, object o2, string o1ExprName, string o2ExprName)
            => new(MismatchDetectedByReferenceCode,
                $"Mismatch detected by reference [Object1: Name = {o1ExprName} ToString() = {o1}, Object2: Name = {o2ExprName} ToString() = {o2}]");
        // Is it possible to know if ToString was overridden? If not, then ToString returns information about the object type

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
