using Xunit.Abstractions;

namespace FluentCompare.UnitTests.Nullability;

public class NullabilityTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public NullabilityTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData(null, null, typeof(int?), nameof(ComparisonErrors.BothObjectsAreNullCode))]
    [InlineData(null, "test", typeof(string), nameof(ComparisonErrors.NullPassedAsArgumentCode))]
    [InlineData("test", null, typeof(string), nameof(ComparisonErrors.NullPassedAsArgumentCode))]
    [InlineData(12.3, null, typeof(float?), nameof(ComparisonErrors.NullPassedAsArgumentCode))]
    [InlineData(null, 12.3, typeof(float?), nameof(ComparisonErrors.NullPassedAsArgumentCode))]
    public void ReturnComparisonError_WhenNullsArePassedWithPrimitiveTypes_WhenNullsNotAllowed(
        object? obj1,
        object? obj2,
        Type type,
        string expecterErrorCode)
    {
        // Arrange
        // Cast obj1 to the runtime type specified by 'type'
        var castedObj1 = obj1 is null ? null : Convert.ChangeType(obj1, Nullable.GetUnderlyingType(type) ?? type);
        var castedObj2 = obj2 is null ? null : Convert.ChangeType(obj2, Nullable.GetUnderlyingType(type) ?? type);

        // Act
        var result = ComparisonBuilder.Create()
            .DisallowNullComparison()
            .Compare(castedObj1, castedObj2);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.WasSuccessful.ShouldBeFalse();
        result.ErrorCount.ShouldBe(1);
        result.Errors[0].Code.ShouldContain(string.Concat(expecterErrorCode.SkipLast(4)));
    }

    [Theory]
    [InlineData(null, null, typeof(int?), nameof(ComparisonErrors.BothObjectsAreNullCode))]
    [InlineData(null, "test", typeof(string), nameof(ComparisonErrors.OneOfTheObjectsIsNullCode))]
    [InlineData("test", null, typeof(string), nameof(ComparisonErrors.OneOfTheObjectsIsNullCode))]
    [InlineData(12.3, null, typeof(float?), nameof(ComparisonErrors.OneOfTheObjectsIsNullCode))]
    [InlineData(null, 12.3, typeof(float?), nameof(ComparisonErrors.OneOfTheObjectsIsNullCode))]
    public void ReturnComparisonError_WhenNullsArePassedWithPrimitiveTypes_WhenNullsNotAllowed_PackObjectsToArray(
        object? obj1,
        object? obj2,
        Type type,
        string expecterErrorCode)
    {
        // Arrange
        // Cast obj1 to the runtime type specified by 'type'
        var castedObj1 = obj1 is null ? null : Convert.ChangeType(obj1, Nullable.GetUnderlyingType(type) ?? type);
        var castedObj2 = obj2 is null ? null : Convert.ChangeType(obj2, Nullable.GetUnderlyingType(type) ?? type);

        object? objectsArray1 = new object?[] { castedObj1 };
        object? objectsArray2 = new object?[] { castedObj2 };

        // Act
        var result = ComparisonBuilder.Create()
            .DisallowNullComparison()
            .Compare(objectsArray1, objectsArray2);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.WasSuccessful.ShouldBeFalse();
        result.ErrorCount.ShouldBe(1);
        result.Errors[0].Code.ShouldContain(string.Concat(expecterErrorCode.SkipLast(4)));
    }
}
