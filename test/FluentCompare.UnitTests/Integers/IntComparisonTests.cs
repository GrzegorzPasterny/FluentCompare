namespace FluentCompare.UnitTests.Integers
{
    public class IntComparisonTests
    {
        private readonly ITestOutputHelper _output;

        public IntComparisonTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public static TheoryData<int[]?, int, int, string?> ParamsIntCases =>
            new()
            {
                { null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
                { new[] { 1 }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
                { new[] { 1, 1 }, 0, 0, null },
                { new[] { 1, 2 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
                { new[] { 1, 1, 2 }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            };

        public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, int[][]?, int, int, string?> IntArrayParamsCases =>
            new()
            {
                { b => b, null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
                { b => b, new[] { new[] { 1 } }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
                { b => b, new int[][] { null!, new[] { 1 } }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
                { b => b, new int[][] { new[] { 1 }, null! }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
                { b => b.DisallowNullComparison(), new int[][] { new[] { 1 }, null! }, 0, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
                { b => b, new[] { new[] { 1, 2 }, new[] { 1 } }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
                { b => b, new[] { new[] { 1, 2 }, new[] { 1, 3 } }, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
                { b => b, new[] { new[] { 1, 2 }, new[] { 1, 2 } }, 0, 0, null },
            };

        public static TheoryData<int?[]?, int?[]?, bool, int, int, string?> NullableIntArrayPairCases =>
            new()
            {
                { new int?[] { 1, 2, 3 }, new int?[] { 1, 9, 3 }, false, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
                { new int?[] { 1, null, 3 }, new int?[] { 1, 2, 3 }, false, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
                { new int?[] { 1, 2, 3 }, new int?[] { 1, 9, 3 }, true, 1, 0, ComparisonMismatches<int>.MismatchDetectedCode },
            };

        private void AssertFirstMismatchCode(ComparisonResult result, string expectedCode)
        {
            _output.WriteLine(result.ToString());
            result.MismatchCount.ShouldBeGreaterThan(0, result.ToString());
            result.Mismatches[0].Code.ShouldBe(expectedCode, result.Mismatches[0].Message);
        }

        private void AssertFirstErrorCode(ComparisonResult result, string expectedCode)
        {
            _output.WriteLine(result.ToString());
            result.ErrorCount.ShouldBeGreaterThan(0, result.ToString());
            result.Errors[0].Code.ShouldBe(expectedCode, result.Errors[0].Message);
        }

        [Fact]
        public void Compare_TwoEqualIntegers_ReturnsAllMatchingResult()
        {
            // Arrange
            int int1 = 42;
            int int2 = 42;

            // Act
            var result = ComparisonBuilder.Create()
                .Compare(int1, int2);

            // Assert
            _output.WriteLine(result.ToString());
            result.AllMatched.ShouldBeTrue();
        }

        [Fact]
        public void Compare_TwoDifferentIntegers_WithoutVariableNames_ReturnsAllMatchingResult()
        {
            // Act
            var result = ComparisonBuilder.Create()
                .Compare(0, 1);

            // Assert
            _output.WriteLine(result.ToString());
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
            var result = ComparisonBuilder.Create()
                .Compare(int1, int2);

            // Assert
            _output.WriteLine(result.ToString());
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
            object array1 = new int[] { 1, 2, 3, 4, 5 };
            object array2 = new int[] { 1, 2, 3, 4, 5 };

            // Act
            var result = ComparisonBuilder.Create()
            .Compare(array1, array2);

            // Assert
            _output.WriteLine(result.ToString());
            result.AllMatched.ShouldBeTrue();
        }

        [Fact]
        public void Compare_TwoDifferentIntArrays_ReturnsNotMatchingResult()
        {
            // Arrange
            int[] array1 = { 1, 2, 3, 4, 5 };
            int[] array2 = { 1, 2, 3, 4, 6 };

            // Act
            var result = ComparisonBuilder.Create()
            .Compare(array1, array2);

            // Assert
            _output.WriteLine(result.ToString());
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
            var result = ComparisonBuilder.Create()
            .Compare(array1, array2);

            // Assert
            _output.WriteLine(result.ToString());
            result.WasSuccessful.ShouldBeFalse();
            result.ErrorCount.ShouldBe(1);
            result.Errors.First().Code.ShouldBe(ComparisonErrors.InputArrayLengthsDifferCode);
        }

        [Fact]
        public void Compare_TwoDifferentAnonymousIntArrays_ReturnsNotMatchingResult()
        {
            // Act
            var result = ComparisonBuilder.Create()
                .Compare<int[]>([1, 2, 3, 4, 5], [1, 2, 3, 4, 6]);

            // Assert
            _output.WriteLine(result.ToString());
            result.WasSuccessful.ShouldBeTrue();
            result.AllMatched.ShouldBeFalse();
            result.MismatchCount.ShouldBe(1);
            result.Mismatches.First().Code.ShouldBe(ComparisonMismatches<int>.MismatchDetectedCode);
            result.Mismatches.First().Message.ShouldContain("[1, 2, 3, 4, 5]");
            result.Mismatches.First().Message.ShouldContain("[1, 2, 3, 4, 6]");
        }

        [Theory]
        [MemberData(nameof(ParamsIntCases))]
        public void Compare_ParamsInt_UsesExpectedOutcome(
            int[]? values,
            int expectedMismatches,
            int expectedErrors,
            string? expectedCode)
        {
            var builder = ComparisonBuilder.Create();
            var result = values is null
                ? builder.Compare((int[]?)null)
                : builder.Compare(values);

            result.MismatchCount.ShouldBe(expectedMismatches);
            result.ErrorCount.ShouldBe(expectedErrors);

            if (expectedCode is not null)
            {
                if (expectedErrors > 0)
                {
                    AssertFirstErrorCode(result, expectedCode);
                }
                else
                {
                    AssertFirstMismatchCode(result, expectedCode);
                }
            }
        }

        [Theory]
        [MemberData(nameof(IntArrayParamsCases))]
        public void Compare_IntArrayParams_UsesExpectedOutcome(
            Func<ComparisonBuilder, ComparisonBuilder> configure,
            int[][]? values,
            int expectedMismatches,
            int expectedErrors,
            string? expectedCode)
        {
            var builder = configure(ComparisonBuilder.Create());
            var result = builder.Compare(values);

            result.MismatchCount.ShouldBe(expectedMismatches);
            result.ErrorCount.ShouldBe(expectedErrors);

            if (expectedCode is not null)
            {
                if (expectedErrors > 0)
                {
                    AssertFirstErrorCode(result, expectedCode);
                }
                else
                {
                    AssertFirstMismatchCode(result, expectedCode);
                }
            }
        }

        [Theory]
        [MemberData(nameof(NullableIntArrayPairCases))]
        public void Compare_NullableIntArrayPair_UsesExpectedOutcome(
            int?[]? first,
            int?[]? second,
            bool useObjectOverload,
            int expectedMismatches,
            int expectedErrors,
            string? expectedCode)
        {
            var builder = ComparisonBuilder.Create();
            var result = useObjectOverload
                ? builder.Compare((object?)first, (object?)second)
                : builder.Compare(first, second);

            result.MismatchCount.ShouldBe(expectedMismatches);
            result.ErrorCount.ShouldBe(expectedErrors);

            if (expectedCode is not null)
            {
                if (expectedErrors > 0)
                {
                    AssertFirstErrorCode(result, expectedCode);
                }
                else
                {
                    AssertFirstMismatchCode(result, expectedCode);
                }
            }
        }

        [Fact]
        public void Compare_TwoIntArrays_OneArrayIsNull_ReturnsNotMatchingResult()
        {
            // Act
            var result = ComparisonBuilder.Create()
            .Compare<int[]?>([1, 2, 3, 4, 5], null);

            // Assert
            _output.WriteLine(result.ToString());
            result.WasSuccessful.ShouldBeTrue();
            result.AllMatched.ShouldBeFalse();
            result.MismatchCount.ShouldBe(1);
            result.Mismatches.First().Code.ShouldBe(ComparisonMismatches.NullPassedAsArgumentCode);
        }
    }
}
