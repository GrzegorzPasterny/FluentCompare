using FluentCompare.Configuration.Models;
using FluentCompare.Execution;

namespace FluentCompare.Configuration
{
	public class ComparisonBuilder
	{
		private ComparisonConfiguration _configuration = new();

		public IExecuteComparison<object> Build()
		{
			// TODO: Consider creating ObjectsComparisonByReferenceEquality
			// and ObjectsComparisonByPropertyEquality classes
			return new ObjectsComparison(_configuration.ComplexTypesComparisonMode);
		}

		public ComparisonBuilder UsePropertyEquality()
		{
			_configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.PropertyEquality;
			return this;
		}

		public ComparisonBuilder UseReferenceEquality()
		{
			_configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.ReferenceEquality;
			return this;
		}
	}
}
