namespace FluentCompare.Tests.Shared.Models;
public class ClassWithAllSupportedTypes
{
    public int Int { get; set; }
    public int[] IntArray { get; set; }
    public string String { get; set; }
    public string[] StringArray { get; set; }
    public double Double { get; set; }
    public double[] DoubleArray { get; set; }
    public object Object { get; set; }
    public object[] ObjectArray { get; set; }
    public ClassWithAllSupportedTypes NestedClass { get; set; }
    public ClassWithAllSupportedTypes[] NestedClassArray { get; set; }
}
