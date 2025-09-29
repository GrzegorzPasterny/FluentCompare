namespace FluentCompare.ResultObjects
{
	public class ComparisonResult
	{
		private readonly List<ComparisonMismatch> _mismatches = new();
		public IReadOnlyList<ComparisonMismatch> Mismatches => _mismatches;

		public bool AllMatched => _mismatches.Count == 0;
		public int MismatchCount => _mismatches.Count;

		public override string ToString()
		{
			return $"Comparison result [AllMatched={AllMatched}, MismatchedCount={MismatchCount}]";
		}
	}
}
