internal class StringComparison : StringComparisonBase
{
    public StringComparison(ComparisonConfiguration configuration)
        : base(configuration) { }

    public override ComparisonResult Compare(string s1, string s2, string t1ExprName, string t2ExprName, ComparisonResult result)
    {
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
        if (!Compare(s1, s2, _comparisonConfiguration.ComparisonType, _comparisonConfiguration.StringConfiguration.StringComparisonType))
        {
            result.AddMismatch(ComparisonMismatches<string>.MismatchDetected(
                s1, s2, t1ExprName, t2ExprName, _comparisonConfiguration.ComparisonType, s => s));
        }
        return result;
    }

    public override ComparisonResult Compare(string[] strings, ComparisonResult result)
    {
        if (strings == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(string)));
            return result;
        }

        if (strings.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(strings.Length, typeof(string)));
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
            if (!Compare(sFirst, sCurrent, _comparisonConfiguration.ComparisonType, _comparisonConfiguration.StringConfiguration.StringComparisonType))
            {
                result.AddMismatch(ComparisonMismatches<string>.MismatchDetected(
                    sFirst, sCurrent, i, _comparisonConfiguration.ComparisonType, s => s));
            }

            Compare(sFirst, sCurrent, "string[0]", $"string[{i}]", result);
        }

        return result;
    }

    public override ComparisonResult Compare(string[][] strings, ComparisonResult result)
    {
        if (strings == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(string[])));
            return result;
        }
        if (strings.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(strings.Length, typeof(string[])));
            return result;
        }
        var first = strings[0];
        for (int i = 1; i < strings.Length; i++)
        {
            Compare(first, strings[i], $"strings[0]", $"strings[{i}]", result);
        }
        return result;
    }

    public override ComparisonResult Compare(string[] sArr1, string[] sArr2, string sArr1ExprName, string sArr2ExprName, ComparisonResult result)
    {
        if (sArr1 is null && sArr2 is null)
        {
            result.AddWarning(ComparisonErrors.BothObjectsAreNull(sArr1ExprName, sArr2ExprName));
        }

        if (ReferenceEquals(sArr1, sArr2))
        {
            return result;
        }

        if (sArr1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(sArr1ExprName, typeof(string[])));
            return result;
        }

        if (sArr2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(sArr2ExprName, typeof(string[])));
            return result;
        }

        if (sArr1.Length != sArr2.Length)
        {
            // TODO: Make it configurable to add warning, or error
            result.AddWarning(ComparisonErrors.InputArrayLengthsDiffer(sArr1.Length, sArr2.Length, sArr1ExprName, sArr2ExprName, typeof(string[])));

            // TODO: Perform the comparison in case of warning
            return result;
        }

        for (int i = 0; i < sArr1.Length; i++)
        {
            var s1 = sArr1[i];
            var s2 = sArr2[i];

            if (s1 == null && s2 == null)
            {
                result.AddWarning(ComparisonErrors.BothObjectsAreNull(i, typeof(string)));
                continue;
            }

            if (s1 == null || s2 == null)
            {
                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(
                    $"{sArr1ExprName}[{i}]", $"{sArr2ExprName}[{i}]", typeof(string)));
                continue;
            }

            if (!Compare(s1, s2,
                _comparisonConfiguration.ComparisonType, _comparisonConfiguration.StringConfiguration.StringComparisonType))
            {
                result.AddMismatch(ComparisonMismatches<string>.MismatchDetected(
                    s1, s2, $"{sArr1ExprName}[{i}]", $"{sArr2ExprName}[{i}]", _comparisonConfiguration.ComparisonType, s => s));
            }
        }

        return result;
    }
}
