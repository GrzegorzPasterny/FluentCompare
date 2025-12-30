using Xunit.Abstractions;

namespace FluentCompare.UnitTests.Bools;

public class BoolComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public BoolComparisonTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    [Fact]
    public void Compare_BothNull_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(null as bool?);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.NullPassedAsArgumentCode);
        result.Mismatches.ShouldBeEmpty();
        result.Warnings.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_SingleBool_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(true);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.NotEnoughObjectsToCompareCode);
        result.Mismatches.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_EqualBools_ShouldAllMatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(true, true);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_NotEqualBools_ShouldAddMismatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(true, false);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.Mismatches.Count.ShouldBe(1);
        result.Mismatches[0].Code.ShouldBe(ComparisonMismatches.Bool.MismatchDetectedCode);
    }

    [Theory]
    [InlineData(true, false, ComparisonType.NotEqualTo, true)]
    [InlineData(false, true, ComparisonType.NotEqualTo, true)]
    [InlineData(true, true, ComparisonType.EqualTo, true)]
    [InlineData(false, false, ComparisonType.EqualTo, true)]
    [InlineData(true, false, ComparisonType.EqualTo, false)]
    [InlineData(true, true, ComparisonType.NotEqualTo, false)]
    [InlineData(true, false, ComparisonType.GreaterThan, true)]
    [InlineData(true, true, ComparisonType.GreaterThanOrEqualTo, true)]
    [InlineData(true, true, ComparisonType.GreaterThan, false)]
    [InlineData(false, true, ComparisonType.LessThan, true)]
    [InlineData(false, false, ComparisonType.LessThanOrEqualTo, true)]
    [InlineData(false, false, ComparisonType.LessThan, false)]
    public void Compare_RespectsComparisonType(
        bool left, bool right, ComparisonType type, bool expectedMatch)
    {
        // Arrange
        var builder = CreateBuilder(type);

        // Act
        var result = builder.Compare(left, right);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        if (expectedMatch)
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
    public void Compare_BoolArrays_AllEqual_ShouldMatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare([true, false, true], [true, false, true]);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_BoolArrays_DifferentLengths_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare([true, false], [true, false, true]);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.InputArrayLengthsDifferCode);
    }

    [Fact]
    public void Compare_BoolArrays_Mismatch_ShouldDetectMismatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare([true, false, true], [true, true, true]);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.Mismatches.Count.ShouldBe(1);
        result.Mismatches[0].Code.ShouldBe(ComparisonMismatches.Bool.MismatchDetectedCode);
    }
}
