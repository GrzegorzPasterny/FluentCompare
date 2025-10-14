using FluentCompare.UnitTests.Objects.Models;

namespace FluentCompare.UnitTests.Objects;
public class ClassComparisonTests
{
    public static IEnumerable<object[]> ClassesToCompare_DataSource()
    {
        yield return new object[]
        {
            new ClassWithIntProperty(1),
            new ClassWithIntProperty(1),
            true,
            string.Empty
        };
        yield return new object[]
        {
            new ClassWithIntProperty(1),
            new ClassWithIntProperty(2),
            false,
            ComparisonMismatches<int>.MismatchDetectedCode
        };
    }

    [Theory]
    [MemberData(nameof(ClassesToCompare_DataSource))]
    public void CompareClasses_WithDefaultConfiguration_ReturnsExpectedResult(object obj1, object obj2, bool expectedResult, string errorCode)
    {
        // Arrange
        var equals = obj1.Equals(obj2);

        // Act
        var result = new ComparisonBuilder()
            .Compare(obj1, obj2);

        // Assert
        result.AllMatched.ShouldBe(expectedResult);
        if (expectedResult == false)
            result.Mismatches.ShouldContain(m => m.Code == errorCode);
    }

    [Fact]
    public void CompareClasses_WhenOneOfTheClassIsNull_ReturnsExpectedResult()
    {
        // Arrange
        ClassWithIntProperty obj1 = new(1);
        ClassWithIntProperty obj2 = null;
        // Act
        var result = new ComparisonBuilder()
            .Compare(obj1, obj2);
        // Assert
        result.WasSuccessful.ShouldBeFalse();
        result.AllMatched.ShouldBeFalse();
        result.Errors.ShouldContain(m => m.Code == ComparisonErrors.NullPassedAsArgumentCode);
    }
}
