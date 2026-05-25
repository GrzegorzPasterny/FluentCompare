namespace FluentCompare.UnitTests.Integers;

public class ShortComparisonTests
{
    private readonly ITestOutputHelper _output;

    public ShortComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, short, short, int, int, string?> ShortPairCases =>
        new()
        {
            { b => b.UseComparisonType(ComparisonType.EqualTo), (short)2, (short)2, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.EqualTo), (short)2, (short)3, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), (short)2, (short)3, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), (short)2, (short)2, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), (short)3, (short)2, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), (short)2, (short)3, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThan), (short)2, (short)3, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThan), (short)3, (short)2, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), (short)2, (short)2, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), (short)1, (short)2, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), (short)2, (short)2, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), (short)3, (short)2, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
        };

    public static TheoryData<short[]?, int, int, string?> ParamsShortCases =>
        new()
        {
            { null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new short[] { 1 }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { new short[] { 1, 1 }, 0, 0, null },
            { new short[] { 1, 2 }, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
            { new short[] { 1, 1, 2 }, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
        };

    public static TheoryData<short?, short?, int, int, string?> NullableShortObjectCases =>
        new()
        {
            { (short)7, (short)7, 0, 0, null },
            { (short)1, (short)2, 1, 0, ComparisonMismatches<short>.MismatchDetectedCode },
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
    [MemberData(nameof(ShortPairCases))]
    public void Compare_ShortPair_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        short left,
        short right,
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
    public void Compare_ShortPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare((short)0, (short)1);

        result.MismatchCount.ShouldBe(1);
        result.ErrorCount.ShouldBe(0);
        AssertFirstMismatchCode(result, ComparisonMismatches<short>.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("0");
        result.Mismatches[0].Message.ShouldContain("1");
    }

    [Theory]
    [MemberData(nameof(ParamsShortCases))]
    public void Compare_ParamsShort_UsesExpectedOutcome(
        short[]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = values is null
            ? builder.Compare((short[]?)null)
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
    [MemberData(nameof(NullableShortObjectCases))]
    public void Compare_ObjectOverload_NullableShort_UsesExpectedOutcome(
        short? left,
        short? right,
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
