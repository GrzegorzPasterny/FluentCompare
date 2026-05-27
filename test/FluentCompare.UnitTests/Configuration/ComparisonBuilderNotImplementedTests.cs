namespace FluentCompare.UnitTests.Configuration;

public class ComparisonBuilderNotImplementedTests
{
    private readonly ITestOutputHelper _output;

    public ComparisonBuilderNotImplementedTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private static ComparisonBuilder CreateBuilder()
    {
        return ComparisonBuilder.Create();
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonResult>, int, string?> NotImplementedCases =>
        new()
        {
            { b => b.Compare('a', 'b'), 1, ComparisonErrors.NotImplementedCode },
            { b => b.Compare(new[] { 'a', 'b' }), 1, ComparisonErrors.NotImplementedCode },
            { b => b.Compare(new[] { 'a' }, new[] { 'b' }), 1, ComparisonErrors.NotImplementedCode },
            { b => b.Compare((object)new[] { 'a' }, (object)new[] { 'b' }), 1, ComparisonErrors.NotImplementedCode },
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
    [MemberData(nameof(NotImplementedCases))]
    public void Compare_UnsupportedPrimitiveAndArrayTypes_ReturnNotImplementedError(
        Func<ComparisonBuilder, ComparisonResult> execute,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        var result = execute(builder);

        result.ErrorCount.ShouldBe(expectedErrors);
        result.MismatchCount.ShouldBe(0);

        if (expectedCode is not null)
        {
            AssertFirstErrorCode(result, expectedCode);
        }
    }
}
