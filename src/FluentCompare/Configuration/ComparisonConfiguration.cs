using FluentCompare.Configuration.Models;

namespace FluentCompare.Configuration;

internal class ComparisonConfiguration
{
    internal ComparisonType ComparisonType { get; set; }
        = ComparisonType.EqualTo;

    internal ComplexTypesComparisonMode ComplexTypesComparisonMode { get; set; }
        = ComplexTypesComparisonMode.PropertyEquality;
}
