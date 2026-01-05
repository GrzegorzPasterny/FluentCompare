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
    /// If true, allows arrays of different lengths to be compared without producing an error or mismatch.
    /// Defaults to true.
    /// </summary>
    public bool AllowArrayComparisonOfDifferentLengths { get; set; } = true;

    /// <summary>
    /// Maximum depth for recursive comparison of complex types.
    /// </summary>
    public int MaximumComparisonDepth { get; set; } = 5;

    /// <summary>
    /// If true, the comparison will stop and return on the first mismatch found. 
    /// If false, the comparison will continue and collect all mismatches. Defaults to false.
    /// </summary>
    public bool FinishComparisonOnFirstMismatch { get; set; } = false;

    /// <summary>
    /// Configuration for comparing floating point values. 
    /// (Primitive float family type, or any float type which is a property of a complex type)
    /// </summary>
    public FloatComparisonConfiguration FloatConfiguration { get; set; }
        = new FloatComparisonConfiguration();

    /// <summary>
    /// Configuration for comparing string values.
    /// </summary>
    public StringComparisonConfiguration StringConfiguration { get; set; }
        = new StringComparisonConfiguration();

    /// <summary>
    /// Configuration for comparing byte values.
    /// </summary>
    public ByteComparisonConfiguration ByteConfiguration { get; set; }
        = new ByteComparisonConfiguration();

    // TODO: Implement
    /// <summary>
    /// A list of types that will be excluded from comparison operations. 
    /// Any type in this list will not be compared when performing object comparisons.
    /// </summary>
    public List<Type> TypesExcludedFromComparison { get; set; } = new List<Type>();
}

