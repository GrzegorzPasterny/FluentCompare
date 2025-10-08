
namespace FluentCompare.UnitTests.Objects
{
    public class ObjectsComparisonTests
    {
        [Fact]
        public void Compare_TwoEquivalentAnonymousTypes_WithDefaultConfig_ReturnsAllMatchingResult()
        {
            // Arrange
            var obj1 = new { Name = "Test", Value = 123 };
            var obj2 = new { Name = "Test", Value = 123 };

            // Act
            var result = new ComparisonBuilder()
                .Compare(obj1, obj2);

            // Assert
            result.AllMatched.ShouldBeTrue(); // default behavior checks for equivalency
        }

        [Fact]
        public void Compare_TwoEquivalentAnonymousTypes_WithReferenceEqualityConfig_ReturnsNotMatchingResult()
        {
            // Arrange
            var obj1 = new { Name = "Test", Value = 123 };
            var obj2 = new { Name = "Test", Value = 123 };

            // Act
            var result = new ComparisonBuilder()
                .UseReferenceEquality()
                .Compare(obj1, obj2);

            // Assert
            result.AllMatched.ShouldBeFalse();
        }

        [Fact]
        public void Compare_TwoNonEquivalentAnonymousTypes_WithDefaultConfiguration_ReturnsNotMatchingResult()
        {
            // Arrange
            var obj1 = new { Name = "Test", Value = 123 };
            var obj2 = new { Name = "Test", Value = 456 };

            // Act
            var result = new ComparisonBuilder()
                .Compare(obj1, obj2);

            // Assert
            result.AllMatched.ShouldBeFalse();
        }

        [Fact]
        public void Compare_TwoEquivalentAnonymousTypeArrays_WithDefaultConfiguration_ReturnsAllMatchingResult()
        {
            // Arrange
            var obj1 = new { Name = "Test", Value = 123 };
            var obj2 = new { Name = "Test", Value = 456 };
            var objArr1 = new[] { obj1, obj2 };

            var obj3 = new { Name = "Test", Value = 123 };
            var obj4 = new { Name = "Test", Value = 456 };
            var objArr2 = new[] { obj3, obj4 };

            // Act
            var result = new ComparisonBuilder()
                .Compare(obj1, obj2);

            // Assert
            result.AllMatched.ShouldBeFalse();
        }

        [Fact]
        public void Compare_TwoSameAnonymousTypeArrays_WithDefaultConfiguration_ReturnsAllMatchingResult()
        {
            // Arrange
            var obj1 = new { Name = "Test", Value = 123 };
            var obj2 = new { Name = "Test", Value = 456 };
            var objArr1 = new[] { obj1, obj2 };
            var objArr2 = new[] { obj1, obj2 };

            // Act
            var result = new ComparisonBuilder()
                .Compare(obj1, obj2);

            // Assert
            result.AllMatched.ShouldBeTrue();
        }

        [Fact]
        public void Compare_TwoSameAnonymousTypeArrays_WithReferenceComparison_ReturnsAllMatchingResult()
        {
            // Arrange
            var obj1 = new { Name = "Test", Value = 123 };
            var obj2 = new { Name = "Test", Value = 456 };
            var objArr1 = new[] { obj1, obj2 };
            var objArr2 = new[] { obj1, obj2 };

            // Act
            var result = new ComparisonBuilder()
                .UseReferenceEquality()
                .Compare(obj1, obj2);

            // Assert
            result.AllMatched.ShouldBeTrue();
        }
    }
}
