
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

    public static string ConfigurationIsMissingCode => $"{Namespace}.{nameof(ConfigurationIsMissing)}";
    internal static ComparisonError ConfigurationIsMissing(Type type)
        => new(ConfigurationIsMissingCode, $"Configuration is missing [Type = {type.Name}]");
}

/// <summary>
/// Type-specific comparison errors repository
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ComparisonErrors<T>
{
    public static string Namespace = $"FluentCompare.Error.{typeof(T).Name}";

}

