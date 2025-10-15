
/// <summary>
/// Defines the method used to compare double values in terms of precision.
/// </summary>
public enum DoubleToleranceMethods
{
    /// <summary>
    /// Compares double values by rounding them to a specified number of decimal places before comparison.
    /// </summary>
    Rounding,

    /// <summary>
    /// Compares double values by checking if the absolute difference between them is within a specified epsilon value.
    /// </summary>
    Epsilon
}
