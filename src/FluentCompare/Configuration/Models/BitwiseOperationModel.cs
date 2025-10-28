public record BitwiseOperationModel
{
    /// <summary>
    /// The bitwise operation to perform.
    /// </summary>
    public BitwiseOperation Operation { get; set; }

    /// <summary>
    /// The value to use in the bitwise operation.<br /><br />
    /// For <see cref="BitwiseOperation.And"/>, <see cref="BitwiseOperation.Or"/>,
    /// and <see cref="BitwiseOperation.Xor"/> this value is the mask.
    /// <br />
    /// For <see cref="BitwiseOperation.Not"/>, this value is ignored./>
    /// <br />
    /// For <see cref="BitwiseOperation.ShiftLeft"/> this value indicates the number of positions to shift left.
    /// <br />
    /// For <see cref="BitwiseOperation.ShiftRight"/> this value indicates the number of positions to shift right./>
    /// </summary>
    public byte Value { get; set; }

    /// <summary>
    /// Indexes of comparison objects to exclude from this operation. It is 0 based index.
    /// </summary>
    public List<int> ComparisonObjectIndexesToExclude { get; set; } = new();
}
