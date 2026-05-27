namespace FluentCompare.UnitTests.Configuration;

public class ComparisonBuilderObjectOverloadMixedTypeTests
{
    private readonly ITestOutputHelper _output;

    public ComparisonBuilderObjectOverloadMixedTypeTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public static TheoryData<object, object, int, int, string?> MixedObjectTypeCases =>
        new()
        {
            { 1, 1L, 1, 0, ComparisonMismatches.Object.MismatchDetectedByTypeCode },
            { new[] { 1, 2 }, new[] { 1L, 2L }, 1, 0, ComparisonMismatches.Object.MismatchDetectedByTypeCode },
            { 1.0f, 1.0d, 1, 0, ComparisonMismatches.Object.MismatchDetectedByTypeCode },
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

    [Theory]
    [MemberData(nameof(MixedObjectTypeCases))]
    public void Compare_ObjectOverload_MixedRuntimeTypes_ReturnTypeMismatchInsteadOfThrow(
        object left,
        object right,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var result = ComparisonBuilder.Create().Compare(left, right);

        result.MismatchCount.ShouldBe(expectedMismatches);
        result.ErrorCount.ShouldBe(expectedErrors);

        if (expectedCode is not null)
        {
            AssertFirstMismatchCode(result, expectedCode);
        }
    }
}
