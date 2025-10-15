/// <summary>
/// Defines how complex types (types that contain other complex or primitive types) are compared.
/// </summary>
public enum ComplexTypesComparisonMode
{
    /// <summary>
    /// Compares complex types by their reference. 
    /// Two references are equal if they point to the same object in memory.
    /// </summary>
    ReferenceEquality,
    /// <summary>
    /// Compares complex types by their public properties.
    /// </summary>
    PropertyEquality
}
