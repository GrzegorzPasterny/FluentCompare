namespace FluentCompare.Tests.Shared.Models;

public class ClassWithAllSupportedTypes
{
    public int Int { get; set; }
    public int[]? IntArray { get; set; }
    public byte Byte { get; set; }
    public byte[]? ByteArray { get; set; }
    public bool Bool { get; set; }
    public bool[]? BoolArray { get; set; }
    public string? String { get; set; }
    public string[]? StringArray { get; set; }
    public double Double { get; set; }
    public double[]? DoubleArray { get; set; }
    public float Float { get; set; }
    public float[]? FloatArray { get; set; }
    public decimal Decimal { get; set; }
    public decimal[]? DecimalArray { get; set; }
    public object? Object { get; set; }
    public object[]? ObjectArray { get; set; }
    public ClassWithAllSupportedTypes? NestedClass { get; set; }
    public ClassWithAllSupportedTypes[]? NestedClassArray { get; set; }
}
