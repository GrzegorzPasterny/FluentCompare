namespace FluentCompare.UnitTests.Decimals;

public class DecimalComparisonTests
{
    private readonly ITestOutputHelper _output;

    public DecimalComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, decimal, decimal, int, int, string?> DecimalPairCases =>
        new()
        {
            { b => b.UseComparisonType(ComparisonType.EqualTo), 42m, 42m, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.EqualTo), 42m, 43m, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), 42m, 43m, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), 42m, 42m, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), 43m, 42m, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), 42m, 43m, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThan), 42m, 43m, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThan), 43m, 42m, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), 42m, 42m, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), 41m, 42m, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), 42m, 42m, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), 43m, 42m, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<decimal[]?, int, int, string?> ParamsDecimalCases =>
        new()
        {
            { null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new[] { 1m }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { new[] { 1m, 1m }, 0, 0, null },
            { new[] { 1m, 2m }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { new[] { 1m, 1m, 2m }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<decimal?, decimal?, int, int, string?> NullableDecimalObjectCases =>
        new()
        {
            { 7m, 7m, 0, 0, null },
            { 1m, 2m, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, decimal, decimal, int, int, string?> DecimalPrecisionCases =>
        new()
        {
            { b => b.WithDoublePrecision(2), 1.234m, 1.2345m, 0, 0, null },
            { b => b.WithDoublePrecision(2), 1.234m, 1.245m, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.WithDoublePrecision(3), 1.234m, 1.2341m, 0, 0, null },
            { b => b.WithDoublePrecision(3), 1.234m, 1.235m, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
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
    [MemberData(nameof(DecimalPairCases))]
    public void Compare_DecimalPair_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        decimal left,
        decimal right,
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
    public void Compare_DecimalPair_NamedVariableInvocation_ShouldIncludeVariableNamesInMismatchMessage()
    {
        var builder = CreateBuilder();
        decimal leftValue = 42m;
        decimal rightValue = 43m;

        var result = builder.Compare(leftValue, rightValue);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain(nameof(leftValue));
        result.Mismatches[0].Message.ShouldContain(nameof(rightValue));
    }

    [Fact]
    public void Compare_DecimalPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare(0m, 1m);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("0");
        result.Mismatches[0].Message.ShouldContain("1");
    }

    [Theory]
    [MemberData(nameof(DecimalPrecisionCases))]
    public void Compare_DecimalPair_WithPrecisionConfigurations_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        decimal left,
        decimal right,
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

    [Theory]
    [MemberData(nameof(ParamsDecimalCases))]
    public void Compare_ParamsDecimal_UsesExpectedOutcome(
        decimal[]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = values is null
            ? builder.Compare((decimal[]?)null)
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
    [MemberData(nameof(NullableDecimalObjectCases))]
    public void Compare_ObjectOverload_NullableDecimal_UsesExpectedOutcome(
        decimal? left,
        decimal? right,
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
