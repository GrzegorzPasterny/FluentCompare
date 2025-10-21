using FluentCompare.Tests.Shared.Models;

using Xunit.Abstractions;

namespace FluentCompare.UnitTests.Objects;
public class ClassArrayComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ClassArrayComparisonTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public static IEnumerable<object[]> CompareClassArray_WithDefaultConfiguration_ReturnsExpectedResult_DataSource()
    {
        yield return new object[]
        {
            new ClassWithIntProperty[]
            {
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(2)
            },
            new ClassWithIntProperty[]
            {
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(2)
            },
            true,
            true,
            string.Empty
        };
        yield return new object[]
        {
            new ClassWithIntProperty[]
            {
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(2)
            },
            new ClassWithIntProperty[]
            {
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(3)
            },
            false,
            true,
            ComparisonMismatches<int>.MismatchDetectedCode
        };
        yield return new object[]
        {
            new ClassWithIntProperty[]
            {
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(2)
            },
            new ClassWithIntProperty[]
            {
                new ClassWithIntProperty(1),
                null
            },
            false,
            true,
            ComparisonMismatches.Object.MismatchDetectedByNullCode
        };
        yield return new object[]
        {
            new ClassWithIntProperty[]
            {
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(2)
            },
            null,
            false,
            false,
            ComparisonErrors.NullPassedAsArgumentCode
        };
        yield return new object[]
        {
            new ClassWithIntProperty[]
            {
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(2)
            },
            new ClassWithNestedClassWithIntProperty[]
            {
                new ClassWithNestedClassWithIntProperty()
                {
                    ClassWithIntProperty = new ClassWithIntProperty(1)
                },
                new ClassWithNestedClassWithIntProperty()
                {
                    ClassWithIntProperty = new ClassWithIntProperty(2)
                }
            },
            false,
            true,
            ComparisonMismatches.Object.MismatchDetectedByTypeCode
        };
    }

    [Theory]
    [MemberData(nameof(CompareClassArray_WithDefaultConfiguration_ReturnsExpectedResult_DataSource))]
    public void CompareClassArray_WithDefaultConfiguration_ReturnsExpectedResult
        (object[] obj1, object[] obj2, bool mismatchExpectedResult, bool errorExpectedResult, string errorCode)
    {
        // Act
        var result = ComparisonBuilder.Create()
            .Compare(obj1, obj2);

        _testOutputHelper.WriteLine(result.ToString());

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
    }
}
