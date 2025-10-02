using FluentCompare.ResultObjects;

namespace FluentCompare.Execution
{
	public class ObjectsComparison : IExecuteComparison<object>
	{
		internal ObjectsComparison() { }

		public ComparisonResult Compare(params object[] objects)
		{
			throw new NotImplementedException();
		}
	}
}
