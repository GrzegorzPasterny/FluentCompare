using FluentCompare.Tests.Shared.Models;

namespace FluentCompare.UnitTests.Configuration;

public class ComparisonBuilderComparisonConfigurationOverloadTests
{
    private readonly ITestOutputHelper _output;

    public ComparisonBuilderComparisonConfigurationOverloadTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private static ComparisonBuilder CreateBuilder()
    {
        return ComparisonBuilder.Create();
    }

    public static TheoryData<Func<ComparisonResult>, int, int, string?> StaticCompareOverloadCases =>
        new()
        {
            {
                () => ComparisonBuilder.Compare(
                    new ClassWithAllSupportedTypes { Int = 1 },
                    (ClassWithAllSupportedTypes?)null,
                    new ComparisonConfiguration { AllowNullsInArguments = false }),
                0,
                1,
                ComparisonErrors.NullPassedAsArgumentCode
            },
            {
                () => ComparisonBuilder.Compare(
                    new object[] { new ClassWithIntProperty(1) },
                    new object[] { new ClassWithIntProperty(1) },
                    new ComparisonConfiguration { AllowArrayComparisonOfDifferentLengths = true }),
                0,
                0,
                null
            },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonResult>, int, int, string?> ConfigureOverloadCases =>
        new()
        {
            {
                b => b.Configure(cfg =>
                    {
                        cfg.AllowNullsInArguments = false;
                    })
                    .Compare(new ClassWithAllSupportedTypes { Int = 1 }, (ClassWithAllSupportedTypes?)null),
                0,
                1,
                ComparisonErrors.NullPassedAsArgumentCode
            },
            {
                b => b.Configure(cfg =>
                    {
                        cfg.AllowArrayComparisonOfDifferentLengths = true;
                    })
                    .Compare(new object[] { new ClassWithIntProperty(1) }, new object[] { new ClassWithIntProperty(1) }),
                0,
                0,
                null
            },
        };

    private void AssertFirstErrorCode(ComparisonResult result, string expectedCode)
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

        result.ErrorCount.ShouldBeGreaterThan(0, result.ToString());
        result.Errors[0].Code.ShouldBe(expectedCode, result.Errors[0].Message);
    }

    [Theory]
    [MemberData(nameof(StaticCompareOverloadCases))]
    public void Compare_StaticOverload_UsesExpectedOutcome(
        Func<ComparisonResult> execute,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var result = execute();

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);

        if (expectedCode is not null)
        {
            AssertFirstErrorCode(result, expectedCode);
        }
    }

    [Theory]
    [MemberData(nameof(ConfigureOverloadCases))]
    public void Compare_ConfigureOverload_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonResult> execute,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var result = execute(CreateBuilder());

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);

        if (expectedCode is not null)
        {
            AssertFirstErrorCode(result, expectedCode);
        }
    }
}
