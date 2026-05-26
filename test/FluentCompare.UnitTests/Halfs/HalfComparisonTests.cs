namespace FluentCompare.UnitTests.Halfs;

public class HalfComparisonTests
{
    private readonly ITestOutputHelper _output;

    public HalfComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, Half, Half, int, int, string?> HalfPairCases =>
        new()
        {
            { b => b.UseComparisonType(ComparisonType.EqualTo), (Half)42, (Half)42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.EqualTo), (Half)42, (Half)43, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), (Half)42, (Half)43, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), (Half)42, (Half)42, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), (Half)43, (Half)42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), (Half)42, (Half)43, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThan), (Half)42, (Half)43, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThan), (Half)43, (Half)42, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), (Half)42, (Half)42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), (Half)41, (Half)42, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), (Half)42, (Half)42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), (Half)43, (Half)42, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<Half[]?, int, int, string?> ParamsHalfCases =>
        new()
        {
            { null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new[] { (Half)1 }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { new[] { (Half)1, (Half)1 }, 0, 0, null },
            { new[] { (Half)1, (Half)2 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { new[] { (Half)1, (Half)1, (Half)2 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
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
    [MemberData(nameof(HalfPairCases))]
    public void Compare_HalfPair_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        Half left,
        Half right,
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
    public void Compare_HalfPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare((Half)0, (Half)1);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("0");
        result.Mismatches[0].Message.ShouldContain("1");
    }

    [Theory]
    [MemberData(nameof(ParamsHalfCases))]
    public void Compare_ParamsHalf_UsesExpectedOutcome(
        Half[]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = values is null
            ? builder.Compare((Half[]?)null)
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
}
