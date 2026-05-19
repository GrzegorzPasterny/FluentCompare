namespace FluentCompare.UnitTests.Bools;

public class BoolArrayComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public BoolArrayComparisonTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<bool[]?, bool[]?, int, int, string?> BoolArrayPairCases =>
        new()
        {
            { new[] { true, false, true }, new[] { true, false, true }, 0, 0, null },
            { new[] { true, false, true }, new[] { true, true, true }, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
            { new[] { true, false }, new[] { true, false, true }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, bool[][]?, int, int, string?> BoolArrayParamsCases =>
        new()
        {
            { b => b, null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { true } }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new bool[][] { null!, new[] { true } }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b, new bool[][] { new[] { true }, null! }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new bool[][] { new[] { true }, null! }, 0, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
            { b => b, new[] { new[] { true, false }, new[] { true } }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
            { b => b, new[] { new[] { true, false }, new[] { true, true } }, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
            { b => b, new[] { new[] { true, false }, new[] { true, false } }, 0, 0, null },
        };

    public static TheoryData<bool?[]?, bool?[]?, bool, int, int, string?> NullableBoolArrayPairCases =>
        new()
        {
            { new bool?[] { true, false, true }, new bool?[] { true, true, true }, false, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
            { new bool?[] { true, null, true }, new bool?[] { true, false, true }, false, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new bool?[] { true, false, true }, new bool?[] { true, true, true }, true, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, bool?[][]?, int, int, string?> NullableBoolArrayParamsCases =>
        new()
        {
            { b => b, new bool?[][] { new bool?[] { true, false }, new bool?[] { true, false } }, 0, 0, null },
            { b => b, new bool?[][] { new bool?[] { true, false }, null! }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new bool?[][] { new bool?[] { true, false }, null! }, 0, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
        };

    private void LogResult(params ComparisonResult[] results)
    {
        foreach (var result in results)
        {
            _testOutputHelper.WriteLine(result.ToString());
        }
    }

    private void AssertFirstMismatchCode(ComparisonResult result, string expectedCode)
    {
        LogResult(result);
        result.MismatchCount.ShouldBeGreaterThan(0, result.ToString());
        result.Mismatches[0].Code.ShouldBe(expectedCode, result.Mismatches[0].Message);
    }

    private void AssertFirstErrorCode(ComparisonResult result, string expectedCode)
    {
        LogResult(result);
        result.ErrorCount.ShouldBeGreaterThan(0, result.ToString());
        result.Errors[0].Code.ShouldBe(expectedCode, result.Errors[0].Message);
    }

    [Theory]
    [MemberData(nameof(BoolArrayPairCases))]
    public void Compare_BoolArrayPair_UsesExpectedOutcome(
        bool[]? first,
        bool[]? second,
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
    [MemberData(nameof(BoolArrayParamsCases))]
    public void Compare_BoolArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        bool[][]? values,
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
    [MemberData(nameof(NullableBoolArrayPairCases))]
    public void Compare_NullableBoolArrayPair_UsesExpectedOutcome(
        bool?[]? first,
        bool?[]? second,
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
    [MemberData(nameof(NullableBoolArrayParamsCases))]
    public void Compare_NullableBoolArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        bool?[][]? values,
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
