using Xunit.Abstractions;

namespace FluentCompare.UnitTests.Objects;

public class ParamsObjectsComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ParamsObjectsComparisonTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData([null, null, null])]
    public void Compare_ParamsObjects_AllShouldMatch(params object[] objects)
    {
        // Arrange
        var builder = ComparisonBuilder.Create();
        // Act
        var result = builder.Compare(objects);
        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null, null, null)]
    public void Compare_ParamsNullableObject_AllShouldMatch(params object?[] objects)
    {
        // Arrange
        var builder = ComparisonBuilder.Create();
        // Act
        var result = builder.Compare(objects);
        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData(1, 1, 1)]
    public void Compare_ParamsObject_AllShouldMatch(object obj1, object obj2, object obj3)
    {
        // Arrange
        var builder = ComparisonBuilder.Create();
        // Act
        var result = builder.Compare(obj1, obj2, obj3);
        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null, 1, null, nameof(ComparisonMismatches.Object.MismatchDetectedByNullCode), ComplexTypesComparisonMode.PropertyEquality)]
    [InlineData(1, null, 1, nameof(ComparisonMismatches.Object.MismatchDetectedByNullCode), ComplexTypesComparisonMode.PropertyEquality)]
    [InlineData(null, 1, null, nameof(ComparisonMismatches.Object.MismatchDetectedByReferenceCode), ComplexTypesComparisonMode.ReferenceEquality)]
    [InlineData(1, null, 1, nameof(ComparisonMismatches.Object.MismatchDetectedByReferenceCode), ComplexTypesComparisonMode.ReferenceEquality)]
    public void Compare_ParamsObject_ShouldReturnMismatch(
        object obj1, object obj2, object obj3, string mismatchCode, ComplexTypesComparisonMode complexTypesComparisonMode)
    {
        // Arrange
        var builder = ComparisonBuilder
            .Create()
            .UseComplexTypeComparisonMode(complexTypesComparisonMode);
        // Act
        var result = builder.Compare(obj1, obj2, obj3);
        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.MismatchCount.ShouldBeGreaterThan(0);
        result.Mismatches[0].Code.ShouldContain(string.Concat(mismatchCode.SkipLast(4))); // Skip "Code" suffix
    }

    [Theory(Skip = "Not implemented")]
    [InlineData(null, null, null)]
    public void Compare_ParamsObject_ShouldReturnError(object obj1, object obj2, object obj3)
    {
        // Arrange
        var builder = ComparisonBuilder
            .Create()
            .DisallowNullsInArguments();
        // Act
        var result = builder.Compare(obj1, obj2, obj3);
        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.ErrorCount.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.Object.BothObjectsAreNullCode);
    }
}
