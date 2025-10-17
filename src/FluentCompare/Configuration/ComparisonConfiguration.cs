/// <summary>
/// FluentCompare configuration object
/// </summary>
public class ComparisonConfiguration
{
    /// <summary>
    /// The type of comparison to perform on primitive types. Defaults to EqualTo.
    /// Complex types are compared according to the <see cref="ComplexTypesComparisonMode"/> setting.
    /// </summary>
    public ComparisonType ComparisonType { get; set; }
        = ComparisonType.EqualTo;

    /// <summary>
    /// Defines how complex types (types that contain other complex or primitive types) are compared.
    /// </summary>
    public ComplexTypesComparisonMode ComplexTypesComparisonMode { get; set; }
        = ComplexTypesComparisonMode.PropertyEquality;

    /// <summary>
    /// If true, doesn't produce a mismatch, or error, when comparing nulls. Only null is equal to null.
    /// Defaults to true.
    /// </summary>
    public bool AllowNullComparison { get; set; } = true;

    /// <summary>
    /// If true, allows nulls in the arguments to be compared. If false, passing nulls will result in an error.
    /// Defaults to true.
    /// </summary>
    public bool AllowNullsInArguments { get; set; } = true;

    /// <summary>
    /// Configuration for comparing double values. 
    /// (Primitive double type, or any double type which is a property of a complex type)
    /// </summary>
    public DoubleComparisonConfiguration DoubleConfiguration { get; set; }
        = new DoubleComparisonConfiguration();

    public StringComparisonConfiguration StringConfiguration { get; set; }
        = new StringComparisonConfiguration();
}
