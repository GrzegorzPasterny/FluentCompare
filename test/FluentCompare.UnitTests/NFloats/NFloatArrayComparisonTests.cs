using System.Runtime.InteropServices;

namespace FluentCompare.UnitTests.NFloats;

public class NFloatArrayComparisonTests
{
    private readonly ITestOutputHelper _output;

    public NFloatArrayComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<NFloat[]?, NFloat[]?, int, int, string?> NFloatArrayPairCases =>
        new()
        {
            { new[] { (NFloat)1, (NFloat)2, (NFloat)3 }, new[] { (NFloat)1, (NFloat)2, (NFloat)3 }, 0, 0, null },
            { new[] { (NFloat)1, (NFloat)2, (NFloat)3 }, new[] { (NFloat)1, (NFloat)9, (NFloat)3 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { new[] { (NFloat)1, (NFloat)2, (NFloat)3 }, new[] { (NFloat)1, (NFloat)2 }, 0, 0, null },
            { new[] { (NFloat)1, (NFloat)2 }, null, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { null, new[] { (NFloat)1, (NFloat)2 }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
        };

    public static TheoryData<ComparisonType, NFloat[]?, NFloat[]?, int, int, string?> NFloatArrayPairComparisonTypeCases =>
        new()
        {
            { ComparisonType.EqualTo, new[] { (NFloat)1 }, new[] { (NFloat)1 }, 0, 0, null },
            { ComparisonType.EqualTo, new[] { (NFloat)1 }, new[] { (NFloat)2 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.NotEqualTo, new[] { (NFloat)1 }, new[] { (NFloat)2 }, 0, 0, null },
            { ComparisonType.NotEqualTo, new[] { (NFloat)1 }, new[] { (NFloat)1 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.GreaterThan, new[] { (NFloat)2 }, new[] { (NFloat)1 }, 0, 0, null },
            { ComparisonType.GreaterThan, new[] { (NFloat)1 }, new[] { (NFloat)2 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.LessThan, new[] { (NFloat)1 }, new[] { (NFloat)2 }, 0, 0, null },
            { ComparisonType.LessThan, new[] { (NFloat)2 }, new[] { (NFloat)1 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.GreaterThanOrEqualTo, new[] { (NFloat)1 }, new[] { (NFloat)1 }, 0, 0, null },
            { ComparisonType.GreaterThanOrEqualTo, new[] { (NFloat)1 }, new[] { (NFloat)2 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.LessThanOrEqualTo, new[] { (NFloat)1 }, new[] { (NFloat)1 }, 0, 0, null },
            { ComparisonType.LessThanOrEqualTo, new[] { (NFloat)2 }, new[] { (NFloat)1 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, NFloat[][]?, int, int, string?> NFloatArrayParamsCases =>
        new()
        {
            { b => b, null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { (NFloat)1 } }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new NFloat[][] { null!, new[] { (NFloat)1 } }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new NFloat[][] { new[] { (NFloat)1 }, null! }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new NFloat[][] { new[] { (NFloat)1 }, null! }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { (NFloat)1, (NFloat)2 }, new[] { (NFloat)1 } }, 0, 0, null },
            { b => b, new[] { new[] { (NFloat)1, (NFloat)2 }, new[] { (NFloat)1, (NFloat)2 } }, 0, 0, null },
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
    [MemberData(nameof(NFloatArrayPairCases))]
    public void Compare_NFloatArrayPair_UsesExpectedOutcome(
        NFloat[]? first,
        NFloat[]? second,
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
    [MemberData(nameof(NFloatArrayPairComparisonTypeCases))]
    public void Compare_NFloatArrayPair_RespectsComparisonType_UsesExpectedOutcome(
        ComparisonType comparisonType,
        NFloat[]? first,
        NFloat[]? second,
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
    [MemberData(nameof(NFloatArrayParamsCases))]
    public void Compare_NFloatArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        NFloat[][]? values,
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

    [Fact]
    public void Compare_NFloatArrayPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();
        NFloat[] firstLiteral = [(NFloat)1, (NFloat)2];
        NFloat[] secondLiteral = [(NFloat)1, (NFloat)3];

        var result = builder.Compare(firstLiteral, secondLiteral);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("1");
        result.Mismatches[0].Message.ShouldContain("2");
        result.Mismatches[0].Message.ShouldContain("3");
    }
}
