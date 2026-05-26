namespace FluentCompare.UnitTests.Configuration;

public class ComparisonBuilderConfigurationApiTests
{
    private readonly ITestOutputHelper _output;

    public ComparisonBuilderConfigurationApiTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private static ComparisonBuilder CreateBuilder()
    {
        return ComparisonBuilder.Create();
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, string, string, int, int, string?> UseConfigurationCases =>
        new()
        {
            {
                b => b.UseConfiguration(new ComparisonConfiguration
                {
                    ComparisonType = ComparisonType.EqualTo,
                    StringConfiguration = new StringComparisonConfiguration
                    {
                        StringComparisonType = StringComparison.OrdinalIgnoreCase
                    }
                }),
                "Hello",
                "hello",
                0,
                0,
                null
            },
            {
                b => b.UseConfiguration(new ComparisonConfiguration
                {
                    ComparisonType = ComparisonType.EqualTo,
                    StringConfiguration = new StringComparisonConfiguration
                    {
                        StringComparisonType = StringComparison.Ordinal
                    }
                }),
                "Hello",
                "hello",
                1,
                0,
                ComparisonMismatches<string>.MismatchDetectedCode
            },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, string, string, int, int, string?> ConfigureCases =>
        new()
        {
            {
                b => b.Configure(cfg =>
                {
                    cfg.ComparisonType = ComparisonType.EqualTo;
                    cfg.StringConfiguration.StringComparisonType = StringComparison.OrdinalIgnoreCase;
                }),
                "Hello",
                "hello",
                0,
                0,
                null
            },
            {
                b => b.Configure(cfg =>
                {
                    cfg.ComparisonType = ComparisonType.EqualTo;
                    cfg.StringConfiguration.StringComparisonType = StringComparison.Ordinal;
                }),
                "Hello",
                "hello",
                1,
                0,
                ComparisonMismatches<string>.MismatchDetectedCode
            },
        };


    private void AssertFirstMismatchCode(ComparisonResult result, string expectedCode)
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

        result.MismatchCount.ShouldBeGreaterThan(0, result.ToString());
        result.Mismatches[0].Code.ShouldBe(expectedCode, result.Mismatches[0].Message);
    }

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
    [MemberData(nameof(UseConfigurationCases))]
    public void UseConfiguration_InfluencesComparisonOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        string left,
        string right,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = builder.Compare(left, right);

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);

        if (expectedCode is not null)
        {
            if (expectedErrors > 0)
            {
                AssertFirstErrorCode(result, expectedCode);
            }
            else
            {
                AssertFirstMismatchCode(result, expectedCode);
            }
        }
    }

}
