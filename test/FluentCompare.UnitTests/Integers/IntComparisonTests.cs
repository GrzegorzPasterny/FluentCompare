namespace FluentCompare.UnitTests.Integers;

public class IntComparisonTests
{
    private readonly ITestOutputHelper _output;

    public IntComparisonTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, int, int, int, int, string?> IntPairCases =>
        new()
        {
            { b => b.UseComparisonType(ComparisonType.EqualTo), 42, 42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.EqualTo), 42, 43, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), 42, 43, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.NotEqualTo), 42, 42, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), 43, 42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThan), 42, 43, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThan), 42, 43, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThan), 43, 42, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), 42, 42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.GreaterThanOrEqualTo), 41, 42, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), 42, 42, 0, 0, null },
            { b => b.UseComparisonType(ComparisonType.LessThanOrEqualTo), 43, 42, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
        };

    public static TheoryData<ComparisonType, int[]?, int, int, string?> ParamsIntComparisonTypeCases =>
        new()
        {
            { ComparisonType.EqualTo, new[] { 1, 1 }, 0, 0, null },
            { ComparisonType.EqualTo, new[] { 1, 2 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { ComparisonType.NotEqualTo, new[] { 1, 2 }, 0, 0, null },
            { ComparisonType.NotEqualTo, new[] { 1, 1 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { ComparisonType.GreaterThan, new[] { 2, 1 }, 0, 0, null },
            { ComparisonType.GreaterThan, new[] { 1, 2 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { ComparisonType.LessThan, new[] { 1, 2 }, 0, 0, null },
            { ComparisonType.LessThan, new[] { 2, 1 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { ComparisonType.GreaterThanOrEqualTo, new[] { 1, 1 }, 0, 0, null },
            { ComparisonType.GreaterThanOrEqualTo, new[] { 1, 2 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { ComparisonType.LessThanOrEqualTo, new[] { 1, 1 }, 0, 0, null },
            { ComparisonType.LessThanOrEqualTo, new[] { 2, 1 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
        };

    public static TheoryData<int[]?, int, int, string?> ParamsIntCases =>
        new()
        {
            { null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new[] { 1 }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { new[] { 1, 1 }, 0, 0, null },
            { new[] { 1, 2 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { new[] { 1, 1, 2 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
        };

    public static TheoryData<int?, int?, int, int, string?> NullableIntObjectCases =>
        new()
        {
            { 7, 7, 0, 0, null },
            { 1, 2, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, int?, int?, int, int, string?> NullableIntPairNullabilityCases =>
        new()
        {
            { b => b.DisallowNullComparison(), 12, null, 1, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
            { b => b.DisallowNullComparison(), null, 12, 1, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
            { b => b.DisallowNullComparison(), null, null, 0, 1, ComparisonErrors.BothObjectsAreNullCode },
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
    [MemberData(nameof(IntPairCases))]
    public void Compare_IntPair_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        int left,
        int right,
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
    [MemberData(nameof(ParamsIntComparisonTypeCases))]
    public void Compare_ParamsInt_RespectsComparisonType_UsesExpectedOutcome(
        ComparisonType comparisonType,
        int[]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder(comparisonType);
        var result = values is null
            ? builder.Compare((int[]?)null)
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

    [Fact]
    public void Compare_IntPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare(0, 1);

        result.MismatchCount.ShouldBe(1);
        result.ErrorCount.ShouldBe(0);
        AssertFirstMismatchCode(result, ComparisonMismatches<int>.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("0");
        result.Mismatches[0].Message.ShouldContain("1");
    }

    [Theory]
    [MemberData(nameof(ParamsIntCases))]
    public void Compare_ParamsInt_UsesExpectedOutcome(
        int[]? values,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = values is null
            ? builder.Compare((int[]?)null)
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
    [MemberData(nameof(NullableIntObjectCases))]
    public void Compare_ObjectOverload_NullableInt_UsesExpectedOutcome(
        int? left,
        int? right,
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
    [MemberData(nameof(NullableIntPairNullabilityCases))]
    public void Compare_NullableIntPair_Nullability_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        int? left,
        int? right,
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
