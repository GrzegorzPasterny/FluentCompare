namespace FluentCompare.UnitTests.Strings;

public class StringComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public StringComparisonTests(ITestOutputHelper testOutputHelper)
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

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, string?, string?, int, int, int, string?> StringPairCases =>
        new()
        {
            { b => b, "test", "test", 0, 0, 0, null },
            { b => b, "test", "different", 1, 0, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { b => b, null, "abc", 1, 0, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b, null, null, 0, 0, 1, ComparisonErrors.BothObjectsAreNullCode },
            { b => b.UseStringComparisonType(System.StringComparison.OrdinalIgnoreCase), "Hello", "hello", 0, 0, 0, null },
        };

    public static TheoryData<ComparisonType, string, string, int, int, string?> StringPairComparisonTypeCases =>
        new()
        {
            { ComparisonType.EqualTo, "a", "a", 0, 0, null },
            { ComparisonType.EqualTo, "a", "b", 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { ComparisonType.NotEqualTo, "a", "b", 0, 0, null },
            { ComparisonType.NotEqualTo, "a", "a", 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { ComparisonType.GreaterThan, "b", "a", 0, 0, null },
            { ComparisonType.GreaterThan, "a", "b", 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { ComparisonType.LessThan, "a", "b", 0, 0, null },
            { ComparisonType.LessThan, "b", "a", 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { ComparisonType.GreaterThanOrEqualTo, "a", "a", 0, 0, null },
            { ComparisonType.GreaterThanOrEqualTo, "a", "b", 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { ComparisonType.LessThanOrEqualTo, "a", "a", 0, 0, null },
            { ComparisonType.LessThanOrEqualTo, "b", "a", 1, 0, ComparisonMismatches<string>.MismatchDetectedCode },
        };

    public static TheoryData<string[]?, int, int, int, string?> ParamsStringCases =>
        new()
        {
            { null, 0, 1, 0, ComparisonErrors.NullPassedAsArgumentCode },
            { new[] { "onlyOne" }, 0, 1, 0, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { new[] { "apple", "apple", "apple" }, 0, 0, 0, null },
            { new[] { "apple", "banana", "apple" }, 2, 0, 0, ComparisonMismatches<string>.MismatchDetectedCode },
            { new string?[] { "first", null! }!, 1, 0, 0, ComparisonMismatches.NullPassedAsArgumentCode },
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
    [MemberData(nameof(StringPairCases))]
    public void Compare_StringPair_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        string? left,
        string? right,
        int expectedMismatches,
        int expectedErrors,
        int expectedWarnings,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = builder.Compare(left, right);

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
    [MemberData(nameof(StringPairComparisonTypeCases))]
    public void Compare_StringPair_RespectsComparisonType_UsesExpectedOutcome(
        ComparisonType comparisonType,
        string left,
        string right,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder(comparisonType, System.StringComparison.Ordinal);
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
    public void Compare_StringPair_NamedVariableInvocation_ShouldIncludeVariableNamesInMismatchMessage()
    {
        var builder = CreateBuilder();
        string leftValue = "test";
        string rightValue = "different";

        var result = builder.Compare(leftValue, rightValue);

        AssertFirstMismatchCode(result, ComparisonMismatches<string>.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain(nameof(leftValue));
        result.Mismatches[0].Message.ShouldContain(nameof(rightValue));
    }

    [Fact]
    public void Compare_StringPair_LiteralInvocation_ShouldUseLiteralExpressionInMismatchMessage()
    {
        var builder = CreateBuilder();

        var result = builder.Compare("a", "b");

        AssertFirstMismatchCode(result, ComparisonMismatches<string>.MismatchDetectedCode);
        result.Mismatches[0].Message.ShouldContain("a");
        result.Mismatches[0].Message.ShouldContain("b");
    }

    [Theory]
    [MemberData(nameof(ParamsStringCases))]
    public void Compare_ParamsString_UsesExpectedOutcome(
        string[]? values,
        int expectedMismatches,
        int expectedErrors,
        int expectedWarnings,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = values is null
            ? builder.Compare((string[]?)null)
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
    public void Compare_ParamsString_AllNull_ShouldAddWarnings()
    {
        var builder = CreateBuilder();

        var result = builder.Compare<string[]?>(null, null, null, null, null, null, null);

        result.WarningCount.ShouldBe(6);
        result.ErrorCount.ShouldBe(0);
        result.MismatchCount.ShouldBe(0);
        AssertFirstWarningCode(result, ComparisonErrors.BothObjectsAreNullCode);
    }
}
