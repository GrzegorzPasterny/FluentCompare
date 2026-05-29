namespace FluentCompare.UnitTests.Objects;

public class ObjectComparisonReflectionSafetyTests
{
    private readonly ITestOutputHelper _output;

    public ObjectComparisonReflectionSafetyTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private sealed class IndexerHolder
    {
        public int Value { get; set; }

        public string this[int index] => $"item-{index}";
    }

    private sealed class ThrowingPropertyHolder
    {
        public int Value { get; set; }

        public int Throwing => throw new InvalidOperationException("getter failure");
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

    [Fact]
    public void Compare_ObjectWithIndexer_DoesNotThrowAndSkipsIndexerProperty()
    {
        var left = new IndexerHolder { Value = 10 };
        var right = new IndexerHolder { Value = 10 };

        var result = ComparisonBuilder.Create().Compare(left, right);

        LogResult(result);
        result.ErrorCount.ShouldBe(0);
        result.MismatchCount.ShouldBe(0);
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_ObjectWithThrowingGetter_AddsReflectionErrorInsteadOfThrowing()
    {
        var left = new ThrowingPropertyHolder { Value = 10 };
        var right = new ThrowingPropertyHolder { Value = 10 };

        var result = ComparisonBuilder.Create().Compare(left, right);

        LogResult(result);
        result.ErrorCount.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.ReflectionPropertyAccessFailedCode);
    }
}
