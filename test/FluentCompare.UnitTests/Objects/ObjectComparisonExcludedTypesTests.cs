using FluentCompare.Tests.Shared.Models;

namespace FluentCompare.UnitTests.Objects;

public class ObjectComparisonExcludedTypesTests
{
    private readonly ITestOutputHelper _output;

    public ObjectComparisonExcludedTypesTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private void LogResult(ComparisonResult result)
    {
        _output.WriteLine(result.ToString());
        foreach (var mismatch in result.Mismatches)
        {
            _output.WriteLine($"Mismatch [{mismatch.Code}]: {mismatch.Message}");
        }
        foreach (var error in result.Errors)
        {
            _output.WriteLine($"Error [{error.Code}]: {error.Message}");
        }
        foreach (var warning in result.Warnings)
        {
            _output.WriteLine($"Warning [{warning.Code}]: {warning.Message}");
        }
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, object, object, int, int, string?> ExcludedTypeCases =>
        new()
        {
            {
                b => b.Configure(cfg => cfg.TypesExcludedFromComparison.Add(typeof(string))),
                new ClassWithAllSupportedTypes { String = "one", Int = 7 },
                new ClassWithAllSupportedTypes { String = "two", Int = 7 },
                0,
                0,
                null
            },
            {
                b => b.Configure(cfg => cfg.TypesExcludedFromComparison.Add(typeof(string))),
                new ClassWithAllSupportedTypes { String = "one", Int = 7 },
                new ClassWithAllSupportedTypes { String = "two", Int = 8 },
                1,
                0,
                ComparisonMismatches<int>.MismatchDetectedCode
            },
            {
                b => b.Configure(cfg => cfg.TypesExcludedFromComparison.Add(typeof(ClassWithIntProperty))),
                new ClassWithClassWithIntProperty { ClassWithIntProperty = new ClassWithIntProperty(1) },
                new ClassWithClassWithIntProperty { ClassWithIntProperty = new ClassWithIntProperty(2) },
                0,
                0,
                null
            },
        };

    [Theory]
    [MemberData(nameof(ExcludedTypeCases))]
    public void Compare_ObjectPair_ExcludedTypes_UsesExpectedOutcome(
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
            result.Mismatches[0].Code.ShouldBe(expectedCode);
        }
    }
}
