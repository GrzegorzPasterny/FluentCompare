namespace FluentCompare.UnitTests.Floats;

public class FloatComparisonTests
{
    private readonly ITestOutputHelper _output;

    public FloatComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, float, float, int, int, string?> FloatPairCases =>
        new()
        {
            { b => b.UseComparisonType(ComparisonType.EqualTo), 42f, 42f, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.EqualTo), 42f, 43f, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), 42f, 43f, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), 42f, 42f, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), 43f, 42f, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), 42f, 43f, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThan), 42f, 43f, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThan), 43f, 42f, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), 42f, 42f, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), 41f, 42f, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), 42f, 42f, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), 43f, 42f, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<float[]?, int, int, string?> ParamsFloatCases =>
        new()
        {
            { null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new[] { 1f }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { new[] { 1f, 1f }, 0, 0, null },
            { new[] { 1f, 2f }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { new[] { 1f, 1f, 2f }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<float?, float?, int, int, string?> NullableFloatObjectCases =>
        new()
        {
            { 7f, 7f, 0, 0, null },
            { 1f, 2f, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, float?, float?, int, int, string?> NullableFloatPairNullabilityCases =>
        new()
        {
            { b => b.DisallowNullComparison(), 12.3f, null, 1, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
            { b => b.DisallowNullComparison(), null, 12.3f, 1, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
            { b => b.DisallowNullComparison(), null, null, 0, 1, ComparisonErrors.BothObjectsAreNullCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, float, float, int, int, string?> FloatPrecisionCases =>
        new()
        {
            { b => b.WithDoublePrecision(2), 1.234f, 1.2345f, 0, 0, null },
            { b => b.WithDoublePrecision(2), 1.234f, 1.245f, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.WithDoublePrecision(3), 1.234f, 1.2341f, 0, 0, null },
            { b => b.WithDoublePrecision(3), 1.234f, 1.235f, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
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
    [MemberData(nameof(FloatPairCases))]
    public void Compare_FloatPair_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        float left,
        float right,
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
    public void Compare_FloatPair_NamedVariableInvocation_ShouldIncludeVariableNamesInMismatchMessage()
    {
        var builder = CreateBuilder();
        float leftValue = 42f;
        float rightValue = 43f;

        var result = builder.Compare(leftValue, rightValue);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain(nameof(leftValue));
        result.Mismatches[0].Message.ShouldContain(nameof(rightValue));
    }

    [Fact]
    public void Compare_FloatPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare(0f, 1f);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("0");
        result.Mismatches[0].Message.ShouldContain("1");
    }

    [Theory]
    [MemberData(nameof(FloatPrecisionCases))]
    public void Compare_FloatPair_WithPrecisionConfigurations_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        float left,
        float right,
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
    [MemberData(nameof(ParamsFloatCases))]
    public void Compare_ParamsFloat_UsesExpectedOutcome(
        float[]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = values is null
            ? builder.Compare((float[]?)null)
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
    [MemberData(nameof(NullableFloatObjectCases))]
    public void Compare_ObjectOverload_NullableFloat_UsesExpectedOutcome(
        float? left,
        float? right,
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

    [Theory]
    [MemberData(nameof(NullableFloatPairNullabilityCases))]
    public void Compare_NullableFloatPair_Nullability_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        float? left,
        float? right,
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
}
