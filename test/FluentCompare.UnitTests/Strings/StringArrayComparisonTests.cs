using Xunit.Abstractions;

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

    [Fact]
    public void Compare_NullArray_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(null as string[][]);
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.WasSuccessful.ShouldBeFalse();
    }

    [Fact]
    public void Compare_NotEnoughArrays_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(new[] { new[] { "a", "b" } });
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.WasSuccessful.ShouldBeFalse();
    }

    [Fact]
    public void Compare_BothNull_ShouldAddWarning()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare((string[])null, (string[])null);
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Warnings.Count.ShouldBe(1);
        result.Errors.ShouldBeEmpty();
        result.Mismatches.ShouldBeEmpty();
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_ReferenceEquals_ShouldReturnEmptyResult()
    {
        // Arrange
        var builder = CreateBuilder();
        var arr = new[] { "x", "y" };

        // Act
        var result = builder.Compare(arr, arr);
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Errors.ShouldBeEmpty();
        result.Mismatches.ShouldBeEmpty();
        result.Warnings.ShouldBeEmpty();
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_FirstArrayNull_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(null, new[] { "a" });
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.AllMatched.ShouldBeFalse();
        result.WasSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Compare_SecondArrayNull_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(new[] { "a" }, null);
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.AllMatched.ShouldBeFalse();
        result.WasSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Compare_DifferentLengths_ShouldAddWarning()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(new[] { "a", "b" }, new[] { "a" }, "stringExpr1", null);
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Warnings.Count.ShouldBe(1);
        result.Errors.ShouldBeEmpty();
        result.Mismatches.ShouldBeEmpty();
        result.AllMatched.ShouldBeTrue();
        result.Warnings.First().Code.ShouldBe(ComparisonErrors.InputArrayLengthsDifferCode);
        result.Warnings.First().Message.ShouldContain("stringExpr1");
        result.Warnings.First().Message.ShouldContain("StringArrayTwo");
    }

    [Fact]
    public void Compare_AllEqual_ShouldAllMatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(new[] { "a", "b" }, new[] { "a", "b" });
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
        result.Errors.ShouldBeEmpty();
        result.Warnings.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_OneElementDifferent_ShouldAddMismatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(new[] { "a", "b" }, new[] { "a", "x" });
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Mismatches.Count.ShouldBe(1);
        result.Errors.ShouldBeEmpty();
        result.AllMatched.ShouldBeFalse();
    }

    [Fact]
    public void Compare_NullElements_ShouldAddWarningsAndMismatches()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(
            new string[] { "a", null, null },
            new string[] { "a", "b", null });
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Warnings.Count.ShouldBe(1);
        result.Mismatches.Count.ShouldBe(1);
        result.AllMatched.ShouldBeFalse();
    }

    [Fact]
    public void Compare_CaseInsensitive_ShouldMatch()
    {
        // Arrange
        var builder = CreateBuilder(ComparisonType.EqualTo, StringComparison.OrdinalIgnoreCase);

        // Act
        var result = builder.Compare(
            new[] { "Hello", "World" },
            new[] { "hello", "world" });
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_Params_ShouldAggregateResults()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(
            new[] { "a", "b" },
            new[] { "a", "b" },
            new[] { "a", "x" });
        _testOutputHelper.WriteLine(result.ToString());

        // Assert
        result.Mismatches.Count.ShouldBe(1);
        result.Errors.ShouldBeEmpty();
        result.Warnings.ShouldBeEmpty();
        result.AllMatched.ShouldBeFalse();
    }

    [Theory]
    [InlineData("a", "b", ComparisonType.LessThan, true)]
    [InlineData("b", "a", ComparisonType.GreaterThan, true)]
    [InlineData("a", "b", ComparisonType.GreaterThan, false)]
    [InlineData("b", "a", ComparisonType.LessThan, false)]
    public void Compare_RespectsComparisonType(string left, string right, ComparisonType type, bool expectedSuccess)
    {
        // Arrange
        var builder = CreateBuilder(type);

        // Act
        var result = builder.Compare(new[] { left }, new[] { right });
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
}
