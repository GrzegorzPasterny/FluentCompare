using FluentCompare.Configuration;
using FluentCompare.ResultObjects;

namespace FluentCompare.Execution.Int
{
	public class IntArrayComparison : IntComparisonBase, IExecuteComparison<int[]>
	{
		private readonly ComparisonConfiguration _comparisonConfiguration;

		internal IntArrayComparison(ComparisonConfiguration comparisonConfiguration)
		{
			_comparisonConfiguration = comparisonConfiguration;
		}

		public ComparisonResult Compare(params int[][] objects)
		{
			var result = new ComparisonResult();

			if (objects == null || objects.Length < 2)
				return result;

			var first = objects[0];
			for (int i = 1; i < objects.Length; i++)
			{
				var current = objects[i];

				if (first == null || current == null)
				{
					if (first != current)
					{
						result.AddMismatch(new ComparisonMismatch
						{
							Message = $"Array at index 0 and {i} do not match: one is null, the other is not."
						});
					}
					continue;
				}

				if (first.Length != current.Length)
				{
					result.AddMismatch(new ComparisonMismatch
					{
						Message = $"Array length mismatch at index 0 and {i}: {first.Length} vs {current.Length}."
					});
					continue;
				}

				for (int j = 0; j < first.Length; j++)
				{
					if (!Compare(first[j], current[j], _comparisonConfiguration.ComparisonType))
					{
						result.AddMismatch(new ComparisonMismatch
						{
							Message = $"Value mismatch at index {j} of arrays 0 and {i}: {first[j]} vs {current[j]}."
						});
					}
				}
			}

			return result;
		}
	}
}
