namespace FluentCompare.UnitTests.Integers;

public class IntArrayComparisonTests
{
    private readonly ITestOutputHelper _output;

    public IntArrayComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<int[]?, int[]?, int, int, string?> IntArrayPairCases =>
        new()
        {
            { new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 5 }, 0, 0, null },
            { new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 6 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4 }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
        };

    public static TheoryData<ComparisonType, int[]?, int[]?, int, int, string?> IntArrayPairComparisonTypeCases =>
        new()
        {
            { ComparisonType.EqualTo, new[] { 1, 2 }, new[] { 1, 2 }, 0, 0, null },
            { ComparisonType.EqualTo, new[] { 1, 2 }, new[] { 1, 3 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { ComparisonType.NotEqualTo, new[] { 1, 2 }, new[] { 9, 8 }, 0, 0, null },
            { ComparisonType.NotEqualTo, new[] { 1, 2 }, new[] { 1, 2 }, 2, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { ComparisonType.GreaterThan, new[] { 3, 4 }, new[] { 1, 2 }, 0, 0, null },
            { ComparisonType.GreaterThan, new[] { 1, 2 }, new[] { 3, 4 }, 2, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { ComparisonType.LessThan, new[] { 1, 2 }, new[] { 3, 4 }, 0, 0, null },
            { ComparisonType.LessThan, new[] { 3, 4 }, new[] { 1, 2 }, 2, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { ComparisonType.GreaterThanOrEqualTo, new[] { 2, 2 }, new[] { 2, 2 }, 0, 0, null },
            { ComparisonType.GreaterThanOrEqualTo, new[] { 1, 2 }, new[] { 2, 3 }, 2, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { ComparisonType.LessThanOrEqualTo, new[] { 2, 2 }, new[] { 2, 2 }, 0, 0, null },
            { ComparisonType.LessThanOrEqualTo, new[] { 3, 4 }, new[] { 2, 3 }, 2, 0, ComparisonMismatches<int>.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, int[][]?, int, int, string?> IntArrayParamsCases =>
        new()
        {
            { b => b, null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { 1 } }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new int[][] { null!, new[] { 1 } }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b, new int[][] { new[] { 1 }, null! }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new int[][] { new[] { 1 }, null! }, 0, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
            { b => b, new[] { new[] { 1, 2 }, new[] { 1 } }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
            { b => b, new[] { new[] { 1, 2 }, new[] { 1, 3 } }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { b => b, new[] { new[] { 1, 2 }, new[] { 1, 2 } }, 0, 0, null },
        };

    public static TheoryData<int?[]?, int?[]?, bool, int, int, string?> NullableIntArrayPairCases =>
        new()
        {
            { new int?[] { 1, 2, 3 }, new int?[] { 1, 9, 3 }, false, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { new int?[] { 1, null, 3 }, new int?[] { 1, 2, 3 }, false, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new int?[] { 1, 2, 3 }, new int?[] { 1, 9, 3 }, true, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, int[]?, int[]?, int, int, string?> IntArrayPairDifferentLengthsConfigurationCases =>
        new()
        {
            { b => b.DisallowArrayComparisonOfDifferentLengths(), new[] { 1, 2, 3 }, new[] { 1, 2 }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
            { b => b.AllowArrayComparisonOfDifferentLengths(), new[] { 1, 2, 3 }, new[] { 1, 2 }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, int[][]?, int, int, string?> IntArrayParamsFinishModeCases =>
        new()
        {
            { b => b.FinishComparisonCollectingAllMismatches(), new[] { new[] { 1, 1, 1 }, new[] { 2, 2, 2 } }, 3, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { b => b.FinishComparisonOnFirstMismatch(), new[] { new[] { 1, 1, 1 }, new[] { 2, 2, 2 } }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
        };

    private void AssertFirstMismatchCode(ComparisonResult result, string expectedCode)
    {
        _output.WriteLine(result.ToString());
        foreach (var mismatch in result.Mismatches)
        {
            _output.WriteLine($"Mismatch [{mismatch.Code}]: {mismatch.Message}");
        }
        foreach (var error in result.Errors)
        {
            _output.WriteLine($"Error [{error.Code}]: {error.Message}");
        }
        foreach (var warning in result.Warnings)
        {
            _output.WriteLine($"Warning [{warning.Code}]: {warning.Message}");
        }

        result.MismatchCount.ShouldBeGreaterThan(0, result.ToString());
        result.Mismatches[0].Code.ShouldBe(expectedCode, result.Mismatches[0].Message);
    }

    private void AssertFirstErrorCode(ComparisonResult result, string expectedCode)
    {
        _output.WriteLine(result.ToString());
        foreach (var mismatch in result.Mismatches)
        {
            _output.WriteLine($"Mismatch [{mismatch.Code}]: {mismatch.Message}");
        }
        foreach (var error in result.Errors)
        {
            _output.WriteLine($"Error [{error.Code}]: {error.Message}");
        }
        foreach (var warning in result.Warnings)
        {
            _output.WriteLine($"Warning [{warning.Code}]: {warning.Message}");
        }

        result.ErrorCount.ShouldBeGreaterThan(0, result.ToString());
        result.Errors[0].Code.ShouldBe(expectedCode, result.Errors[0].Message);
    }

    [Theory]
    [MemberData(nameof(IntArrayPairCases))]
    public void Compare_IntArrayPair_UsesExpectedOutcome(
        int[]? first,
        int[]? second,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = builder.Compare(first, second);

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);

        if (expectedCode is not null)
        {
            if (expectedErrors > 0)
            {
                AssertFirstErrorCode(result, expectedCode);
            }
            else
            {
                AssertFirstMismatchCode(result, expectedCode);
            }
        }
    }

    [Theory]
    [MemberData(nameof(IntArrayPairComparisonTypeCases))]
    public void Compare_IntArrayPair_RespectsComparisonType_UsesExpectedOutcome(
        ComparisonType comparisonType,
        int[]? first,
        int[]? second,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder(comparisonType);
        var result = builder.Compare(first, second);

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);

        if (expectedCode is not null)
        {
            if (expectedErrors > 0)
            {
                AssertFirstErrorCode(result, expectedCode);
            }
            else
            {
                AssertFirstMismatchCode(result, expectedCode);
            }
        }
    }

    [Theory]
    [MemberData(nameof(IntArrayParamsCases))]
    public void Compare_IntArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        int[][]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = builder.Compare(values);

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);

        if (expectedCode is not null)
        {
            if (expectedErrors > 0)
            {
                AssertFirstErrorCode(result, expectedCode);
            }
            else
            {
                AssertFirstMismatchCode(result, expectedCode);
            }
        }
    }

    [Theory]
    [MemberData(nameof(NullableIntArrayPairCases))]
    public void Compare_NullableIntArrayPair_UsesExpectedOutcome(
        int?[]? first,
        int?[]? second,
        bool useObjectOverload,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = useObjectOverload
            ? builder.Compare((object?)first, (object?)second)
            : builder.Compare(first, second);

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);

        if (expectedCode is not null)
        {
            if (expectedErrors > 0)
            {
                AssertFirstErrorCode(result, expectedCode);
            }
            else
            {
                AssertFirstMismatchCode(result, expectedCode);
            }
        }
    }

    [Theory]
    [MemberData(nameof(IntArrayPairDifferentLengthsConfigurationCases))]
    public void Compare_IntArrayPair_DifferentLengthsConfiguration_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        int[]? first,
        int[]? second,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = builder.Compare(first, second);

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);

        if (expectedCode is not null)
        {
            if (expectedErrors > 0)
            {
                AssertFirstErrorCode(result, expectedCode);
            }
            else
            {
                AssertFirstMismatchCode(result, expectedCode);
            }
        }
    }

    [Theory]
    [MemberData(nameof(IntArrayParamsFinishModeCases))]
    public void Compare_IntArrayParams_FinishMode_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        int[][]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = builder.Compare(values);

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);

        if (expectedCode is not null)
        {
            if (expectedErrors > 0)
            {
                AssertFirstErrorCode(result, expectedCode);
            }
            else
            {
                AssertFirstMismatchCode(result, expectedCode);
            }
        }
    }
}
