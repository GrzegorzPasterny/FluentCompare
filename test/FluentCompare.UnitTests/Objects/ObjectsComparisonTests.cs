using FluentCompare.Configuration;
using Shouldly;

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
				.Build()
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
				.Build()
				.Compare(obj1, obj2);

			// Assert
			result.AllMatched.ShouldBeFalse();
		}

		[Fact]
		public void Compare_TwoNonEquivalentAnonymousTypes_WithDefaultConfiguration_ReturnsNotEqualResult()
		{
			// Arrange
			var obj1 = new { Name = "Test", Value = 123 };
			var obj2 = new { Name = "Test", Value = 456 };

			// Act
			var result = new ComparisonBuilder()
				.Build()
				.Compare(obj1, obj2);

			// Assert
			result.AllMatched.ShouldBeFalse();
		}
	}
}
