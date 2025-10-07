
/// <summary>
/// Generic comparison errors repository
/// </summary>
public static class ComparisonErrors
{
    public static string Namespace = "FluentCompare.Error";

    public static string NotEnoughObjectToCompareCode => $"{Namespace}.{nameof(NotEnoughObjectToCompare)}";
    internal static ComparisonError NotEnoughObjectToCompare(int length, Type type)
        => new(NotEnoughObjectToCompareCode,
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
}

/// <summary>
/// Type-specific comparison errors repository
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ComparisonErrors<T>
{
    public static string Namespace = $"FluentCompare.Error.{typeof(T).Name}";

}

