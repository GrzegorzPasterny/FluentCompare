
public class ComparisonResult
{
    internal ComparisonResult() { }

    // TODO: Hide the results when comparison was not successful
    private readonly List<ComparisonMismatch> _mismatches = new();
    public IReadOnlyList<ComparisonMismatch> Mismatches => _mismatches;

    private readonly List<ComparisonError> _errors = new();
    public IReadOnlyList<ComparisonError> Errors => _errors;

    private readonly List<ComparisonError> _warnings = new();
    public IReadOnlyList<ComparisonError> Warnings => _warnings;

    public bool AllMatched => _mismatches.Count == 0;
    public int MismatchCount => _mismatches.Count;

    public bool WasSuccessful => _errors.Count == 0;
    public int ErrorCount => _errors.Count;

    public override string ToString()
    {
        return $"Comparison result [AllMatched={AllMatched}, MismatchedCount={MismatchCount}]";
    }

    internal void AddMismatch(ComparisonMismatch mismatch)
    {
        _mismatches.Add(mismatch);
    }

    internal void AddError(ComparisonError error)
    {
        _errors.Add(error);
    }

    internal void AddWarning(ComparisonError error)
    {
        _warnings.Add(error);
    }

    internal void AddMismatches(IReadOnlyList<ComparisonMismatch> mismatches)
    {
        _mismatches.AddRange(mismatches);
    }

    internal void AddWarnings(IReadOnlyList<ComparisonError> warnings)
    {
        _warnings.AddRange(warnings);
    }

    internal void AddErrors(IReadOnlyList<ComparisonError> errors)
    {
        _errors.AddRange(errors);
    }

    internal void AddComparisonResult(ComparisonResult results)
    {
        AddMismatches(results.Mismatches);
        AddErrors(results.Errors);
        AddWarnings(results.Warnings);
    }
}
