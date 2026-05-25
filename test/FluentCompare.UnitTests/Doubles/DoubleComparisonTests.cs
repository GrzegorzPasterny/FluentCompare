namespace FluentCompare.UnitTests.Doubles;

public class DoubleComparisonTests
{
    private readonly ITestOutputHelper _output;

    public DoubleComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, double, double, int, int, string?> DoublePairCases =>
        new()
        {
            { b => b.UseComparisonType(ComparisonType.EqualTo), 42.0, 42.0, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.EqualTo), 42.0, 43.0, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), 42.0, 43.0, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), 42.0, 42.0, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), 43.0, 42.0, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), 42.0, 43.0, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThan), 42.0, 43.0, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThan), 43.0, 42.0, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), 42.0, 42.0, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), 41.0, 42.0, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), 42.0, 42.0, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), 43.0, 42.0, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<double[]?, int, int, string?> ParamsDoubleCases =>
        new()
        {
            { null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new[] { 1.0 }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { new[] { 1.0, 1.0 }, 0, 0, null },
            { new[] { 1.0, 2.0 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { new[] { 1.0, 1.0, 2.0 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<double?, double?, int, int, string?> NullableDoubleObjectCases =>
        new()
        {
            { 7.0, 7.0, 0, 0, null },
            { 1.0, 2.0, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, double, double, int, int, string?> DoublePrecisionCases =>
        new()
        {
            { b => b.WithDoublePrecision(2), 1.234, 1.2345, 0, 0, null },
            { b => b.WithDoublePrecision(2), 1.234, 1.245, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.WithDoublePrecision(3), 1.234, 1.2341, 0, 0, null },
            { b => b.WithDoublePrecision(3), 1.234, 1.235, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
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
    [MemberData(nameof(DoublePairCases))]
    public void Compare_DoublePair_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        double left,
        double right,
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
    public void Compare_DoublePair_NamedVariableInvocation_ShouldIncludeVariableNamesInMismatchMessage()
    {
        var builder = CreateBuilder();
        double leftValue = 42.0;
        double rightValue = 43.0;

        var result = builder.Compare(leftValue, rightValue);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain(nameof(leftValue));
        result.Mismatches[0].Message.ShouldContain(nameof(rightValue));
    }

    [Fact]
    public void Compare_DoublePair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare(0.0, 1.0);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("0");
        result.Mismatches[0].Message.ShouldContain("1");
    }

    [Theory]
    [MemberData(nameof(DoublePrecisionCases))]
    public void Compare_DoublePair_WithPrecisionConfigurations_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        double left,
        double right,
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
    [MemberData(nameof(ParamsDoubleCases))]
    public void Compare_ParamsDouble_UsesExpectedOutcome(
        double[]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = values is null
            ? builder.Compare((double[]?)null)
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
    [MemberData(nameof(NullableDoubleObjectCases))]
    public void Compare_ObjectOverload_NullableDouble_UsesExpectedOutcome(
        double? left,
        double? right,
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
