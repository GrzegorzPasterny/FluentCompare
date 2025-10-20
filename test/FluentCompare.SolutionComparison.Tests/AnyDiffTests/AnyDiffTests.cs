using AnyDiff;

using FluentCompare.Tests.Shared.Models;

using Xunit.Abstractions;

namespace FluentCompare.SolutionComparison.Tests.AnyDiffTests;
public class AnyDiffTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AnyDiffTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void PerformIntArrayComparison_UsingAnyDiff_CompareWithFluentCompare_AnyDiffFailsToFindMismatch()
    {
        var object1 = new int[] { 1, 2, 3 };
        var object2 = new int[] { 1, 3, 3 };

        ICollection<Difference> differences = AnyDiff.AnyDiff.Diff(object1, object2);
        var comparisonResult = new ComparisonBuilder()
            .Compare(object1, object2);

        PrintResults(differences, comparisonResult);
    }

    [Fact]
    public void PerformClassWithIntComparison_UsingAnyDiff_CompareWithFluentCompare()
    {
        var object1 = new ClassWithIntProperty(1);
        var object2 = new ClassWithIntProperty(2);

        ICollection<Difference> differences = AnyDiff.AnyDiff.Diff(object1, object2);
        var comparisonResult = new ComparisonBuilder()
            .Compare(object1, object2);

        PrintResults(differences, comparisonResult);
    }

    private void PrintResults(ICollection<Difference> differences, ComparisonResult comparisonResult)
    {
        _testOutputHelper.WriteLine("Differences found using AnyDiff:");
        foreach (Difference difference in differences)
        {
            _testOutputHelper.WriteLine(difference.ToString());
            _testOutputHelper.WriteLine($"[" +
                $"{nameof(Difference.Property)} = {difference.Property}, " +
                $"{nameof(Difference.PropertyType)} = {difference.PropertyType.Name}, " +
                $"{nameof(Difference.Path)} = {difference.Path}, " +
                $"{nameof(Difference.Delta)} = {difference.Delta}, " +
                $"{nameof(Difference.ArrayIndex)} = {difference.ArrayIndex}]");
        }

        _testOutputHelper.WriteLine("Comparison result using FluentCompare:");
        _testOutputHelper.WriteLine(comparisonResult.ToString());
    }
}
