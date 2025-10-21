using AnyDiff;

using Xunit.Abstractions;

namespace FluentCompare.SolutionComparison.Tests.AnyDiffTests;
public class AnyDiffTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AnyDiffTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [MemberData(nameof(TestsDataSource.PerformObjectComparison_DataSource),
        MemberType = typeof(TestsDataSource))]
    public void PerformObjectComparison_UsingAnyDiff_CompareWithFluentCompare(object obj1, object obj2)
    {
        ICollection<Difference> differences = AnyDiff.AnyDiff.Diff(obj1, obj2);
        var comparisonResult = ComparisonBuilder.Create()
            .Compare(obj1, obj2);

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

        _testOutputHelper.WriteLine(string.Empty);
        _testOutputHelper.WriteLine("===========================");
        _testOutputHelper.WriteLine(string.Empty);

        _testOutputHelper.WriteLine("Comparison result using FluentCompare:");
        _testOutputHelper.WriteLine(comparisonResult.ToString());
    }
}
