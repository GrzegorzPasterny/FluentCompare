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

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, bool, bool, int, int, string?> BoolPairCases =>
        new()
        {
            { b => b.UseComparisonType(ComparisonType.EqualTo), true, true, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.EqualTo), true, false, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), true, false, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), false, true, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), true, false, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), true, true, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThan), false, true, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThan), false, false, 1, 0, ComparisonMismatches.Bool.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), true, true, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), false, false, 0, 0, null },
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

    private void AssertFirstMismatchCode(ComparisonResult result, string expectedCode)
    {
        _testOutputHelper.WriteLine(result.ToString());
        foreach (var mismatch in result.Mismatches)
        {
            _testOutputHelper.WriteLine($"Mismatch [{mismatch.Code}]: {mismatch.Message}");
        }
        foreach (var error in result.Errors)
        {
            _testOutputHelper.WriteLine($"Error [{error.Code}]: {error.Message}");
        }
        foreach (var warning in result.Warnings)
        {
            _testOutputHelper.WriteLine($"Warning [{warning.Code}]: {warning.Message}");
        }

        result.MismatchCount.ShouldBeGreaterThan(0, result.ToString());
        result.Mismatches[0].Code.ShouldBe(expectedCode, result.Mismatches[0].Message);
    }

    private void AssertFirstErrorCode(ComparisonResult result, string expectedCode)
    {
        _testOutputHelper.WriteLine(result.ToString());
        foreach (var mismatch in result.Mismatches)
        {
            _testOutputHelper.WriteLine($"Mismatch [{mismatch.Code}]: {mismatch.Message}");
        }
        foreach (var error in result.Errors)
        {
            _testOutputHelper.WriteLine($"Error [{error.Code}]: {error.Message}");
        }
        foreach (var warning in result.Warnings)
        {
            _testOutputHelper.WriteLine($"Warning [{warning.Code}]: {warning.Message}");
        }

        result.ErrorCount.ShouldBeGreaterThan(0, result.ToString());
        result.Errors[0].Code.ShouldBe(expectedCode, result.Errors[0].Message);
    }

    [Theory]
    [MemberData(nameof(BoolPairCases))]
    public void Compare_BoolPair_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        bool left,
        bool right,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
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

    [Fact]
    public void Compare_BoolPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare(true, false);

        result.MismatchCount.ShouldBe(1);
        result.ErrorCount.ShouldBe(0);
        AssertFirstMismatchCode(result, ComparisonMismatches.Bool.MismatchDetectedCode);
        result.Mismatches[0].Message.ToLowerInvariant().ShouldContain("true");
        result.Mismatches[0].Message.ToLowerInvariant().ShouldContain("false");
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
