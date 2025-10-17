internal class StringComparison : StringComparisonBase, IExecuteComparison<string>
{
    private ComparisonConfiguration _configuration;

    public StringComparison(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ComparisonResult Compare(string s1, string s2, string t1ExprName, string t2ExprName)
    {
        ComparisonResult result = new();
        if (s1 == null && s2 == null)
        {
            result.AddWarning(ComparisonErrors.BothObjectsAreNull(t1ExprName, t2ExprName));
            return result;
        }
        if (s1 == null || s2 == null)
        {
            result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(
                t1ExprName, t2ExprName, typeof(string)));
            return result;
        }
        if (!Compare(s1, s2, _configuration.ComparisonType, _configuration.StringConfiguration.StringComparisonType))
        {
            result.AddMismatch(ComparisonMismatches<string>.MismatchDetected(
                s1, s2, t1ExprName, t2ExprName, _configuration.ComparisonType, s => s));
        }
        return result;
    }

    public ComparisonResult Compare(params string[] objects) => throw new NotImplementedException();
}
