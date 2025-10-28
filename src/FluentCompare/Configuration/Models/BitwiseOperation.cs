/// <summary>
/// Supported bitwise operations
/// </summary>
public enum BitwiseOperation
{
    /// <summary>
    /// Bitwise AND operation
    /// </summary>
    And,

    /// <summary>
    /// Bitwise OR operation
    /// </summary>
    Or,

    /// <summary>
    /// Bitwise XOR operation
    /// </summary>
    Xor,

    /// <summary>
    /// Bitwise NOT operation. Mask provided doesn't matter here, as the original value is simply inverted.
    /// </summary>
    Not,

    /// <summary>
    /// Bitwise Shift Left operation. Shifts the bits to the left by the number of positions specified in the mask.
    /// </summary>
    ShiftLeft,

    /// <summary>
    /// Bitwise Shift Right operation. Shifts the bits to the right by the number of positions specified in the mask.
    /// </summary>
    ShiftRight
}
