namespace FluentCompare.UnitTests.Decimals;

public class DecimalArrayComparisonTests
{
    private readonly ITestOutputHelper _output;

    public DecimalArrayComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<decimal[]?, decimal[]?, int, int, string?> DecimalArrayPairCases =>
        new()
        {
            { new[] { 1m, 2m, 3m }, new[] { 1m, 2m, 3m }, 0, 0, null },
            { new[] { 1m, 2m, 3m }, new[] { 1m, 9m, 3m }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { new[] { 1m, 2m, 3m }, new[] { 1m, 2m }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
            { new[] { 1m, 2m }, null, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { null, new[] { 1m, 2m }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
        };

    public static TheoryData<ComparisonType, decimal[]?, decimal[]?, int, int, string?> DecimalArrayPairComparisonTypeCases =>
        new()
        {
            { ComparisonType.EqualTo, new[] { 1m }, new[] { 1m }, 0, 0, null },
            { ComparisonType.EqualTo, new[] { 1m }, new[] { 2m }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.NotEqualTo, new[] { 1m }, new[] { 2m }, 0, 0, null },
            { ComparisonType.NotEqualTo, new[] { 1m }, new[] { 1m }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.GreaterThan, new[] { 2m }, new[] { 1m }, 0, 0, null },
            { ComparisonType.GreaterThan, new[] { 1m }, new[] { 2m }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.LessThan, new[] { 1m }, new[] { 2m }, 0, 0, null },
            { ComparisonType.LessThan, new[] { 2m }, new[] { 1m }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.GreaterThanOrEqualTo, new[] { 1m }, new[] { 1m }, 0, 0, null },
            { ComparisonType.GreaterThanOrEqualTo, new[] { 1m }, new[] { 2m }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.LessThanOrEqualTo, new[] { 1m }, new[] { 1m }, 0, 0, null },
            { ComparisonType.LessThanOrEqualTo, new[] { 2m }, new[] { 1m }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, decimal[][]?, int, int, string?> DecimalArrayParamsCases =>
        new()
        {
            { b => b, null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { 1m } }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new decimal[][] { null!, new[] { 1m } }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new decimal[][] { new[] { 1m }, null! }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new decimal[][] { new[] { 1m }, null! }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { 1m, 2m }, new[] { 1m } }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
            { b => b, new[] { new[] { 1m, 2m }, new[] { 1m, 2m } }, 0, 0, null },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, decimal[]?, decimal[]?, int, int, string?> DecimalArrayPrecisionCases =>
        new()
        {
            { b => b.WithDoublePrecision(2), new[] { 1.234m, 2.345m, 3.456m }, new[] { 1.2345m, 2.3456m, 3.4567m }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.WithDoublePrecision(2), new[] { 1.234m, 2.345m, 3.456m }, new[] { 1.2345m, 2.3456m, 3.467m }, 2, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.WithDoublePrecision(1e-17), new[] { 0.3000000000000001m }, new[] { 0.3m }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
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
    [MemberData(nameof(DecimalArrayPairCases))]
    public void Compare_DecimalArrayPair_UsesExpectedOutcome(
        decimal[]? first,
        decimal[]? second,
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
    [MemberData(nameof(DecimalArrayPairComparisonTypeCases))]
    public void Compare_DecimalArrayPair_RespectsComparisonType_UsesExpectedOutcome(
        ComparisonType comparisonType,
        decimal[]? first,
        decimal[]? second,
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
    [MemberData(nameof(DecimalArrayParamsCases))]
    public void Compare_DecimalArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        decimal[][]? values,
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
    [MemberData(nameof(DecimalArrayPrecisionCases))]
    public void Compare_DecimalArrayPair_WithPrecisionConfigurations_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        decimal[]? first,
        decimal[]? second,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
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

    [Fact]
    public void Compare_DecimalArrayPair_NamedVariableInvocation_ShouldIncludeVariableNamesInMismatchMessage()
    {
        var builder = CreateBuilder();
        decimal[] leftValue = [1m, 2m];
        decimal[] rightValue = [1m, 3m];

        var result = builder.Compare(leftValue, rightValue);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain(nameof(leftValue));
        result.Mismatches[0].Message.ShouldContain(nameof(rightValue));
    }

    [Fact]
    public void Compare_DecimalArrayPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        decimal[] firstLiteral = [1m, 2m];
        decimal[] secondLiteral = [1m, 3m];
        var result = builder.Compare(firstLiteral, secondLiteral);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("1");
        result.Mismatches[0].Message.ShouldContain("2");
        result.Mismatches[0].Message.ShouldContain("3");
    }
}
