using FluentCompare.Configuration.Models;
using FluentCompare.ResultObjects;

namespace FluentCompare.Execution
{
	public class ObjectsComparison : IExecuteComparison<object>
	{
		internal ObjectsComparison(ComplexTypesComparisonMode complexTypesComparisonMode) { }

		public ComparisonResult Compare(params object[] objects)
		{
			throw new NotImplementedException();
		}
	}
}
