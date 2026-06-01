namespace FluentCompare.UnitTests.Doubles;

public class DoubleArrayComparisonTests
{
    private readonly ITestOutputHelper _output;

    public DoubleArrayComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<double[]?, double[]?, int, int, string?> DoubleArrayPairCases =>
        new()
        {
            { new[] { 1.0, 2.0, 3.0 }, new[] { 1.0, 2.0, 3.0 }, 0, 0, null },
            { new[] { 1.0, 2.0, 3.0 }, new[] { 1.0, 9.0, 3.0 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { new[] { 1.0, 2.0, 3.0 }, new[] { 1.0, 2.0 }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
            { new[] { 1.0, 2.0 }, null, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { null, new[] { 1.0, 2.0 }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
        };

    public static TheoryData<ComparisonType, double[]?, double[]?, int, int, string?> DoubleArrayPairComparisonTypeCases =>
        new()
        {
            { ComparisonType.EqualTo, new[] { 1.0 }, new[] { 1.0 }, 0, 0, null },
            { ComparisonType.EqualTo, new[] { 1.0 }, new[] { 2.0 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.NotEqualTo, new[] { 1.0 }, new[] { 2.0 }, 0, 0, null },
            { ComparisonType.NotEqualTo, new[] { 1.0 }, new[] { 1.0 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.GreaterThan, new[] { 2.0 }, new[] { 1.0 }, 0, 0, null },
            { ComparisonType.GreaterThan, new[] { 1.0 }, new[] { 2.0 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.LessThan, new[] { 1.0 }, new[] { 2.0 }, 0, 0, null },
            { ComparisonType.LessThan, new[] { 2.0 }, new[] { 1.0 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.GreaterThanOrEqualTo, new[] { 1.0 }, new[] { 1.0 }, 0, 0, null },
            { ComparisonType.GreaterThanOrEqualTo, new[] { 1.0 }, new[] { 2.0 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { ComparisonType.LessThanOrEqualTo, new[] { 1.0 }, new[] { 1.0 }, 0, 0, null },
            { ComparisonType.LessThanOrEqualTo, new[] { 2.0 }, new[] { 1.0 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, double[][]?, int, int, string?> DoubleArrayParamsCases =>
        new()
        {
            { b => b, null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { 1.0 } }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new double[][] { null!, new[] { 1.0 } }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new double[][] { new[] { 1.0 }, null! }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new double[][] { new[] { 1.0 }, null! }, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new[] { new[] { 1.0, 2.0 }, new[] { 1.0 } }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
            { b => b, new[] { new[] { 1.0, 2.0 }, new[] { 1.0, 2.0 } }, 0, 0, null },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, double[]?, double[]?, int, int, string?> DoubleArrayPrecisionCases =>
        new()
        {
            { b => b.WithDoublePrecision(2), new[] { 1.234, 2.345, 3.456 }, new[] { 1.2345, 2.3456, 3.4567 }, 0, 0, null },
            { b => b.WithDoublePrecision(2), new[] { 1.234, 2.345, 3.456 }, new[] { 1.2345, 2.3456, 3.467 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
            { b => b.WithDoublePrecision(1e-17), new[] { 0.1 + 0.2 }, new[] { 0.3 }, 1, 0, ComparisonMismatches.Floats.MismatchDetectedCode },
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

    private void AssertFirstWarningCode(ComparisonResult result, string expectedCode)
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

        result.WarningCount.ShouldBeGreaterThan(0, result.ToString());
        result.Warnings[0].Code.ShouldBe(expectedCode, result.Warnings[0].Message);
    }

    [Theory]
    [MemberData(nameof(DoubleArrayPairCases))]
    public void Compare_DoubleArrayPair_UsesExpectedOutcome(
        double[]? first,
        double[]? second,
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
            else if (result.WarningCount > 0)
            {
                AssertFirstWarningCode(result, expectedCode);
            }
            else
            {
                AssertFirstMismatchCode(result, expectedCode);
            }
        }
    }

    [Theory]
    [MemberData(nameof(DoubleArrayPairComparisonTypeCases))]
    public void Compare_DoubleArrayPair_RespectsComparisonType_UsesExpectedOutcome(
        ComparisonType comparisonType,
        double[]? first,
        double[]? second,
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
            else if (result.WarningCount > 0)
            {
                AssertFirstWarningCode(result, expectedCode);
            }
            else
            {
                AssertFirstMismatchCode(result, expectedCode);
            }
        }
    }

    [Theory]
    [MemberData(nameof(DoubleArrayParamsCases))]
    public void Compare_DoubleArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        double[][]? values,
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
    [MemberData(nameof(DoubleArrayPrecisionCases))]
    public void Compare_DoubleArrayPair_WithPrecisionConfigurations_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        double[]? first,
        double[]? second,
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
    public void Compare_DoubleArrayPair_NamedVariableInvocation_ShouldIncludeVariableNamesInMismatchMessage()
    {
        var builder = CreateBuilder();
        double[] leftValue = [1.0, 2.0];
        double[] rightValue = [1.0, 3.0];

        var result = builder.Compare(leftValue, rightValue);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain(nameof(leftValue));
        result.Mismatches[0].Message.ShouldContain(nameof(rightValue));
    }

    [Fact]
    public void Compare_DoubleArrayPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare([1.0, 2.0], [1.0, 3.0]);

        AssertFirstMismatchCode(result, ComparisonMismatches.Floats.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("1");
        result.Mismatches[0].Message.ShouldContain("2");
        result.Mismatches[0].Message.ShouldContain("3");
    }
}
