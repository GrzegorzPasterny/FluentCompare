namespace FluentCompare.UnitTests.Integers;

public class ShortArrayComparisonTests
{
    private readonly ITestOutputHelper _output;

    public ShortArrayComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<short[]?, short[]?, int, int, string?> ShortArrayPairCases =>
        new()
        {
            { new short[] { 1, 2, 3 }, new short[] { 1, 2, 3 }, 0, 0, null },
            { new short[] { 1, 2, 3 }, new short[] { 1, 9, 3 }, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { new short[] { 1, 2, 3 }, new short[] { 1, 2 }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
        };

    public static TheoryData<ComparisonType, short[]?, short[]?, int, int, string?> ShortArrayPairComparisonTypeCases =>
        new()
        {
            { ComparisonType.EqualTo, new short[] { 1 }, new short[] { 1 }, 0, 0, null },
            { ComparisonType.EqualTo, new short[] { 1 }, new short[] { 2 }, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { ComparisonType.NotEqualTo, new short[] { 1 }, new short[] { 2 }, 0, 0, null },
            { ComparisonType.NotEqualTo, new short[] { 1 }, new short[] { 1 }, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { ComparisonType.GreaterThan, new short[] { 2 }, new short[] { 1 }, 0, 0, null },
            { ComparisonType.GreaterThan, new short[] { 1 }, new short[] { 2 }, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { ComparisonType.LessThan, new short[] { 1 }, new short[] { 2 }, 0, 0, null },
            { ComparisonType.LessThan, new short[] { 2 }, new short[] { 1 }, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { ComparisonType.GreaterThanOrEqualTo, new short[] { 1 }, new short[] { 1 }, 0, 0, null },
            { ComparisonType.GreaterThanOrEqualTo, new short[] { 1 }, new short[] { 2 }, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { ComparisonType.LessThanOrEqualTo, new short[] { 1 }, new short[] { 1 }, 0, 0, null },
            { ComparisonType.LessThanOrEqualTo, new short[] { 2 }, new short[] { 1 }, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, short[][]?, int, int, string?> ShortArrayParamsCases =>
        new()
        {
            { b => b, null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new short[1][] { new short[] { 1 } }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new short[2][] { new short[] { 1, 2 }, new short[] { 1, 2 } }, 0, 0, null },
            { b => b, new short[2][] { new short[] { 1, 2 }, new short[] { 1, 3 } }, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { b => b, new short[2][] { new short[] { 1, 2 }, null! }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new short[2][] { new short[] { 1, 2 }, null! }, 0, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
        };

    public static TheoryData<short?[]?, short?[]?, bool, int, int, string?> NullableShortArrayPairCases =>
        new()
        {
            { new short?[] { 1, 2, 3 }, new short?[] { 1, 9, 3 }, false, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { new short?[] { 1, null, 3 }, new short?[] { 1, 2, 3 }, false, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new short?[] { 1, 2, 3 }, new short?[] { 1, 9, 3 }, true, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, short?[][]?, int, int, string?> NullableShortArrayParamsCases =>
        new()
        {
            { b => b, new short?[][] { new short?[] { 1, 2 }, new short?[] { 1, 2 } }, 0, 0, null },
            { b => b, new short?[][] { new short?[] { 1, 2 }, null! }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new short?[][] { new short?[] { 1, 2 }, null! }, 0, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
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
    [MemberData(nameof(ShortArrayPairCases))]
    public void Compare_ShortArrayPair_UsesExpectedOutcome(
        short[]? first,
        short[]? second,
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
    [MemberData(nameof(ShortArrayPairComparisonTypeCases))]
    public void Compare_ShortArrayPair_RespectsComparisonType_UsesExpectedOutcome(
        ComparisonType comparisonType,
        short[]? first,
        short[]? second,
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
    [MemberData(nameof(ShortArrayParamsCases))]
    public void Compare_ShortArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        short[][]? values,
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
    [MemberData(nameof(NullableShortArrayPairCases))]
    public void Compare_NullableShortArrayPair_UsesExpectedOutcome(
        short?[]? first,
        short?[]? second,
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
    [MemberData(nameof(NullableShortArrayParamsCases))]
    public void Compare_NullableShortArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        short?[][]? values,
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
