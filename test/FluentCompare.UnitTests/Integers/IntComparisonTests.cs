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
        public void Compare_TwoDifferentIntegers_WithoutVariableNames_ReturnsAllMatchingResult()
        {
            // Act
            var result = new ComparisonBuilder()
                .Compare(0, 1);

            // Assert
            result.AllMatched.ShouldBeFalse();
            result.MismatchCount.ShouldBe(1);
            result.Mismatches.First().Code.ShouldBe(ComparisonMismatches<int>.MismatchDetectedCode);
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
            result.MismatchCount.ShouldBe(1);
            result.Mismatches.First().Code.ShouldBe(ComparisonMismatches<int>.MismatchDetectedCode);
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
            result.Mismatches.First().Message.ShouldContain($"{nameof(array1)}");
            result.Mismatches.First().Message.ShouldContain($"{nameof(array2)}");
        }

        [Fact]
        public void Compare_IntArraysWithDifferentLengths_ReturnsComparisonError()
        {
            // Arrange
            int[] array1 = { 1, 2, 3, 4, 5 };
            int[] array2 = { 1, 2, 3, 4 };

            // Act
            var result = new ComparisonBuilder()
            .Compare(array1, array2);

            // Assert
            result.WasSuccessful.ShouldBeFalse();
            result.ErrorCount.ShouldBe(1);
            result.Errors.First().Code.ShouldBe(ComparisonErrors.InputArrayLengthsDifferCode);
        }

        [Fact]
        public void Compare_TwoDifferentAnonymousIntArrays_ReturnsNotMatchingResult()
        {
            // Act
            var result = new ComparisonBuilder()
                .Compare<int[]>([1, 2, 3, 4, 5], [1, 2, 3, 4, 6]);

            // Assert
            result.WasSuccessful.ShouldBeTrue();
            result.AllMatched.ShouldBeFalse();
            result.MismatchCount.ShouldBe(1);
            result.Mismatches.First().Code.ShouldBe(ComparisonMismatches<int>.MismatchDetectedCode);
            // Those messages contain the array values, because there are no variable names.
            // TODO: Consider improving the message formatting for anonymous arrays
            result.Mismatches.First().Message.ShouldContain("[1, 2, 3, 4, 5]");
            result.Mismatches.First().Message.ShouldContain("[1, 2, 3, 4, 6]");
        }

        [Fact]
        public void Compare_TwoIntArrays_OneArrayIsNull_ReturnsNotMatchingResult()
        {
            // Act
            var result = new ComparisonBuilder()
            .Compare<int[]>([1, 2, 3, 4, 5], null);

            // Assert
            result.WasSuccessful.ShouldBeFalse();
            result.ErrorCount.ShouldBe(1);
            result.Mismatches.First().Code.ShouldBe(ComparisonMismatches<int>.MismatchDetectedCode);
            // Those messages contain the array values, because there are no variable names.
            // TODO: Consider improving the message formatting for anonymous arrays
            result.Mismatches.First().Message.ShouldContain("[1, 2, 3, 4, 5]");
            result.Mismatches.First().Message.ShouldContain("[1, 2, 3, 4, 6]");
        }
    }
}
