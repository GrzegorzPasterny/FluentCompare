using FluentCompare.Tests.Shared.Models;

using Xunit.Abstractions;

namespace FluentCompare.UnitTests.Objects;

public class ClassComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ClassComparisonTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public static IEnumerable<object[]> CompareClassesWithIntProp_WithDefaultConfiguration_ReturnsExpectedResult_DataSource()
    {
        yield return new object[]
        {
            new ClassWithIntProperty(1),
            new ClassWithIntProperty(1),
            true,
            true,
            string.Empty
        };
        yield return new object[]
        {
            new ClassWithIntProperty(1),
            new ClassWithIntProperty(2),
            false,
            true,
            ComparisonMismatches<int>.MismatchDetectedCode
        };
        yield return new object[]
        {
            new ClassWithIntProperty(1),
            null!,
            false,
            true,
            ComparisonMismatches.NullPassedAsArgumentCode
        };
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
                ClassWithIntProperty = null!
            },
            false,
            false,
            ComparisonErrors.NullPassedAsArgumentCode
        };
        yield return new object[]
        {
            null!,
            new ClassWithNestedClassWithIntProperty()
            {
                ClassWithIntProperty = null!
            },
            false,
            true,
            ComparisonMismatches.NullPassedAsArgumentCode
        };
    }

    [Theory]
    [MemberData(nameof(CompareClassesWithIntProp_WithDefaultConfiguration_ReturnsExpectedResult_DataSource))]
    public void CompareClassesWithIntProp_WithDefaultConfiguration_ReturnsExpectedResult
        (object obj1, object obj2, bool mismatchExpectedResult, bool errorExpectedResult, string errorCode)
    {
        // Act
        var result = ComparisonBuilder.Create()
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
