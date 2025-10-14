using FluentCompare.UnitTests.Objects.Models;

using Xunit.Abstractions;

namespace FluentCompare.UnitTests.Objects;
public class ClassComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ClassComparisonTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public static IEnumerable<object[]> CompareClasses_WithDefaultConfiguration_ReturnsExpectedResult_DataSource()
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
    [MemberData(nameof(CompareClasses_WithDefaultConfiguration_ReturnsExpectedResult_DataSource))]
    public void CompareClassesWithIntProperty_WithDefaultConfiguration_ReturnsExpectedResult
        (object obj1, object obj2, bool expectedResult, string errorCode)
    {
        // Act
        var result = new ComparisonBuilder()
            .Compare(obj1, obj2);

        // Assert
        result.AllMatched.ShouldBe(expectedResult);
        if (expectedResult == false)
            result.Mismatches.ShouldContain(m => m.Code == errorCode);

        _testOutputHelper.WriteLine(result.ToString());
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
        _testOutputHelper.WriteLine(result.ToString());
    }

    public static IEnumerable<object[]> CompareNestedClassesWithIntProp_WithDefaultConfiguration_ReturnsExpectedResult_DataSource()
    {
        yield return new object[]
        {
            new ClassWithNestedClassWithIntProperty()
            {
                ClassWithIntProperty = new ClassWithIntProperty(1)
            },
            new ClassWithNestedClassWithIntProperty()
            {
                ClassWithIntProperty = new ClassWithIntProperty(1)
            },
            true,
            true,
            string.Empty
        };
        yield return new object[]
        {
            new ClassWithNestedClassWithIntProperty()
            {
                ClassWithIntProperty = new ClassWithIntProperty(1)
            },
            new ClassWithNestedClassWithIntProperty()
            {
                ClassWithIntProperty = new ClassWithIntProperty(2)
            },
            false,
            true,
            ComparisonMismatches<int>.MismatchDetectedCode
        };
        yield return new object[]
        {
            new ClassWithNestedClassWithIntProperty()
            {
                ClassWithIntProperty = new ClassWithIntProperty(1)
            },
            new ClassWithNestedClassWithIntProperty()
            {
                ClassWithIntProperty = null
            },
            false,
            false,
            ComparisonErrors.NullPassedAsArgumentCode
        };
        yield return new object[]
        {
            null,
            new ClassWithNestedClassWithIntProperty()
            {
                ClassWithIntProperty = null
            },
            false,
            false,
            ComparisonErrors.NullPassedAsArgumentCode
        };
    }

    [Theory]
    [MemberData(nameof(CompareNestedClassesWithIntProp_WithDefaultConfiguration_ReturnsExpectedResult_DataSource))]
    public void CompareNestedClassesWithIntProp_WithDefaultConfiguration_ReturnsExpectedResult
        (object obj1, object obj2, bool mismatchExpectedResult, bool errorExpectedResult, string errorCode)
    {
        // Act
        var result = new ComparisonBuilder()
            .Compare(obj1, obj2);

        // Assert
        result.WasSuccessful.ShouldBe(errorExpectedResult);
        if (errorExpectedResult is false)
        {
            result.Errors.ShouldContain(e => e.Code == errorCode);
        }
        else
        {
            result.AllMatched.ShouldBe(mismatchExpectedResult);
            if (mismatchExpectedResult == false)
                result.Mismatches.ShouldContain(m => m.Code == errorCode);
        }

        _testOutputHelper.WriteLine(result.ToString());
    }
}
