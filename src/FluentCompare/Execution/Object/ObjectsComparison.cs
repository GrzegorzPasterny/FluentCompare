using FluentCompare.Configuration;
using FluentCompare.ResultObjects;

namespace FluentCompare.Execution.Object
{
	public class ObjectsComparison : IExecuteComparison<object>
	{
		private readonly ComparisonConfiguration _comparisonConfiguration;

		internal ObjectsComparison(
			ComparisonConfiguration comparisonConfiguration)
		{
			_comparisonConfiguration = comparisonConfiguration;
		}

		public ComparisonResult Compare(params object[] objects)
		{
			throw new NotImplementedException();
		}
	}
}
