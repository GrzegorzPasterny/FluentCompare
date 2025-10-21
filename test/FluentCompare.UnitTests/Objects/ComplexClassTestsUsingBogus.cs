using Xunit.Abstractions;

namespace FluentCompare.UnitTests.Objects;
public class ComplexClassTestsUsingBogus
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ComplexClassTestsUsingBogus(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void CompareComplexClassesFilledWithRandomValues_ShouldGenerateProperRaport()
    {
        // Arrange
        var obj1 = TestDataGenerator.CreateClassWithAllSupportedTypes();
        var obj2 = TestDataGenerator.CreateClassWithAllSupportedTypes();

        // Act
        var result = new ComparisonBuilder()
            .Compare(obj1, obj2);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.WasSuccessful.ShouldBeTrue();
    }
}
