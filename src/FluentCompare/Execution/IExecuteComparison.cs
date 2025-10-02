using FluentCompare.ResultObjects;

namespace FluentCompare.Execution
{
	public interface IExecuteComparison<T>
	{
		ComparisonResult Compare(params T[] objects);
	}
}
