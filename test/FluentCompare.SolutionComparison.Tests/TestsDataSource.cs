using FluentCompare.Tests.Shared.Models;

namespace FluentCompare.SolutionComparison.Tests;
public class TestsDataSource
{
    public static IEnumerable<object[]> PerformObjectComparison_DataSource()
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
    }
}
