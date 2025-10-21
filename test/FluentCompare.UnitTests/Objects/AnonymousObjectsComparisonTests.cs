
using Xunit.Abstractions;

namespace FluentCompare.UnitTests.Objects
{
    public class AnonymousObjectsComparisonTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public AnonymousObjectsComparisonTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Compare_TwoEquivalentAnonymousTypes_WithDefaultConfig_ReturnsAllMatchingResult()
        {
            // Arrange
            var obj1 = new { Name = "Test", Value = 123 };
            var obj2 = new { Name = "Test", Value = 123 };

            // Act
            var result = ComparisonBuilder.Create()
                .Compare(obj1, obj2);

            // Assert
            result.AllMatched.ShouldBeTrue(); // default behavior checks for equivalency
        }

        [Fact]
        public void Compare_TwoEquivalentAnonymousTypes_WithReferenceEqualityConfig_ReturnsNotMatchingResult()
        {
            // Act
            var result = ComparisonBuilder.Create()
                .UseReferenceEquality()
                .Compare(new { Name = "Test", Value = 123 }, new { Name = "Test", Value = 123 });

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
            var result = ComparisonBuilder.Create()
                .Compare(obj1, obj2);

            // Assert
            result.AllMatched.ShouldBeFalse();
            result.Mismatches.Count.ShouldBe(1);
            _testOutputHelper.WriteLine(result.ToString());
            result.Mismatches.First().Message.ShouldContain(nameof(obj1));
            result.Mismatches.First().Message.ShouldContain(nameof(obj2));
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
            var result = ComparisonBuilder.Create()
                .Compare(objArr1, objArr2);

            // Assert
            result.AllMatched.ShouldBeFalse();
            result.Mismatches.Count.ShouldBe(1);
            _testOutputHelper.WriteLine(result.ToString());
            result.Mismatches.First().Code.ShouldBe(ComparisonMismatches<object>.MismatchDetectedCode);
            result.Mismatches.First().Message.ShouldContain(nameof(objArr1));
            result.Mismatches.First().Message.ShouldContain(nameof(objArr2));
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
            var result = ComparisonBuilder.Create()
                .Compare([obj1, obj2], [obj1, obj2]);

            // Assert
            result.AllMatched.ShouldBeTrue();
        }

        [Fact]
        public void Compare_TwoSameAnonymousTypeArrays_WithReferenceComparison_ReturnsNotMatchingResult()
        {
            // Arrange
            var obj1 = new { Name = "Test", Value = 123 };
            var obj2 = new { Name = "Test", Value = 456 };
            var objArr1 = new[] { obj1, obj2 };
            var objArr2 = new[] { obj1, obj2 };

            // Act
            var result = ComparisonBuilder.Create()
                .UseReferenceEquality()
                .Compare(objArr1, objArr2);

            // Assert
            result.AllMatched.ShouldBeFalse();
            result.MismatchCount.ShouldBe(1);
            _testOutputHelper.WriteLine(result.ToString());
            result.Mismatches.First().Code.ShouldBe(ComparisonMismatches.Object.MismatchDetectedByReferenceCode);
            result.Mismatches.First().Message.ShouldContain(nameof(objArr1));
            result.Mismatches.First().Message.ShouldContain(nameof(objArr2));
        }
    }
}
