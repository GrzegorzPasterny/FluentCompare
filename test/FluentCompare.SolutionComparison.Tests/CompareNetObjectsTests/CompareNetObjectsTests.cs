using KellermanSoftware.CompareNetObjects;

using Xunit.Abstractions;

namespace FluentCompare.SolutionComparison.Tests.CompareNetObjectsTests;

public class CompareNetObjectsTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CompareNetObjectsTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [MemberData(nameof(TestsDataSource.PerformObjectComparison_DataSource),
        MemberType = typeof(TestsDataSource))]
    public void PerformObjectComparison_UsingAnyDiff_CompareWithFluentCompare(object obj1, object obj2)
    {
        var compareLogic = new CompareLogic(new ComparisonConfig()
        {
            MaxDifferences = int.MaxValue
        });
        KellermanSoftware.CompareNetObjects.ComparisonResult comparisonResultNetObjects =
            compareLogic.Compare(obj1, obj2);

        var comparisonResult = ComparisonBuilder.Create()
            .Compare(obj1, obj2);

        PrintResults(comparisonResultNetObjects, comparisonResult);
    }

    private void PrintResults(
        KellermanSoftware.CompareNetObjects.ComparisonResult comparisonResultNetObjects,
        ComparisonResult comparisonResult)
    {
        _testOutputHelper.WriteLine("Differences found using CompareNetObjects:");
        _testOutputHelper.WriteLine(comparisonResultNetObjects.DifferencesString);

        _testOutputHelper.WriteLine(string.Empty);
        _testOutputHelper.WriteLine("===========================");
        _testOutputHelper.WriteLine(string.Empty);

        _testOutputHelper.WriteLine("Comparison result using FluentCompare:");
        _testOutputHelper.WriteLine(comparisonResult.ToString());
    }
}
