namespace FluentCompare.UnitTests.Integers;

public class LongArrayComparisonTests
{
    private readonly ITestOutputHelper _output;

    public LongArrayComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<long[]?, long[]?, int, int, string?> LongArrayPairCases =>
        new()
        {
            { new long[] { 1, 2, 3 }, new long[] { 1, 2, 3 }, 0, 0, null },
            { new long[] { 1, 2, 3 }, new long[] { 1, 9, 3 }, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { new long[] { 1, 2, 3 }, new long[] { 1, 2 }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
        };

    public static TheoryData<ComparisonType, long[]?, long[]?, int, int, string?> LongArrayPairComparisonTypeCases =>
        new()
        {
            { ComparisonType.EqualTo, new long[] { 1 }, new long[] { 1 }, 0, 0, null },
            { ComparisonType.EqualTo, new long[] { 1 }, new long[] { 2 }, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { ComparisonType.NotEqualTo, new long[] { 1 }, new long[] { 2 }, 0, 0, null },
            { ComparisonType.NotEqualTo, new long[] { 1 }, new long[] { 1 }, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { ComparisonType.GreaterThan, new long[] { 2 }, new long[] { 1 }, 0, 0, null },
            { ComparisonType.GreaterThan, new long[] { 1 }, new long[] { 2 }, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { ComparisonType.LessThan, new long[] { 1 }, new long[] { 2 }, 0, 0, null },
            { ComparisonType.LessThan, new long[] { 2 }, new long[] { 1 }, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { ComparisonType.GreaterThanOrEqualTo, new long[] { 1 }, new long[] { 1 }, 0, 0, null },
            { ComparisonType.GreaterThanOrEqualTo, new long[] { 1 }, new long[] { 2 }, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { ComparisonType.LessThanOrEqualTo, new long[] { 1 }, new long[] { 1 }, 0, 0, null },
            { ComparisonType.LessThanOrEqualTo, new long[] { 2 }, new long[] { 1 }, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, long[][]?, int, int, string?> LongArrayParamsCases =>
        new()
        {
            { b => b, null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new long[1][] { new long[] { 1 } }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new long[2][] { new long[] { 1, 2 }, new long[] { 1, 2 } }, 0, 0, null },
            { b => b, new long[2][] { new long[] { 1, 2 }, new long[] { 1, 3 } }, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { b => b, new long[2][] { new long[] { 1, 2 }, null! }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new long[2][] { new long[] { 1, 2 }, null! }, 0, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
        };

    public static TheoryData<long?[]?, long?[]?, bool, int, int, string?> NullableLongArrayPairCases =>
        new()
        {
            { new long?[] { 1, 2, 3 }, new long?[] { 1, 9, 3 }, false, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { new long?[] { 1, null, 3 }, new long?[] { 1, 2, 3 }, false, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new long?[] { 1, 2, 3 }, new long?[] { 1, 9, 3 }, true, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, long?[][]?, int, int, string?> NullableLongArrayParamsCases =>
        new()
        {
            { b => b, new long?[][] { new long?[] { 1, 2 }, new long?[] { 1, 2 } }, 0, 0, null },
            { b => b, new long?[][] { new long?[] { 1, 2 }, null! }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new long?[][] { new long?[] { 1, 2 }, null! }, 0, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
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
    [MemberData(nameof(LongArrayPairCases))]
    public void Compare_LongArrayPair_UsesExpectedOutcome(
        long[]? first,
        long[]? second,
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
    [MemberData(nameof(LongArrayPairComparisonTypeCases))]
    public void Compare_LongArrayPair_RespectsComparisonType_UsesExpectedOutcome(
        ComparisonType comparisonType,
        long[]? first,
        long[]? second,
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
    [MemberData(nameof(LongArrayParamsCases))]
    public void Compare_LongArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        long[][]? values,
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
    [MemberData(nameof(NullableLongArrayPairCases))]
    public void Compare_NullableLongArrayPair_UsesExpectedOutcome(
        long?[]? first,
        long?[]? second,
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
    [MemberData(nameof(NullableLongArrayParamsCases))]
    public void Compare_NullableLongArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        long?[][]? values,
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
