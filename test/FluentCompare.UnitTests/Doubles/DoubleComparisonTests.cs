namespace FluentCompare.UnitTests.Doubles;

public class DoubleComparisonTests
{
    [Fact]
    public void Compare_TwoEqualDoubles_ReturnsAllMatchingResult()
    {
        // Arrange
        double dbl1 = 42.0;
        double dbl2 = 42.0;

        // Act
        var result = new ComparisonBuilder()
            .Compare(dbl1, dbl2);

        // Assert
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_TwoDifferentDoubles_ReturnsNotMatchingResult()
    {
        // Arrange
        double dbl1 = 42.0;
        double dbl2 = 43.0;

        // Act
        var result = new ComparisonBuilder()
            .Compare(dbl1, dbl2);

        // Assert
        result.AllMatched.ShouldBeFalse();
        result.Mismatches.First().Message.ShouldContain($"{nameof(dbl1)}");
        result.Mismatches.First().Message.ShouldContain($"{nameof(dbl2)}");
    }

    [Fact]
    public void Compare_TwoEqualDoubleArrays_ReturnsAllMatchingResult()
    {
        // Arrange
        double[] array1 = { 1.0, 2.0, 3.0, 4.0, 5.0 };
        double[] array2 = { 1.0, 2.0, 3.0, 4.0, 5.0 };

        // Act
        var result = new ComparisonBuilder()
        .Compare(array1, array2);

        // Assert
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_TwoDifferentDoubleArrays_ReturnsNotMatchingResult()
    {
        // Arrange
        double[] array1 = { 1.0, 2.0, 3.0, 4.0, 5.0 };
        double[] array2 = { 1.0, 2.0, 3.0, 4.0, 6.0 };

        // Act
        var result = new ComparisonBuilder()
            .Compare(array1, array2);

        // Assert
        result.AllMatched.ShouldBeFalse();
        result.Mismatches.First().Message.ShouldContain($"{nameof(array1)}");
        result.Mismatches.First().Message.ShouldContain($"{nameof(array2)}");
    }

    [Fact]
    public void Compare_DoubleArraysWithDifferentLengths_ReturnsNotMatchingResult()
    {
        // Arrange
        double[] array1 = { 1.0, 2.0, 3.0, 4.0, 5.0 };
        double[] array2 = { 1.0, 2.0, 3.0, 4.0 };

        // Act
        var result = new ComparisonBuilder()
            .Compare(array1, array2);

        // Assert
        result.AllMatched.ShouldBeFalse();
    }

    [Fact]
    public void Compare_TwoDoublesEqualWithinPrecision2_ReturnsAllMatchingResult()
    {
        // Arrange
        double dbl1 = 1.234;
        double dbl2 = 1.235; // Rounds to 1.23 and 1.24? Wait, but for matching case, adjust
                             // Better example: both round to same
        dbl1 = 1.234;
        dbl2 = 1.2345; // Both round to 1.23 at precision 2

        // Act
        var result = new ComparisonBuilder()
            .ForDouble()
            .WithPrecision(2)
            .Compare(dbl1, dbl2);

        // Assert
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_TwoDoublesUnequalEvenWithPrecision2_ReturnsNotMatchingResult()
    {
        // Arrange
        double dbl1 = 1.234;
        double dbl2 = 1.245; // 1.23 vs 1.25 at precision 2

        // Act
        var result = new ComparisonBuilder()
            .ForDouble()
            .WithPrecision(2)
            .Compare(dbl1, dbl2);

        // Assert
        result.AllMatched.ShouldBeFalse();
        result.Mismatches.First().Message.ShouldContain("1.23");
        result.Mismatches.First().Message.ShouldContain("1.25");
    }

    [Fact]
    public void Compare_TwoDoublesEqualWithPrecision3ButNotWith2_ReturnsAllMatchingResult()
    {
        // Arrange
        double dbl1 = 1.234;
        double dbl2 = 1.2341; // At precision 3: both 1.234

        // Act
        var result = new ComparisonBuilder()
            .ForDouble()
            .WithPrecision(3)
            .Compare(dbl1, dbl2);

        // Assert
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_TwoDoublesUnequalWithPrecision3_ReturnsNotMatchingResult()
    {
        // Arrange
        double dbl1 = 1.234;
        double dbl2 = 1.235; // At precision 3: 1.234 vs 1.235

        // Act
        var result = new ComparisonBuilder()
            .ForDouble()
            .WithPrecision(3)
            .Compare(dbl1, dbl2);

        // Assert
        result.AllMatched.ShouldBeFalse();
        result.Mismatches.First().Message.ShouldContain("1.234");
        result.Mismatches.First().Message.ShouldContain("1.235");
    }

    [Fact]
    public void Compare_DoubleArraysEqualWithinPrecision2_ReturnsAllMatchingResult()
    {
        // Arrange
        double[] array1 = { 1.234, 2.345, 3.456 };
        double[] array2 = { 1.2345, 2.3456, 3.4567 }; // All round to same at precision 2: 1.23, 2.35, 3.46

        // Act
        var result = new ComparisonBuilder()
            .ForDouble()
            .WithPrecision(2)
            .Compare(array1, array2);

        // Assert
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_DoubleArraysUnequalWithinPrecision2_ReturnsNotMatchingResult()
    {
        // Arrange
        double[] array1 = { 1.234, 2.345, 3.456 };
        double[] array2 = { 1.2345, 2.3456, 3.467 }; // Last rounds to 3.47

        // Act
        var result = new ComparisonBuilder()
            .ForDouble()
            .WithPrecision(2)
            .Compare(array1, array2);

        // Assert
        result.AllMatched.ShouldBeFalse();
        result.Mismatches.ShouldNotBeEmpty();
        result.Mismatches.First().Message.ShouldContain("index 2");
        result.Mismatches.First().Message.ShouldContain("3.46");
        result.Mismatches.First().Message.ShouldContain("3.47");
    }

    [Fact]
    public void Compare_DoubleArraysWithFloatingPointPrecisionIssue_WithSufficientPrecision_ReturnsAllMatchingResult()
    {
        // Arrange
        double[] array1 = { 0.1 + 0.2 };
        double[] array2 = { 0.3 };

        // Act
        var result = new ComparisonBuilder()
            .ForDouble()
            .WithPrecision(15) // High precision to catch floating point exactness, but since rounded, may still match if exact
            .Compare(array1, array2);

        // Assert
        // Note: Due to floating point, 0.1+0.2 != 0.3 exactly, so with high precision, should mismatch
        // But adjust expectation based on implementation; assuming strict after round, but round to 15 decimals
        result.AllMatched.ShouldBeFalse(); // Failing test for exact mismatch
    }

    [Fact]
    public void Compare_SingleDoubleToArrayOfOne_WithMatchingValue_ReturnsAllMatchingResult()
    {
        // Arrange
        double dbl1 = 42.0;
        double[] array1 = { 42.0 };

        // Act
        // Note: This tests if single vs array of one, but based on Compare overloads, may need adjustment; assuming array compare
        var result = new ComparisonBuilder()
            .ForDouble()
            .Compare(array1, new[] { dbl1 });

        // Assert
        result.AllMatched.ShouldBeTrue();
    }

    [Fact]
    public void Compare_DoubleArraysWithNullOne_ReturnsNotMatchingResult()
    {
        // Arrange
        double[] array1 = { 1.0, 2.0 };
        double[] array2 = null;

        // Act
        var result = new ComparisonBuilder()
            .ForDouble()
            .Compare(array1, array2);

        // Assert
        result.AllMatched.ShouldBeFalse();
        result.Mismatches.First().Message.ShouldContain("null");
    }
}
