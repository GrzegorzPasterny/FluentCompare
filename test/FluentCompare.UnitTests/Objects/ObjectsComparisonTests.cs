using FluentCompare.Configuration;
using Shouldly;

namespace FluentCompare.UnitTests.Objects
{
	public class ObjectsComparisonTests
	{
		[Fact]
		public void Compare_TwoIdenticalObjects_ReturnsEqualResult()
		{
			// Arrange
			var obj1 = new { Name = "Test", Value = 123 };
			var obj2 = new { Name = "Test", Value = 123 };

			// Act
			var result = new ComparisonBuilder()
				.Build()
				.Compare(obj1, obj2);

			// Assert
			result.AllMatched.ShouldBeTrue();
		}

		[Fact]
		public void Compare_TwoDifferentObjects_ReturnsNotEqualResult()
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
