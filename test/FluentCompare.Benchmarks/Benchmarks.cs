using System.Text;

using BenchmarkDotNet.Attributes;

using FluentCompare.Tests.Shared.Models;
using FluentCompare.UnitTests;

using KellermanSoftware.CompareNetObjects;

namespace FluentCompare.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 3, iterationCount: 8)]
public class Benchmarks
{
    private ClassWithAllSupportedTypes? _obj1;
    private ClassWithAllSupportedTypes? _obj2;

    [GlobalSetup]
    public void Setup()
    {
        _obj1 = TestDataGenerator.CreateClassWithAllSupportedTypes(depth: 2);
        _obj2 = TestDataGenerator.CreateClassWithAllSupportedTypes(depth: 2);
    }

    [Benchmark]
    public string CompareWith_FluentComparison()
    {
        var comparisonResult = ComparisonBuilder.Create()
            .Compare(_obj1, _obj2);
        return comparisonResult.ToString();
    }

    [Benchmark]
    public string CompareWith_CompareNetObjects()
    {
        var compareLogic = new CompareLogic(new ComparisonConfig()
        {
            MaxDifferences = int.MaxValue
        });
        KellermanSoftware.CompareNetObjects.ComparisonResult comparisonResultNetObjects =
            compareLogic.Compare(_obj1, _obj2);

        return comparisonResultNetObjects.DifferencesString;
    }

    [Benchmark]
    public string CompareWith_AnyDiff()
    {
        ICollection<AnyDiff.Difference> differences = AnyDiff.AnyDiff.Diff(_obj1, _obj2);

        var result = new StringBuilder();

        foreach (AnyDiff.Difference difference in differences)
        {
            result.AppendLine(difference.ToString());
            result.AppendLine($"[" +
                $"{nameof(AnyDiff.Difference.Property)} = {difference.Property}, " +
                $"{nameof(AnyDiff.Difference.PropertyType)} = {difference.PropertyType.Name}, " +
                $"{nameof(AnyDiff.Difference.Path)} = {difference.Path}, " +
                $"{nameof(AnyDiff.Difference.Delta)} = {difference.Delta}, " +
                $"{nameof(AnyDiff.Difference.ArrayIndex)} = {difference.ArrayIndex}]");
        }

        return result.ToString();
    }
}
