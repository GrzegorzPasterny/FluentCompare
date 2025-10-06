public class ComparisonConfiguration
{
    public ComparisonType ComparisonType { get; set; }
        = ComparisonType.EqualTo;

    public ComplexTypesComparisonMode ComplexTypesComparisonMode { get; set; }
        = ComplexTypesComparisonMode.PropertyEquality;
}
