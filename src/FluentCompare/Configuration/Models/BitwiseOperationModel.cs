public record BitwiseOperationModel
{
    /// <summary>
    /// The bitwise operation to perform.
    /// </summary>
    public BitwiseOperation Operation { get; set; }

    /// <summary>
    /// The value to use in the bitwise operation.
    /// </summary>
    public byte Value { get; set; }

    /// <summary>
    /// Indexes of comparison objects to exclude from this operation.
    /// </summary>
    public List<int> ComparisonObjectIndexesToExclude { get; set; } = new();
}
