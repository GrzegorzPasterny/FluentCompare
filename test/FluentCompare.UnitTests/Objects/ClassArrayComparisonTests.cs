using FluentCompare.Tests.Shared.Models;



namespace FluentCompare.UnitTests.Objects;

public class ClassArrayComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ClassArrayComparisonTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private void LogResult(ComparisonResult result)
    {
        _testOutputHelper.WriteLine(result.ToString());
        foreach (var mismatch in result.Mismatches)
        {
            _testOutputHelper.WriteLine($"Mismatch [{mismatch.Code}]: {mismatch.Message}");
        }
        foreach (var error in result.Errors)
        {
            _testOutputHelper.WriteLine($"Error [{error.Code}]: {error.Message}");
        }
        foreach (var warning in result.Warnings)
        {
            _testOutputHelper.WriteLine($"Warning [{warning.Code}]: {warning.Message}");
        }
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, object[]?, object[]?, int, int, string?> ObjectArrayComparisonModeCases =>
        new()
        {
            {
                b => b.UsePropertyEquality(),
                [new ClassWithIntProperty(1)],
                [new ClassWithIntProperty(1)],
                0,
                0,
                null
            },
            {
                b => b.UseReferenceEquality(),
                [new ClassWithIntProperty(1)],
                [new ClassWithIntProperty(1)],
                1,
                0,
                ComparisonMismatches.Object.MismatchDetectedByReferenceCode
            },
            {
                b => b.UseComplexTypeComparisonMode(ComplexTypesComparisonMode.ReferenceEquality),
                [new ClassWithIntProperty(1)],
                [new ClassWithIntProperty(1)],
                1,
                0,
                ComparisonMismatches.Object.MismatchDetectedByReferenceCode
            },
        };

    public static TheoryData<int, int, int, int, string?> ObjectArrayComparisonDepthCases =>
        new()
        {
            { 5, 1, 0, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { 1, 0, 0, 1, ComparisonErrors.DepthLimitReachedCode },
        };

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
            },
            false,
            false,
            ComparisonErrors.InputArrayLengthsDifferCode
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
                null!
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
            null!,
            false,
            true,
            ComparisonMismatches.NullPassedAsArgumentCode
        };
        yield return new object[]
        {
            new ClassWithIntProperty[]
            {
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(2)
            },
            new ClassWithClassWithIntProperty[]
            {
                new ClassWithClassWithIntProperty()
                {
                    ClassWithIntProperty = new ClassWithIntProperty(1)
                },
                new ClassWithClassWithIntProperty()
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

    [Theory]
    [MemberData(nameof(ObjectArrayComparisonModeCases))]
    public void Compare_ObjectArrayPair_ConfigurationMode_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        object[]? first,
        object[]? second,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(ComparisonBuilder.Create());
        var result = builder.Compare(first, second);

        LogResult(result);
        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);

        if (expectedCode is not null)
        {
            if (expectedErrors > 0)
            {
                result.Errors[0].Code.ShouldBe(expectedCode);
            }
            else
            {
                result.Mismatches[0].Code.ShouldBe(expectedCode);
            }
        }
    }

    [Theory]
    [MemberData(nameof(ObjectArrayComparisonDepthCases))]
    public void Compare_ObjectArrayPair_ComparisonDepth_UsesExpectedOutcome(
        int depth,
        int expectedMismatches,
        int expectedErrors,
        int expectedWarnings,
        string? expectedCode)
    {
        var first = new object[]
        {
            new ClassWithAllSupportedTypes
            {
                NestedClass = new ClassWithAllSupportedTypes
                {
                    Int = 1
                }
            }
        };

        var second = new object[]
        {
            new ClassWithAllSupportedTypes
            {
                NestedClass = new ClassWithAllSupportedTypes
                {
                    Int = 2
                }
            }
        };

        var result = ComparisonBuilder.Create()
            .SetComparisonDepth(depth)
            .Compare(first, second);

        LogResult(result);
        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);
        if (expectedWarnings > 0)
        {
            result.WarningCount.ShouldBeGreaterThanOrEqualTo(expectedWarnings);
        }
        else
        {
            result.WarningCount.ShouldBe(0);
        }

        if (expectedCode is not null)
        {
            if (expectedWarnings > 0)
            {
                result.Warnings[0].Code.ShouldBe(expectedCode);
            }
            else if (expectedErrors > 0)
            {
                result.Errors[0].Code.ShouldBe(expectedCode);
            }
            else
            {
                result.Mismatches[0].Code.ShouldBe(expectedCode);
            }
        }
    }
}
