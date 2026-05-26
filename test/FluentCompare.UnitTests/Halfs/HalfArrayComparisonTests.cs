namespace FluentCompare.UnitTests.Halfs;

public class HalfArrayComparisonTests
{
    private readonly ITestOutputHelper _output;

    public HalfArrayComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<Half[]?, Half[]?, int, int, string?> HalfArrayPairCases =>
        new()
        {
            { new[] { (Half)1, (Half)2, (Half)3 }, new[] { (Half)1, (Half)2, (Half)3 }, 0, 0, null },
            { new[] { (Half)1, (Half)2, (Half)3 }, new[] { (Half)1, (Half)9, (Half)3 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { new[] { (Half)1, (Half)2, (Half)3 }, new[] { (Half)1, (Half)2 }, 0, 0, null },
            { new[] { (Half)1, (Half)2 }, null, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { null, new[] { (Half)1, (Half)2 }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
        };

    public static TheoryData<ComparisonType, Half[]?, Half[]?, int, int, string?> HalfArrayPairComparisonTypeCases =>
        new()
        {
            { ComparisonType.EqualTo, new[] { (Half)1 }, new[] { (Half)1 }, 0, 0, null },
            { ComparisonType.EqualTo, new[] { (Half)1 }, new[] { (Half)2 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.NotEqualTo, new[] { (Half)1 }, new[] { (Half)2 }, 0, 0, null },
            { ComparisonType.NotEqualTo, new[] { (Half)1 }, new[] { (Half)1 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.GreaterThan, new[] { (Half)2 }, new[] { (Half)1 }, 0, 0, null },
            { ComparisonType.GreaterThan, new[] { (Half)1 }, new[] { (Half)2 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.LessThan, new[] { (Half)1 }, new[] { (Half)2 }, 0, 0, null },
            { ComparisonType.LessThan, new[] { (Half)2 }, new[] { (Half)1 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.GreaterThanOrEqualTo, new[] { (Half)1 }, new[] { (Half)1 }, 0, 0, null },
            { ComparisonType.GreaterThanOrEqualTo, new[] { (Half)1 }, new[] { (Half)2 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.LessThanOrEqualTo, new[] { (Half)1 }, new[] { (Half)1 }, 0, 0, null },
            { ComparisonType.LessThanOrEqualTo, new[] { (Half)2 }, new[] { (Half)1 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, Half[][]?, int, int, string?> HalfArrayParamsCases =>
        new()
        {
            { b => b, null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { (Half)1 } }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new Half[][] { null!, new[] { (Half)1 } }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new Half[][] { new[] { (Half)1 }, null! }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new Half[][] { new[] { (Half)1 }, null! }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { (Half)1, (Half)2 }, new[] { (Half)1 } }, 0, 0, null },
            { b => b, new[] { new[] { (Half)1, (Half)2 }, new[] { (Half)1, (Half)2 } }, 0, 0, null },
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
    [MemberData(nameof(HalfArrayPairCases))]
    public void Compare_HalfArrayPair_UsesExpectedOutcome(
        Half[]? first,
        Half[]? second,
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
    [MemberData(nameof(HalfArrayPairComparisonTypeCases))]
    public void Compare_HalfArrayPair_RespectsComparisonType_UsesExpectedOutcome(
        ComparisonType comparisonType,
        Half[]? first,
        Half[]? second,
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
    [MemberData(nameof(HalfArrayParamsCases))]
    public void Compare_HalfArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        Half[][]? values,
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
    public void Compare_HalfArrayPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();
        Half[] firstLiteral = [(Half)1, (Half)2];
        Half[] secondLiteral = [(Half)1, (Half)3];

        var result = builder.Compare(firstLiteral, secondLiteral);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("1");
        result.Mismatches[0].Message.ShouldContain("2");
        result.Mismatches[0].Message.ShouldContain("3");
    }
}
