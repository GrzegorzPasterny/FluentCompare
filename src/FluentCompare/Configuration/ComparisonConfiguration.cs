using FluentCompare.Configuration.Models;

namespace FluentCompare.Configuration
{
	internal class ComparisonConfiguration
	{
		internal ComplexTypesComparisonMode ComplexTypesComparisonMode { get; set; }
			= ComplexTypesComparisonMode.PropertyEquality;
	}
}
