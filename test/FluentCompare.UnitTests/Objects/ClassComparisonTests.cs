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
            new ClassWithClassWithIntProperty()
            {
                ClassWithIntProperty = new ClassWithIntProperty(1)
            },
            new ClassWithClassWithIntProperty()
            {
                ClassWithIntProperty = new ClassWithIntProperty(1)
            },
            true,
            true,
            string.Empty
        };
        yield return new object[]
        {
            new ClassWithClassWithIntProperty()
            {
                ClassWithIntProperty = new ClassWithIntProperty(1)
            },
            new ClassWithClassWithIntProperty()
            {
                ClassWithIntProperty = new ClassWithIntProperty(2)
            },
            false,
            true,
            ComparisonMismatches<int>.MismatchDetectedCode
        };
        yield return new object[]
        {
            new ClassWithClassWithIntProperty()
            {
                ClassWithIntProperty = new ClassWithIntProperty(1)
            },
            new ClassWithClassWithIntProperty()
            {
                ClassWithIntProperty = null!
            },
            false,
            true,
            ComparisonMismatches.Object.MismatchDetectedByNullCode
        };
        yield return new object[]
        {
            null!,
            new ClassWithClassWithIntProperty()
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

    public static IEnumerable<object[]> CompareNestedClasses_WithLimitedDepth_ShouldReturnExpectedResult_DataSource()
    {
        yield return new object[]
        {
            new ClassWithAllSupportedTypes()
            {
                Double = 1.0,
            },
            new ClassWithAllSupportedTypes()
            {
                Double = 1.1,
            },
            0,
            true,
        };
        yield return new object[]
        {
            new ClassWithAllSupportedTypes()
            {
                Double = 1.0,
            },
            new ClassWithAllSupportedTypes()
            {
                Double = 1.1,
            },
            1,
            false,
        };
        yield return new object[]
        {
            new ClassWithAllSupportedTypes()
            {
                Double = 1.0,
                NestedClass = new ClassWithAllSupportedTypes()
                {
                    String = "string",
                }
            },
            new ClassWithAllSupportedTypes()
            {
                Double = 1.0,
                NestedClass = new ClassWithAllSupportedTypes()
                {
                    String = "other-string",
                }
            },
            1,
            true,
        };
        yield return new object[]
        {
            new ClassWithAllSupportedTypes()
            {
                Double = 1.0,
                NestedClass = new ClassWithAllSupportedTypes()
                {
                    String = "string",
                }
            },
            new ClassWithAllSupportedTypes()
            {
                Double = 1.0,
                NestedClass = new ClassWithAllSupportedTypes()
                {
                    String = "other-string",
                }
            },
            2,
            false,
        };
        yield return new object[]
        {
            new ClassWithAllSupportedTypes()
            {
                Double = 1.0,
                NestedClass = new ClassWithAllSupportedTypes()
                {
                    String = "string",
                    Bool = true,
                    NestedClass = new ClassWithAllSupportedTypes()
                    {
                        NestedClass = new ClassWithAllSupportedTypes()
                        {
                            Int = 1 // difference on 4th level
                        }
                    }
                }
            },
            new ClassWithAllSupportedTypes()
            {
                Double = 1.0,
                NestedClass = new ClassWithAllSupportedTypes()
                {
                    String = "string",
                    Bool = true,
                    NestedClass = new ClassWithAllSupportedTypes()
                    {
                        NestedClass = new ClassWithAllSupportedTypes()
                        {
                            Int = 2 // difference on 4th level
                        }
                    }
                }
            },
            3,
            true,
        };
        yield return new object[]
        {
            new ClassWithAllSupportedTypes()
            {
                Double = 1.0,
                NestedClass = new ClassWithAllSupportedTypes()
                {
                    String = "string",
                    Bool = true,
                    NestedClass = new ClassWithAllSupportedTypes()
                    {
                        NestedClass = new ClassWithAllSupportedTypes()
                        {
                            Int = 1 // difference on 4th level
                        }
                    }
                }
            },
            new ClassWithAllSupportedTypes()
            {
                Double = 1.0,
                NestedClass = new ClassWithAllSupportedTypes()
                {
                    String = "string",
                    Bool = true,
                    NestedClass = new ClassWithAllSupportedTypes()
                    {
                        NestedClass = new ClassWithAllSupportedTypes()
                        {
                            Int = 2 // difference on 4th level
                        }
                    }
                }
            },
            4,
            false,
        };
    }

    [Theory]
    [MemberData(nameof(CompareNestedClasses_WithLimitedDepth_ShouldReturnExpectedResult_DataSource))]
    public void CompareNestedClasses_WithLimitedDepth_ShouldReturnExpectedResult(
        ClassWithAllSupportedTypes nestedClass1,
        ClassWithAllSupportedTypes nestedClass2,
        int depth,
        bool shouldAllMatch)
    {
        // Act
        var result = ComparisonBuilder.Create()
            .SetComparisonDepth(depth)
            .Compare(nestedClass1, nestedClass2);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.WasSuccessful.ShouldBeTrue();
        result.AllMatched.ShouldBe(shouldAllMatch);
    }
}
