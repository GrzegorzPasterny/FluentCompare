namespace FluentCompare.UnitTests.Integers;

public class LongComparisonTests
{
    private readonly ITestOutputHelper _output;

    public LongComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, long, long, int, int, string?> LongPairCases =>
        new()
        {
            { b => b.UseComparisonType(ComparisonType.EqualTo), 2L, 2L, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.EqualTo), 2L, 3L, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), 2L, 3L, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), 2L, 2L, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), 3L, 2L, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), 2L, 3L, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThan), 2L, 3L, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThan), 3L, 2L, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), 2L, 2L, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), 1L, 2L, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), 2L, 2L, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), 3L, 2L, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
        };

    public static TheoryData<long[]?, int, int, string?> ParamsLongCases =>
        new()
        {
            { null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new long[] { 1L }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { new long[] { 1L, 1L }, 0, 0, null },
            { new long[] { 1L, 2L }, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
            { new long[] { 1L, 1L, 2L }, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
        };

    public static TheoryData<long?, long?, int, int, string?> NullableLongObjectCases =>
        new()
        {
            { 7L, 7L, 0, 0, null },
            { 1L, 2L, 1, 0, ComparisonMismatches<long>.MismatchDetectedCode },
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
    [MemberData(nameof(LongPairCases))]
    public void Compare_LongPair_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        long left,
        long right,
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
    public void Compare_LongPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare(0L, 1L);

        result.MismatchCount.ShouldBe(1);
        result.ErrorCount.ShouldBe(0);
        AssertFirstMismatchCode(result, ComparisonMismatches<long>.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("0");
        result.Mismatches[0].Message.ShouldContain("1");
    }

    [Theory]
    [MemberData(nameof(ParamsLongCases))]
    public void Compare_ParamsLong_UsesExpectedOutcome(
        long[]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = values is null
            ? builder.Compare((long[]?)null)
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
    [MemberData(nameof(NullableLongObjectCases))]
    public void Compare_ObjectOverload_NullableLong_UsesExpectedOutcome(
        long? left,
        long? right,
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
