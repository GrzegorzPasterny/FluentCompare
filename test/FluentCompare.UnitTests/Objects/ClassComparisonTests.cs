using FluentCompare.Tests.Shared.Models;



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

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, object, object, int, int, string?> ObjectComparisonModeCases =>
        new()
        {
            {
                b => b.UsePropertyEquality(),
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(1),
                0,
                0,
                null
            },
            {
                b => b.UseReferenceEquality(),
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(1),
                1,
                0,
                ComparisonMismatches.Object.MismatchDetectedByReferenceCode
            },
            {
                b => b.UseComplexTypeComparisonMode(ComplexTypesComparisonMode.ReferenceEquality),
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(1),
                1,
                0,
                ComparisonMismatches.Object.MismatchDetectedByReferenceCode
            },
        };

    public static TheoryData<int, int, int, int, string?> ObjectComparisonDepthCases =>
        new()
        {
            { 5, 1, 0, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            { 1, 0, 0, 1, ComparisonErrors.DepthLimitReachedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, object, object, int, int, string?> ObjectNestedPrimitiveConfigurationCases =>
        new()
        {
            {
                b => b.UseStringComparisonType(StringComparison.OrdinalIgnoreCase),
                new ClassWithAllSupportedTypes { String = "CaseSensitive" },
                new ClassWithAllSupportedTypes { String = "casesensitive" },
                0,
                0,
                null
            },
            {
                b => b.WithDoublePrecision(2),
                new ClassWithAllSupportedTypes { Double = 1.234 },
                new ClassWithAllSupportedTypes { Double = 1.2344 },
                0,
                0,
                null
            },
            {
                b => b.UseComparisonType(ComparisonType.NotEqualTo),
                new ClassWithIntProperty(1),
                new ClassWithIntProperty(2),
                0,
                0,
                null
            },
        };

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

    [Theory]
    [MemberData(nameof(ObjectComparisonModeCases))]
    public void Compare_ObjectPair_ConfigurationMode_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        object left,
        object right,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(ComparisonBuilder.Create());
        var result = builder.Compare(left, right);

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
    [MemberData(nameof(ObjectComparisonDepthCases))]
    public void Compare_ObjectPair_ComparisonDepth_UsesExpectedOutcome(
        int depth,
        int expectedMismatches,
        int expectedErrors,
        int expectedWarnings,
        string? expectedCode)
    {
        var left = new ClassWithAllSupportedTypes
        {
            NestedClass = new ClassWithAllSupportedTypes
            {
                Int = 1
            }
        };

        var right = new ClassWithAllSupportedTypes
        {
            NestedClass = new ClassWithAllSupportedTypes
            {
                Int = 2
            }
        };

        var result = ComparisonBuilder.Create()
            .SetComparisonDepth(depth)
            .Compare(left, right);

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

    [Theory]
    [MemberData(nameof(ObjectNestedPrimitiveConfigurationCases))]
    public void Compare_ObjectPair_NestedPrimitiveSettings_UseActiveConfiguration(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        object left,
        object right,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(ComparisonBuilder.Create());
        var result = builder.Compare(left, right);

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
}
