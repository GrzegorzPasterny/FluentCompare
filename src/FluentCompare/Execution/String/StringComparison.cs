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

    public ComparisonResult Compare(params string[] strings)
    {
        ComparisonResult result = new();

        if (strings == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(string)));
            return result;
        }

        if (strings.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectToCompare(strings.Length, typeof(string)));
            return result;
        }

        var sFirst = strings[0];

        for (int i = 1; i < strings.Length; i++)
        {
            var sCurrent = strings[i];

            if (sFirst == null && sCurrent == null)
            {
                result.AddWarning(ComparisonErrors.BothObjectsAreNull(i, typeof(string)));
                return result;
            }
            if (sFirst == null || sCurrent == null)
            {
                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(i, typeof(string)));
                return result;
            }
            if (!Compare(sFirst, sCurrent, _configuration.ComparisonType, _configuration.StringConfiguration.StringComparisonType))
            {
                result.AddMismatch(ComparisonMismatches<string>.MismatchDetected(
                    sFirst, sCurrent, i, _configuration.ComparisonType, s => s));
            }

            result.AddComparisonResult(Compare(sFirst, sCurrent, "string[0]", $"string[{i}]"));
        }

        return result;
    }
}
