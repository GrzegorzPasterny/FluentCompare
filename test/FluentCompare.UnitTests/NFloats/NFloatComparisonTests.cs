using System.Runtime.InteropServices;

namespace FluentCompare.UnitTests.NFloats;

public class NFloatComparisonTests
{
    private readonly ITestOutputHelper _output;

    public NFloatComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, NFloat, NFloat, int, int, string?> NFloatPairCases =>
        new()
        {
            { b => b.UseComparisonType(ComparisonType.EqualTo), (NFloat)42, (NFloat)42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.EqualTo), (NFloat)42, (NFloat)43, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), (NFloat)42, (NFloat)43, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), (NFloat)42, (NFloat)42, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), (NFloat)43, (NFloat)42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), (NFloat)42, (NFloat)43, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThan), (NFloat)42, (NFloat)43, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThan), (NFloat)43, (NFloat)42, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), (NFloat)42, (NFloat)42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), (NFloat)41, (NFloat)42, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), (NFloat)42, (NFloat)42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), (NFloat)43, (NFloat)42, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<NFloat[]?, int, int, string?> ParamsNFloatCases =>
        new()
        {
            { null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new[] { (NFloat)1 }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { new[] { (NFloat)1, (NFloat)1 }, 0, 0, null },
            { new[] { (NFloat)1, (NFloat)2 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { new[] { (NFloat)1, (NFloat)1, (NFloat)2 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
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
    [MemberData(nameof(NFloatPairCases))]
    public void Compare_NFloatPair_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        NFloat left,
        NFloat right,
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
    public void Compare_NFloatPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare((NFloat)0, (NFloat)1);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("0");
        result.Mismatches[0].Message.ShouldContain("1");
    }

    [Theory]
    [MemberData(nameof(ParamsNFloatCases))]
    public void Compare_ParamsNFloat_UsesExpectedOutcome(
        NFloat[]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = values is null
            ? builder.Compare((NFloat[]?)null)
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
