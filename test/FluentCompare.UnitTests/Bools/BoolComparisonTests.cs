namespace FluentCompare.UnitTests.Bools;

public class BoolComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public BoolComparisonTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<bool, bool, ComparisonType, int, int, string?> BoolPairCases =>
        new()
        {
            { true, true, ComparisonType.EqualTo, 0, 0, null },
            { true, false, ComparisonType.EqualTo, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
            { true, false, ComparisonType.NotEqualTo, 0, 0, null },
            { false, true, ComparisonType.NotEqualTo, 0, 0, null },
            { true, false, ComparisonType.GreaterThan, 0, 0, null },
            { true, true, ComparisonType.GreaterThan, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
            { false, true, ComparisonType.LessThan, 0, 0, null },
            { false, false, ComparisonType.LessThan, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
            { true, true, ComparisonType.GreaterThanOrEqualTo, 0, 0, null },
            { false, false, ComparisonType.LessThanOrEqualTo, 0, 0, null },
        };

    public static TheoryData<bool[]?, int, int, string?> ParamsBoolCases =>
        new()
        {
            { null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new[] { true }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { new[] { true, true }, 0, 0, null },
            { new[] { true, false }, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
            { new[] { true, true, false }, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
        };

    public static TheoryData<bool?, bool?, int, int, string?> NullableBoolObjectCases =>
        new()
        {
            { true, true, 0, 0, null },
            { true, false, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
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
    [MemberData(nameof(BoolPairCases))]
    public void Compare_BoolPair_UsesExpectedOutcome(
        bool left,
        bool right,
        ComparisonType comparisonType,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder(comparisonType);
        var result = builder.Compare(left, right);

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
    [MemberData(nameof(ParamsBoolCases))]
    public void Compare_ParamsBool_UsesExpectedOutcome(
        bool[]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = values is null
            ? builder.Compare((bool[]?)null)
            : builder.Compare(values);

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
    [MemberData(nameof(NullableBoolObjectCases))]
    public void Compare_ObjectOverload_NullableBool_UsesExpectedOutcome(
        bool? left,
        bool? right,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = builder.Compare((object?)left, (object?)right);

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
