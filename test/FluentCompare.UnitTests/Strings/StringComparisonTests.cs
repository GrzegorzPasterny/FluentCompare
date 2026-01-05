using Xunit.Abstractions;

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

    [Fact]
    public void Compare_BothStringsNull_ShouldAddWarning()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare<string?>(null, null);
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Warnings.Count.ShouldBe(1);
        result.Errors.Count.ShouldBe(0);
        result.Mismatches.Count.ShouldBe(0);
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_OneStringNull_ShouldAddMismatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(null, "abc");
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Mismatches.Count.ShouldBe(1);
        result.Errors.ShouldBeEmpty();
        result.Warnings.ShouldBeEmpty();
        result.AllMatched.ShouldBeFalse();
        result.WasSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Compare_EqualStrings_ShouldAllMatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare("test", "test");
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
        result.Errors.ShouldBeEmpty();
        result.Warnings.ShouldBeEmpty();
        result.WasSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Compare_NotEqualStrings_ShouldAddMismatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare("test", "different");
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Mismatches.Count.ShouldBe(1);
        result.AllMatched.ShouldBeFalse();
        result.WasSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Compare_CaseInsensitive_ShouldMatch()
    {
        // Arrange
        var builder = CreateBuilder(ComparisonType.EqualTo, System.StringComparison.OrdinalIgnoreCase);

        // Act
        var result = builder.Compare("Hello", "hello");
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_Params_EqualStrings_ShouldAllMatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare("apple", "apple", "apple");
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.AllMatched.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
        result.Mismatches.ShouldBeEmpty();
        result.Warnings.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_Params_MismatchedStrings_ShouldAddMismatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare("apple", "banana", "apple");
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Mismatches.Count.ShouldBeGreaterThan(0);
        result.AllMatched.ShouldBeFalse();
    }

    [Fact]
    public void Compare_Params_NullArray_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(null as string[]);
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.WasSuccessful.ShouldBeFalse();
        result.AllMatched.ShouldBeFalse();
    }

    [Fact]
    public void Compare_Params_NotEnoughElements_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare("onlyOne");
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.WasSuccessful.ShouldBeFalse();
        result.AllMatched.ShouldBeFalse();
    }

    [Fact]
    public void Compare_Params_WithOneNull_ShouldAddMismatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare("first", null);
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Mismatches.Count.ShouldBe(1);
        result.Errors.ShouldBeEmpty();
        result.AllMatched.ShouldBeFalse();
    }

    [Fact]
    public void Compare_Params_AllNull_ShouldAddWarning()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare<string[]?>(null, null, null, null, null, null, null);
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Warnings.Count.ShouldBe(6);
        result.Errors.ShouldBeEmpty();
        result.Mismatches.ShouldBeEmpty();
        result.AllMatched.ShouldBeTrue();
    }

    [Theory]
    [InlineData("a", "b", ComparisonType.LessThan, true)]
    [InlineData("b", "a", ComparisonType.GreaterThan, true)]
    [InlineData("a", "b", ComparisonType.GreaterThan, false)]
    [InlineData("b", "a", ComparisonType.LessThan, false)]
    public void Compare_RespectsComparisonType(string left, string right, ComparisonType type, bool expectedSuccess)
    {
        // Arrange
        var builder = CreateBuilder(type, System.StringComparison.Ordinal);

        // Act
        var result = builder.Compare(left, right);
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        if (expectedSuccess)
        {
            result.AllMatched.ShouldBeTrue();
            result.Mismatches.ShouldBeEmpty();
        }
        else
        {
            result.AllMatched.ShouldBeFalse();
            result.Mismatches.Count.ShouldBeGreaterThan(0);
        }
    }

    [Fact]
    public void Compare_OrdinalVsIgnoreCase_ShouldBehaveDifferently()
    {
        // Arrange
        var ordinal = CreateBuilder(ComparisonType.EqualTo, System.StringComparison.Ordinal);
        var ignoreCase = CreateBuilder(ComparisonType.EqualTo, System.StringComparison.OrdinalIgnoreCase);

        // Act
        var ordinalResult = ordinal.Compare("a", "A");
        var ignoreCaseResult = ignoreCase.Compare("a", "A");
        _testOutputHelper.WriteLine(ordinalResult.ToString());
        _testOutputHelper.WriteLine(ignoreCaseResult.ToString());

        // Assert
        ordinalResult.AllMatched.ShouldBeFalse();
        ignoreCaseResult.AllMatched.ShouldBeTrue();
    }
}
