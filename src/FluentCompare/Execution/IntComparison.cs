using FluentCompare.ResultObjects;

namespace FluentCompare.Execution
{
	public class IntComparison : IExecuteComparison<int>
	{
		internal IntComparison() { }

		public ComparisonResult Compare(params int[] ints)
		{
			throw new NotImplementedException();
		}
	}
}
