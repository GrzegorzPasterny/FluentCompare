namespace FluentCompare.UnitTests.Integers
{
    public class IntComparisonTests
    {
        [Fact]
        public void Compare_TwoEqualIntegers_ReturnsAllMatchingResult()
        {
            // Arrange
            int int1 = 42;
            int int2 = 42;

            // Act
            var result = new ComparisonBuilder()
                .Compare(int1, int2);

            // Assert
            result.AllMatched.ShouldBeTrue();
        }

        [Fact]
        public void Compare_TwoDifferentIntegers_ReturnsNotMatchingResult()
        {
            // Arrange
            int int1 = 42;
            int int2 = 43;

            // Act
            var result = new ComparisonBuilder()
                .Compare(int1, int2);

            // Assert
            result.AllMatched.ShouldBeFalse();
            result.Mismatches.First().Message.ShouldContain($"{nameof(int1)}");
            result.Mismatches.First().Message.ShouldContain($"{nameof(int2)}");
        }

        [Fact]
        public void Compare_TwoEqualIntArrays_ReturnsAllMatchingResult()
        {
            // Arrange
            int[] array1 = { 1, 2, 3, 4, 5 };
            int[] array2 = { 1, 2, 3, 4, 5 };

            // Act
            var result = new ComparisonBuilder()
            .Compare(array1, array2);

            // Assert
            result.AllMatched.ShouldBeTrue();
        }

        [Fact]
        public void Compare_TwoDifferentIntArrays_ReturnsNotMatchingResult()
        {
            // Arrange
            int[] array1 = { 1, 2, 3, 4, 5 };
            int[] array2 = { 1, 2, 3, 4, 6 };

            // Act
            var result = new ComparisonBuilder()
            .Compare(array1, array2);

            // Assert
            result.AllMatched.ShouldBeFalse();
        }

        [Fact]
        public void Compare_IntArraysWithDifferentLengths_ReturnsNotMatchingResult()
        {
            // Arrange
            int[] array1 = { 1, 2, 3, 4, 5 };
            int[] array2 = { 1, 2, 3, 4 };

            // Act
            var result = new ComparisonBuilder()
            .Compare(array1, array2);

            // Assert
            result.AllMatched.ShouldBeFalse();
        }
    }
}
