namespace FluentCompare.UnitTests.Floats;

public class FloatArrayComparisonTests
{
    private readonly ITestOutputHelper _output;

    public FloatArrayComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<float[]?, float[]?, int, int, string?> FloatArrayPairCases =>
        new()
        {
            { new[] { 1f, 2f, 3f }, new[] { 1f, 2f, 3f }, 0, 0, null },
            { new[] { 1f, 2f, 3f }, new[] { 1f, 9f, 3f }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { new[] { 1f, 2f, 3f }, new[] { 1f, 2f }, 1, 0, ComparisonMismatches.InputArrayLengthsDifferCode },
            { new[] { 1f, 2f }, null, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { null, new[] { 1f, 2f }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
        };

    public static TheoryData<ComparisonType, float[]?, float[]?, int, int, string?> FloatArrayPairComparisonTypeCases =>
        new()
        {
            { ComparisonType.EqualTo, new[] { 1f }, new[] { 1f }, 0, 0, null },
            { ComparisonType.EqualTo, new[] { 1f }, new[] { 2f }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.NotEqualTo, new[] { 1f }, new[] { 2f }, 0, 0, null },
            { ComparisonType.NotEqualTo, new[] { 1f }, new[] { 1f }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.GreaterThan, new[] { 2f }, new[] { 1f }, 0, 0, null },
            { ComparisonType.GreaterThan, new[] { 1f }, new[] { 2f }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.LessThan, new[] { 1f }, new[] { 2f }, 0, 0, null },
            { ComparisonType.LessThan, new[] { 2f }, new[] { 1f }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.GreaterThanOrEqualTo, new[] { 1f }, new[] { 1f }, 0, 0, null },
            { ComparisonType.GreaterThanOrEqualTo, new[] { 1f }, new[] { 2f }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.LessThanOrEqualTo, new[] { 1f }, new[] { 1f }, 0, 0, null },
            { ComparisonType.LessThanOrEqualTo, new[] { 2f }, new[] { 1f }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, float[][]?, int, int, string?> FloatArrayParamsCases =>
        new()
        {
            { b => b, null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { 1f } }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new float[][] { null!, new[] { 1f } }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new float[][] { new[] { 1f }, null! }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new float[][] { new[] { 1f }, null! }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { 1f, 2f }, new[] { 1f } }, 1, 0, ComparisonMismatches.InputArrayLengthsDifferCode },
            { b => b, new[] { new[] { 1f, 2f }, new[] { 1f, 2f } }, 0, 0, null },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, float[]?, float[]?, int, int, string?> FloatArrayPrecisionCases =>
        new()
        {
            { b => b.WithDoublePrecision(2), new[] { 1.234f, 2.345f, 3.456f }, new[] { 1.2345f, 2.3456f, 3.4567f }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.WithDoublePrecision(2), new[] { 1.234f, 2.345f, 3.456f }, new[] { 1.2345f, 2.3456f, 3.467f }, 2, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.WithDoublePrecision(1e-8), new[] { 0.1f + 0.2f }, new[] { 0.3f }, 0, 0, null },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, float[]?, float[]?, int, int, int, string?> FloatArrayDifferentLengthConfigurationCases =>
        new()
        {
            { b => b.DisallowArrayComparisonOfDifferentLengths(), new[] { 1f, 2f, 3f }, new[] { 1f, 2f }, 1, 0, 0, ComparisonMismatches.InputArrayLengthsDifferCode },
            { b => b.AllowArrayComparisonOfDifferentLengths(), new[] { 1f, 2f, 3f }, new[] { 1f, 2f }, 0, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, float[]?, float[]?, int, int, int, string?> FloatArrayDifferentLengthWithPrefixMismatchCases =>
        new()
        {
            { b => b.AllowArrayComparisonOfDifferentLengths(), new[] { 9f, 2f, 3f }, new[] { 1f, 2f }, 1, 0, 1, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, float[]?, float[]?, int, int, int, string?> FloatArrayFinishModeCases =>
        new()
        {
            { b => b.FinishComparisonOnFirstMismatch(), new[] { 9f, 8f, 7f }, new[] { 1f, 2f, 3f }, 1, 0, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.FinishComparisonCollectingAllMismatches(), new[] { 9f, 8f, 7f }, new[] { 1f, 2f, 3f }, 3, 0, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
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
    [MemberData(nameof(FloatArrayPairCases))]
    public void Compare_FloatArrayPair_UsesExpectedOutcome(
        float[]? first,
        float[]? second,
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
    [MemberData(nameof(FloatArrayFinishModeCases))]
    public void Compare_FloatArrayPair_FinishMode_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        float[]? first,
        float[]? second,
        int expectedMismatches,
        int expectedErrors,
        int expectedWarnings,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = builder.Compare(first, second);

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);
        result.WarningCount.ShouldBe(expectedWarnings);

        if (expectedCode is not null)
        {
            AssertFirstMismatchCode(result, expectedCode);
        }
    }

    [Theory]
    [MemberData(nameof(FloatArrayDifferentLengthWithPrefixMismatchCases))]
    public void Compare_FloatArrayPair_DifferentLengthAllowed_StillComparesSharedPrefix(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        float[]? first,
        float[]? second,
        int expectedMismatches,
        int expectedErrors,
        int expectedWarnings,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = builder.Compare(first, second);

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);
        result.WarningCount.ShouldBe(expectedWarnings);

        if (expectedCode is not null)
        {
            AssertFirstMismatchCode(result, expectedCode);
        }
    }

    [Theory]
    [MemberData(nameof(FloatArrayDifferentLengthConfigurationCases))]
    public void Compare_FloatArrayPair_DifferentLengthConfiguration_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        float[]? first,
        float[]? second,
        int expectedMismatches,
        int expectedErrors,
        int expectedWarnings,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = builder.Compare(first, second);

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);
        result.WarningCount.ShouldBe(expectedWarnings);

        if (expectedCode is not null)
        {
            if (expectedErrors > 0)
            {
                AssertFirstErrorCode(result, expectedCode);
            }
            else if (expectedMismatches > 0)
            {
                AssertFirstMismatchCode(result, expectedCode);
            }
            else
            {
                _output.WriteLine(result.ToString());
                result.WarningCount.ShouldBeGreaterThan(0, result.ToString());
                result.Warnings[0].Code.ShouldBe(expectedCode, result.Warnings[0].Message);
            }
        }
    }

    [Theory]
    [MemberData(nameof(FloatArrayPairComparisonTypeCases))]
    public void Compare_FloatArrayPair_RespectsComparisonType_UsesExpectedOutcome(
        ComparisonType comparisonType,
        float[]? first,
        float[]? second,
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
    [MemberData(nameof(FloatArrayParamsCases))]
    public void Compare_FloatArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        float[][]? values,
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
    [MemberData(nameof(FloatArrayPrecisionCases))]
    public void Compare_FloatArrayPair_WithPrecisionConfigurations_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        float[]? first,
        float[]? second,
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
    public void Compare_FloatArrayPair_NamedVariableInvocation_ShouldIncludeVariableNamesInMismatchMessage()
    {
        var builder = CreateBuilder();
        float[] leftValue = [1f, 2f];
        float[] rightValue = [1f, 3f];

        var result = builder.Compare(leftValue, rightValue);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain(nameof(leftValue));
        result.Mismatches[0].Message.ShouldContain(nameof(rightValue));
    }

    [Fact]
    public void Compare_FloatArrayPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare([1f, 2f], [1f, 3f]);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("1");
        result.Mismatches[0].Message.ShouldContain("2");
        result.Mismatches[0].Message.ShouldContain("3");
    }
}
