using FluentCompare.Execution;

namespace FluentCompare.Configuration
{
	public class ComparisonBuilder
	{
		private ComparisonConfiguration _configuration = new();

		public IExecuteComparison<object> Build()
		{
			return new ObjectsComparison();
		}
	}
}
