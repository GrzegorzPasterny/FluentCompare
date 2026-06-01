namespace FluentCompare.UnitTests.Strings;

public class StringArrayComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public StringArrayComparisonTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo,
        StringComparison stringComparison = StringComparison.Ordinal)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType)
            .UseStringComparisonType(stringComparison);
    }

    public static TheoryData<string[]?, string[]?, int, int, int, string?> StringArrayPairCases =>
        new()
        {
            { new[] { "a", "b" }, new[] { "a", "b" }, 0, 0, 0, null },
            { new[] { "a", "b" }, new[] { "a", "x" }, 1, 0, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { new[] { "a", "b" }, new[] { "a" }, 0, 1, 0, ComparisonErrors.InputArrayLengthsDifferCode },
            { null, null, 0, 0, 1, ComparisonErrors.BothObjectsAreNullCode },
            { null, new[] { "a" }, 1, 0, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { new[] { "a" }, null, 1, 0, 0, ComparisonMismatches.NullPassedAsArgumentCode },
        };

    public static TheoryData<ComparisonType, string[], string[], int, int, string?> StringArrayPairComparisonTypeCases =>
        new()
        {
            { ComparisonType.EqualTo, new[] { "a" }, new[] { "a" }, 0, 0, null },
            { ComparisonType.EqualTo, new[] { "a" }, new[] { "b" }, 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { ComparisonType.NotEqualTo, new[] { "a" }, new[] { "b" }, 0, 0, null },
            { ComparisonType.NotEqualTo, new[] { "a" }, new[] { "a" }, 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { ComparisonType.GreaterThan, new[] { "b" }, new[] { "a" }, 0, 0, null },
            { ComparisonType.GreaterThan, new[] { "a" }, new[] { "b" }, 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { ComparisonType.LessThan, new[] { "a" }, new[] { "b" }, 0, 0, null },
            { ComparisonType.LessThan, new[] { "b" }, new[] { "a" }, 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { ComparisonType.GreaterThanOrEqualTo, new[] { "a" }, new[] { "a" }, 0, 0, null },
            { ComparisonType.GreaterThanOrEqualTo, new[] { "a" }, new[] { "b" }, 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { ComparisonType.LessThanOrEqualTo, new[] { "a" }, new[] { "a" }, 0, 0, null },
            { ComparisonType.LessThanOrEqualTo, new[] { "b" }, new[] { "a" }, 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
        };

    public static TheoryData<string[][]?, int, int, int, string?> StringArrayParamsCases =>
        new()
        {
            { null, 0, 1, 0, ComparisonErrors.NullPassedAsArgumentCode },
            { new[] { new[] { "a", "b" } }, 0, 1, 0, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { new[] { new[] { "a", "b" }, new[] { "a", "b" } }, 0, 0, 0, null },
            { new[] { new[] { "a", "b" }, new[] { "a", "x" } }, 1, 0, 0, ComparisonMismatches<string>.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, string[]?, string[]?, int, int, int, string?> StringArrayComparisonModeCases =>
        new()
        {
            { b => b.UseStringComparisonType(StringComparison.OrdinalIgnoreCase), new[] { "Hello" }, new[] { "hello" }, 0, 0, 0, null },
            { b => b.UseStringComparisonType(StringComparison.Ordinal), new[] { "Hello" }, new[] { "hello" }, 1, 0, 0, ComparisonMismatches<string>.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, string[]?, string[]?, int, int, int, string?> StringArrayDifferentLengthConfigurationCases =>
        new()
        {
            { b => b.DisallowArrayComparisonOfDifferentLengths(), new[] { "a", "b" }, new[] { "a" }, 0, 1, 0, ComparisonErrors.InputArrayLengthsDifferCode },
            { b => b.AllowArrayComparisonOfDifferentLengths(), new[] { "a", "b" }, new[] { "a" }, 0, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, string[]?, string[]?, int, int, int, string?> StringArrayDifferentLengthWithPrefixMismatchCases =>
        new()
        {
            { b => b.AllowArrayComparisonOfDifferentLengths(), new[] { "x", "b" }, new[] { "a" }, 1, 0, 1, ComparisonMismatches<string>.MismatchDetectedCode },
            { b => b.AllowArrayComparisonOfDifferentLengths(), new[] { "a" }, new[] { "x", "b" }, 1, 0, 1, ComparisonMismatches<string>.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, string[]?, string[]?, int, int, int, string?> StringArrayFinishModeCases =>
        new()
        {
            { b => b.FinishComparisonOnFirstMismatch(), new[] { "x", "y", "z" }, new[] { "a", "b", "c" }, 1, 0, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { b => b.FinishComparisonCollectingAllMismatches(), new[] { "x", "y", "z" }, new[] { "a", "b", "c" }, 3, 0, 0, ComparisonMismatches<string>.MismatchDetectedCode },
        };

    private void AssertFirstMismatchCode(ComparisonResult result, string expectedCode)
    {
        _testOutputHelper.WriteLine(result.ToString());
        foreach (var mismatch in result.Mismatches)
        {
            _testOutputHelper.WriteLine($"Mismatch [{mismatch.Code}]: {mismatch.Message}");
        }
        foreach (var error in result.Errors)
        {
            _testOutputHelper.WriteLine($"Error [{error.Code}]: {error.Message}");
        }
        foreach (var warning in result.Warnings)
        {
            _testOutputHelper.WriteLine($"Warning [{warning.Code}]: {warning.Message}");
        }

        result.MismatchCount.ShouldBeGreaterThan(0, result.ToString());
        result.Mismatches[0].Code.ShouldBe(expectedCode, result.Mismatches[0].Message);
    }

    private void AssertFirstErrorCode(ComparisonResult result, string expectedCode)
    {
        _testOutputHelper.WriteLine(result.ToString());
        foreach (var mismatch in result.Mismatches)
        {
            _testOutputHelper.WriteLine($"Mismatch [{mismatch.Code}]: {mismatch.Message}");
        }
        foreach (var error in result.Errors)
        {
            _testOutputHelper.WriteLine($"Error [{error.Code}]: {error.Message}");
        }
        foreach (var warning in result.Warnings)
        {
            _testOutputHelper.WriteLine($"Warning [{warning.Code}]: {warning.Message}");
        }

        result.ErrorCount.ShouldBeGreaterThan(0, result.ToString());
        result.Errors[0].Code.ShouldBe(expectedCode, result.Errors[0].Message);
    }

    private void AssertFirstWarningCode(ComparisonResult result, string expectedCode)
    {
        _testOutputHelper.WriteLine(result.ToString());
        foreach (var mismatch in result.Mismatches)
        {
            _testOutputHelper.WriteLine($"Mismatch [{mismatch.Code}]: {mismatch.Message}");
        }
        foreach (var error in result.Errors)
        {
            _testOutputHelper.WriteLine($"Error [{error.Code}]: {error.Message}");
        }
        foreach (var warning in result.Warnings)
        {
            _testOutputHelper.WriteLine($"Warning [{warning.Code}]: {warning.Message}");
        }

        result.WarningCount.ShouldBeGreaterThan(0, result.ToString());
        result.Warnings[0].Code.ShouldBe(expectedCode, result.Warnings[0].Message);
    }

    [Theory]
    [MemberData(nameof(StringArrayPairCases))]
    public void Compare_StringArrayPair_UsesExpectedOutcome(
        string[]? first,
        string[]? second,
        int expectedMismatches,
        int expectedErrors,
        int expectedWarnings,
        string? expectedCode)
    {
        var builder = CreateBuilder();
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
                AssertFirstWarningCode(result, expectedCode);
            }
        }
    }

    [Theory]
    [MemberData(nameof(StringArrayDifferentLengthWithPrefixMismatchCases))]
    public void Compare_StringArrayPair_DifferentLengthAllowed_StillComparesSharedPrefix(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        string[]? first,
        string[]? second,
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
    [MemberData(nameof(StringArrayFinishModeCases))]
    public void Compare_StringArrayPair_FinishMode_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        string[]? first,
        string[]? second,
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
    [MemberData(nameof(StringArrayDifferentLengthConfigurationCases))]
    public void Compare_StringArrayPair_DifferentLengthConfiguration_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        string[]? first,
        string[]? second,
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
                AssertFirstWarningCode(result, expectedCode);
            }
        }
    }

    [Theory]
    [MemberData(nameof(StringArrayComparisonModeCases))]
    public void Compare_StringArrayPair_StringComparisonMode_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        string[]? first,
        string[]? second,
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
                AssertFirstWarningCode(result, expectedCode);
            }
        }
    }

    [Theory]
    [MemberData(nameof(StringArrayPairComparisonTypeCases))]
    public void Compare_StringArrayPair_RespectsComparisonType_UsesExpectedOutcome(
        ComparisonType comparisonType,
        string[] first,
        string[] second,
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
    [MemberData(nameof(StringArrayParamsCases))]
    public void Compare_StringArrayParams_UsesExpectedOutcome(
        string[][]? values,
        int expectedMismatches,
        int expectedErrors,
        int expectedWarnings,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = values is null
            ? builder.Compare((string[][]?)null)
            : builder.Compare(values);

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
                AssertFirstWarningCode(result, expectedCode);
            }
        }
    }

    [Fact]
    public void Compare_StringArrayPair_NamedVariableInvocation_ShouldIncludeVariableNamesInMismatchMessage()
    {
        var builder = CreateBuilder();
        string[] leftValue = ["a", "b"];
        string[] rightValue = ["a", "x"];

        var result = builder.Compare(leftValue, rightValue);

        AssertFirstMismatchCode(result, ComparisonMismatches<string>.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain(nameof(leftValue));
        result.Mismatches[0].Message.ShouldContain(nameof(rightValue));
    }

    [Fact]
    public void Compare_StringArrayPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare(["a", "b"], ["a", "x"]);

        AssertFirstMismatchCode(result, ComparisonMismatches<string>.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("[\"a\", \"b\"]");
        result.Mismatches[0].Message.ShouldContain("[\"a\", \"x\"]");
    }

    [Fact]
    public void Compare_StringArrayPair_ReferenceEquals_ShouldReturnEmptyResult()
    {
        var builder = CreateBuilder();
        var arr = new[] { "x", "y" };

        var result = builder.Compare(arr, arr);

        _testOutputHelper.WriteLine(result.ToString());
        result.Errors.ShouldBeEmpty();
        result.Mismatches.ShouldBeEmpty();
        result.Warnings.ShouldBeEmpty();
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_StringArrayPair_NullElements_ShouldAddWarningsAndMismatches()
    {
        var builder = CreateBuilder();

        var result = builder.Compare<string[]?>(
            ["a", null!, null!],
            ["a", "b", null!]);

        _testOutputHelper.WriteLine(result.ToString());
        foreach (var mismatch in result.Mismatches)
        {
            _testOutputHelper.WriteLine($"Mismatch [{mismatch.Code}]: {mismatch.Message}");
        }
        foreach (var error in result.Errors)
        {
            _testOutputHelper.WriteLine($"Error [{error.Code}]: {error.Message}");
        }
        foreach (var warning in result.Warnings)
        {
            _testOutputHelper.WriteLine($"Warning [{warning.Code}]: {warning.Message}");
        }

        result.WarningCount.ShouldBe(1);
        result.MismatchCount.ShouldBe(1);
        result.AllMatched.ShouldBeFalse();
    }
}
