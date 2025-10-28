
public class ByteComparisonConfiguration
{
    /// <summary>
    /// List of bitwise operations to perform on byte values before comparison.
    /// </summary>
    public List<(BitwiseOperation, byte)> BitwiseOperations { get; set; } = new();
}
