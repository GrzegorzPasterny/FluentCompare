using FluentCompare.Tests.Shared.Models;
using FluentCompare.Tests.Utilities;

namespace FluentCompare.SolutionComparison.Tests;

public class TestsDataSource
{
    public static IEnumerable<object?[]> PerformObjectComparison_DataSource()
    {
        yield return new object[]
        {
            new int[] { 1, 2, 3 },
            new int[] { 1, 3, 3 }
        };
        yield return new object[]
        {
            new ClassWithIntProperty(1),
            new ClassWithIntProperty(2)
        };
        yield return new object?[]
        {
            TestDataGenerator.CreateClassWithAllSupportedTypes(depth: 1),
            TestDataGenerator.CreateClassWithAllSupportedTypes(depth: 1),
        };
        yield return new object?[]
        {
            TestDataGenerator.CreateClassWithAllSupportedTypes(depth: 2),
            TestDataGenerator.CreateClassWithAllSupportedTypes(depth: 2),
        };
    }
}
