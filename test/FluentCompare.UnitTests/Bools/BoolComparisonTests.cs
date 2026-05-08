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
        var builder = CreateBuilder();
        var result = builder.Compare([true, false, true], [true, true, true]);
        _testOutputHelper.WriteLine(result.ToString());
        result.Mismatches.Count.ShouldBe(1);
        result.Mismatches[0].Code.ShouldBe(ComparisonMismatches.Bool.MismatchDetectedCode);
    }

    [Fact]
    public void Compare_BoolArray_Null_ShouldAddError()
    {
        var builder = CreateBuilder();
        var result = builder.Compare(null as bool[]);
        _testOutputHelper.WriteLine(result.ToString());
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.NullPassedAsArgumentCode);
    }

    [Fact]
    public void Compare_BoolArray_NotEnoughElements_ShouldAddError()
    {
        var builder = CreateBuilder();
        var result = builder.Compare(new[] { true });
        _testOutputHelper.WriteLine(result.ToString());
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.NotEnoughObjectsToCompareCode);
    }

    [Fact]
    public void Compare_BoolJaggedArray_Null_ShouldAddError()
    {
        var builder = CreateBuilder();
        var result = builder.Compare(null as bool[][]);
        _testOutputHelper.WriteLine(result.ToString());
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.NullPassedAsArgumentCode);
    }

    [Fact]
    public void Compare_BoolJaggedArray_NotEnoughElements_ShouldAddError()
    {
        var builder = CreateBuilder();
        var result = builder.Compare(new[] { new[] { true } });
        _testOutputHelper.WriteLine(result.ToString());
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.NotEnoughObjectsToCompareCode);
    }

    [Fact]
    public void Compare_BoolJaggedArray_InnerNull_ShouldAddError()
    {
        var builder = CreateBuilder();
        var arr = new bool[][] { null!, new[] { true } };
        var result = builder.Compare(arr);
        _testOutputHelper.WriteLine(result.ToString());
        result.MismatchCount.ShouldBe(1);
        result.Mismatches[0].Code.ShouldBe(ComparisonMismatches.NullPassedAsArgumentCode);
    }

    [Fact]
    public void Compare_BoolJaggedArray_InnerLengthMismatch_ShouldAddError()
    {
        var builder = CreateBuilder();
        var arr1 = new[] { new[] { true, false }, new[] { true } };
        var result = builder.Compare(arr1);
        _testOutputHelper.WriteLine(result.ToString());
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.InputArrayLengthsDifferCode);
    }

    [Fact]
    public void Compare_BoolJaggedArray_InnerMismatch_ShouldDetectMismatch()
    {
        var builder = CreateBuilder();
        var arr = new[] { new[] { true, false }, new[] { true, true } };
        var result = builder.Compare(arr);
        _testOutputHelper.WriteLine(result.ToString());
        result.Mismatches.Count.ShouldBe(1);
        result.Mismatches[0].Code.ShouldBe(ComparisonMismatches.Bool.MismatchDetectedCode);
    }

    [Fact]
    public void Compare_BoolJaggedArray_AllEqual_ShouldMatch()
    {
        var builder = CreateBuilder();
        var arr = new[] { new[] { true, false }, new[] { true, false } };
        var result = builder.Compare(arr);
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_BoolArray_ReferenceEquals_ShouldReturnAllMatched()
    {
        var builder = CreateBuilder();
        var arr = new[] { true, false };
        var result = builder.Compare(arr, arr);
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_BoolJaggedArray_ReferenceEquals_ShouldReturnAllMatched()
    {
        var builder = CreateBuilder();
        var arr = new[] { new[] { true, false } };
        var result = builder.Compare(arr, arr);
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
        result.Errors.ShouldBeEmpty();
    }
}
