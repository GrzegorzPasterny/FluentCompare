
/// <summary>
/// Generic comparison errors repository
/// </summary>
public static class ComparisonErrors
{
    public static string Namespace = "FluentCompare.Error";

    /// <summary>
    /// Code for error indicating that not enough objects were provided for comparison
    /// </summary>
    public static string NotEnoughObjectsToCompareCode => $"{Namespace}.{nameof(NotEnoughObjectsToCompare)}";
    internal static ComparisonError NotEnoughObjectsToCompare(int length, Type type)
        => new(NotEnoughObjectsToCompareCode,
            $"At least two values are required for comparison [Type = {type.Name}");

    public static string NullPassedAsArgumentCode => $"{Namespace}.{nameof(NullPassedAsArgument)}";
    internal static ComparisonError NullPassedAsArgument(Type type)
        => new(NullPassedAsArgumentCode, $"Null value was provided for comparison [Type = {type.Name}]");
    internal static ComparisonError NullPassedAsArgument(string expressionName, Type type)
        => new(NullPassedAsArgumentCode, $"Null value was provided for comparison [VariableName = {expressionName}, Type = {type.Name}]");

    public static string ConfigurationIsMissingCode => $"{Namespace}.{nameof(ConfigurationIsMissing)}";
    internal static ComparisonError ConfigurationIsMissing(Type type)
        => new(ConfigurationIsMissingCode, $"Configuration is missing [Type = {type.Name}]");

    public static string InputArrayLengthsDifferCode => $"{Namespace}.{nameof(InputArrayLengthsDiffer)}";
    internal static ComparisonError InputArrayLengthsDiffer(int t1Length, int t2Length, string t1ArrExprName, string t2ArrExprName, Type type)
        => new(InputArrayLengthsDifferCode, $"Array lengths differ " +
            $"[{t1ArrExprName} length = {t1Length}, {t2ArrExprName} length = {t2Length}, Type = {type.Name}]");
    internal static ComparisonError InputArrayLengthsDiffer(int arr1Length, int arr2Length, int arr1index, int arr2index, Type type)
        => new(InputArrayLengthsDifferCode, $"Array lengths differ " +
            $"[Array[{arr1index}] length = {arr1Length}, Array[{arr2index}] length = {arr2Length}, Type = {type.Name}]");

    public static string BothObjectsAreNullCode => $"{Namespace}.{nameof(BothObjectsAreNull)}";
    internal static ComparisonError BothObjectsAreNull()
         => new(BothObjectsAreNullCode, $"Both objects in comparison are null");
    internal static ComparisonError BothObjectsAreNull(string t1ExprName, string t2ExprName)
         => new(BothObjectsAreNullCode, $"Both {t1ExprName} and {t2ExprName} are null");
    internal static ComparisonError BothObjectsAreNull(int i, Type type)
         => new(BothObjectsAreNullCode, $"Objects of type {type.Name} at index 0 and {i} are null");

    public static string OneOfTheObjectsIsNullCode => $"{Namespace}.{nameof(OneOfTheObjectsIsNull)}";
    internal static ComparisonError OneOfTheObjectsIsNull(string t1ExprName, string t2ExprName)
         => new(OneOfTheObjectsIsNullCode, $"One of the objects is null [Object1 = {t1ExprName}, Object2 = {t2ExprName}, AllowNullComparison = False]");

    public static string DepthLimitReachedCode => $"{Namespace}.{nameof(DepthLimitReached)}";
    internal static ComparisonError DepthLimitReached(int currentDepth)
         => new(DepthLimitReachedCode, $"Comparison depth limit ({currentDepth}) has been reached");
    internal static ComparisonError DepthLimitReached(int currentDepth, string t1ExprName, string t2ExprName)
        // TODO: Should I put expression names in brackets like here? 
        => new(DepthLimitReachedCode, $"Comparison depth limit ({currentDepth}) has been reached for [{t1ExprName}] and [{t2ExprName}]");

    public static class Object
    {
        public static string Namespace = "FluentCompare.Error.Object";

        public static string BothObjectsAreNullCode => $"{Namespace}.{nameof(BothObjectsAreNull)}";
        internal static ComparisonError BothObjectsAreNull(object? o1, object? o2, int o1Index, int o2Index)
            => new(BothObjectsAreNullCode, $"One of the objects is null while the other is not " +
                $"[Object1Index = {o1Index}, Object2Index = {o2Index}, " +
                $"Object1 = {o1 ?? "null"}, Object2 = {o2 ?? "null"}]");

    }
}

/// <summary>
/// Type-specific comparison errors repository
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ComparisonErrors<T>
{
    public static string Namespace = $"FluentCompare.Error.{typeof(T).Name}";

}

