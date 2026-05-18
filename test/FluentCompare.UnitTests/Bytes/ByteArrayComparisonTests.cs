namespace FluentCompare.UnitTests.Bytes;

public class ByteArrayComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ByteArrayComparisonTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    // Entry point: Compare<T>(T t1, T t2) : where T is byte[]
    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, byte[], byte[], int, string?> ByteArrayBitwiseCases =>
        new()
        {
            { b => b.ApplyBitwiseOperation(BitwiseOperation.Or, 0x02), new byte[] { 0x00 }, new byte[] { 0x01 }, 1, ComparisonMismatches.Byte.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.Xor, 0x01), new byte[] { 0x00 }, new byte[] { 0x01 }, 1, ComparisonMismatches.Byte.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.ShiftLeft, 1), new byte[] { 0x80 }, new byte[] { 0x00 }, 0, null },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.ShiftRight, 1), new byte[] { 0x01 }, new byte[] { 0x00 }, 0, null },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.Not, 0x00), new byte[] { 0b_0000_1111, 0b_1111_0000 }, new byte[] { 0b_0000_1111, 0b_1111_0000 }, 0, null },
        };

    private void LogResult(params ComparisonResult[] results)
    {
        foreach (var result in results)
        {
            _testOutputHelper.WriteLine(result.ToString());
        }
    }

    private void AssertFirstMismatchCode(ComparisonResult result, string expectedCode)
    {
        LogResult(result);
        result.MismatchCount.ShouldBeGreaterThan(0, result.ToString());
        result.Mismatches[0].Code.ShouldBe(expectedCode, result.Mismatches[0].Message);
    }

    private void AssertFirstErrorCode(ComparisonResult result, string expectedCode)
    {
        LogResult(result);
        result.ErrorCount.ShouldBeGreaterThan(0, result.ToString());
        result.Errors[0].Code.ShouldBe(expectedCode, result.Errors[0].Message);
    }

    [Theory]
    [MemberData(nameof(ByteArrayBitwiseCases))]
    public void Compare_ByteArrays_WithBitwiseOperations_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        byte[] first,
        byte[] second,
        int expectedMismatches,
        string? expectedMismatchCode)
    {
        // Arrange
        var builder = configure(CreateBuilder());

        // Act
        var result = builder.Compare(first, second);
        LogResult(result);

        // Assert
        result.MismatchCount.ShouldBe(expectedMismatches);
        result.AllMatched.ShouldBe(expectedMismatches == 0);

        if (expectedMismatchCode is not null)
        {
            AssertFirstMismatchCode(result, expectedMismatchCode);
        }
    }

    [Fact]
    public void Compare_ByteArray_SkipsBitwiseOperation_WhenIndexExcluded()
    {
        // Arrange: Exclude index 1 from bitwise AND, so second array is not transformed
        var builder = CreateBuilder()
            .ApplyBitwiseOperation(BitwiseOperation.And, 0xAF, 1);

        var arr1 = new byte[] { 0xFB, 0xFC };
        var arr2 = new byte[] { 0xAB, 0xAC };
        // Act
        var result = builder.Compare(arr1, arr2);
        LogResult(result);
        // Assert: arr2 is not transformed, arr1 is transformed, so both arrays are still equal
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_ParamsByteArray_UsesByteArrayComparisonPath()
    {
        // Arrange
        var builder = CreateBuilder();
        // Act
        var result = builder.Compare(new byte[] { 1, 2 }, new byte[] { 1, 3 });
        LogResult(result);
        // Assert
        result.AllMatched.ShouldBeFalse();
        result.MismatchCount.ShouldBe(1);
    }

    [Fact]
    public void Compare_ByteJaggedArray_UsesByteJaggedArrayComparisonPath()
    {
        // Arrange
        var builder = CreateBuilder();
        byte[][] arr = new byte[][] { new byte[] { 1, 2 }, new byte[] { 1, 2 } };
        // Act
        var result = builder.Compare(arr);
        LogResult(result);
        // Assert
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_ByteJaggedArray_UseNullableTypes_UsesByteJaggedArrayComparisonPath()
    {
        // Arrange
        var builder = CreateBuilder();
        byte?[][]? arr =
        [
            [1, 2],
            [1, 2]
        ];
        // Act
        var result = builder.Compare(arr);
        LogResult(result);
        // Assert
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_BothNull_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare((byte[]?)null!);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.ErrorCount.ShouldBe(1);
        AssertFirstErrorCode(result, ComparisonErrors.NullPassedAsArgumentCode);
    }

    [Fact]
    public void Compare_ByteArrays_AllEqual_ShouldMatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 3 });

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
        result.Errors.ShouldBeEmpty();
        result.Warnings.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_ByteArrays_DifferentLengths_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(new byte[] { 1, 2 }, new byte[] { 1, 2, 3 });

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.ErrorCount.ShouldBe(1);
        AssertFirstErrorCode(result, ComparisonErrors.InputArrayLengthsDifferCode);
        result.AllMatched.ShouldBeFalse();
        result.WasSuccessful.ShouldBeFalse();
    }

    [Fact]
    public void Compare_ByteArrays_Mismatch_ShouldDetectMismatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(new byte[] { 1, 2, 3 }, new byte[] { 1, 9, 3 });

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.MismatchCount.ShouldBe(1);
        AssertFirstMismatchCode(result, ComparisonMismatches<byte>.MismatchDetectedCode);
        result.AllMatched.ShouldBeFalse();
        result.WasSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Compare_ByteJaggedArray_MismatchWithoutBitwise_ShouldDetectMismatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare(
            new byte[] { 0x01, 0x02 },
            new byte[] { 0x01, 0x03 },
            new byte[] { 0x01, 0x02 });
        LogResult(result);

        // Assert
        result.AllMatched.ShouldBeFalse();
        result.MismatchCount.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void Compare_ByteJaggedArray_MismatchWithBitwise_ShouldDetectMismatch()
    {
        // Arrange
        var builder = CreateBuilder()
            .ApplyBitwiseOperation(BitwiseOperation.And, 0x0F);

        // Act
        var result = builder.Compare(
            new byte[] { 0xFF, 0xE1 },
            new byte[] { 0x0F, 0xE0 });
        LogResult(result);

        // Assert
        result.AllMatched.ShouldBeFalse();
        result.MismatchCount.ShouldBe(1);
    }

    [Fact]
    public void Compare_ByteJaggedArray_WithBitwise_Mismatch_CoversByteSpecificMismatch()
    {
        // Arrange: Bitwise AND will transform values so that a mismatch is detected
        var builder = CreateBuilder()
            .ApplyBitwiseOperation(BitwiseOperation.And, 0x0F);

        // 0xFF & 0x0F = 0x0F, 0x0F & 0x0F = 0x0F (match)
        // 0xE1 & 0x0F = 0x01, 0xE0 & 0x0F = 0x00 (mismatch at index 1)
        var arr = new byte[2][] { new byte[] { 0xFF, 0xE1 }, new byte[] { 0x0F, 0xE0 } };

        // Act
        var result = builder.Compare(arr);

        // Assert: Should hit the Byte-specific mismatch branch
        result.AllMatched.ShouldBeFalse();
        result.MismatchCount.ShouldBe(1);
        AssertFirstMismatchCode(result, ComparisonMismatches.Byte.MismatchDetectedCode);
    }

    [Fact]
    public void Compare_ByteJaggedArray_WithBitwise_OneOfTheArraysIsNull_CoversByteSpecificMismatch()
    {
        // Arrange: Bitwise AND will transform values so that a mismatch is detected
        var builder = CreateBuilder()
            .ApplyBitwiseOperation(BitwiseOperation.And, 0x0F);

        // Third array is null
        var arr = new byte[3][] { new byte[] { 0xFF, 0xE1 }, new byte[] { 0x0F, 0xE1 }, null };

        // Act
        var result = builder.Compare(arr);

        // Assert: Should hit the Byte-specific mismatch branch
        result.AllMatched.ShouldBeFalse();
        result.MismatchCount.ShouldBe(1);
        AssertFirstMismatchCode(result, ComparisonMismatches.NullPassedAsArgumentCode);
    }

    [Fact]
    public void Compare_ByteJaggedArray_WithLengthMismatch_CoversByteSpecificMismatch()
    {
        // Arrange: Bitwise AND will transform values so that a mismatch is detected
        var builder = CreateBuilder()
            .ApplyBitwiseOperation(BitwiseOperation.And, 0x0F);

        // second array is longer, causing an error
        var arr = new byte[2][] { new byte[] { 0xFF, 0xE1 }, new byte[] { 0x0F, 0xE1, 0x23 } };

        // Act
        var result = builder.Compare(arr);

        // Assert: Should hit the Byte-specific mismatch branch
        result.AllMatched.ShouldBeFalse();
        result.ErrorCount.ShouldBe(1);
        AssertFirstErrorCode(result, ComparisonErrors.InputArrayLengthsDifferCode);
    }

    [Fact]
    public void Compare_ByteJaggedArray_NullArray_CoversNullBranch()
    {
        // Arrange
        var builder = CreateBuilder();
        // Act
        var result = builder.Compare(new byte[] { 1, 2 }, null);
        // Assert
        result.MismatchCount.ShouldBe(1);
        AssertFirstMismatchCode(result, ComparisonMismatches.NullPassedAsArgumentCode);
    }

    [Fact]
    public void Compare_ByteJaggedArray_DifferentLengths_CoversLengthErrorBranch()
    {
        // Arrange
        var builder = CreateBuilder();
        // Act
        var result = builder.Compare(new byte[] { 1, 2 }, new byte[] { 1 });
        // Assert
        result.ErrorCount.ShouldBe(1);
        AssertFirstErrorCode(result, ComparisonErrors.InputArrayLengthsDifferCode);
    }

    [Fact]
    public void Compare_ByteJaggedArray_MismatchAtNonZeroIndex_CoversMismatchBranch()
    {
        // Arrange
        var builder = CreateBuilder();
        // Act
        var result = builder.Compare(new byte[] { 1, 2, 3 }, new byte[] { 1, 9, 3 });
        // Assert: mismatch at index 1
        result.AllMatched.ShouldBeFalse();
        result.MismatchCount.ShouldBe(1);
        AssertFirstMismatchCode(result, ComparisonMismatches<byte>.MismatchDetectedCode);
    }

    [Fact]
    public void Compare_ByteJaggedArray_TooShort_CoversGuardClause()
    {
        var builder = CreateBuilder();

        var resultEmpty = builder.Compare(new byte[0][]);
        resultEmpty.Errors.Count.ShouldBe(1);
        AssertFirstErrorCode(resultEmpty, ComparisonErrors.NotEnoughObjectsToCompareCode);

        var resultOne = builder.Compare(new byte[1][] { new byte[] { 1 } });
        resultOne.Errors.Count.ShouldBe(1);
        AssertFirstErrorCode(resultOne, ComparisonErrors.NotEnoughObjectsToCompareCode);
    }

    [Fact]
    public void Compare_NullableByteArrays_WithValues_ShouldCompareAsBytes()
    {
        var builder = CreateBuilder();

        byte?[] left = [1, 2, 3];
        byte?[] right = [1, 9, 3];

        var result = builder.Compare(left, right);
        LogResult(result);

        result.AllMatched.ShouldBeFalse();
        result.MismatchCount.ShouldBe(1);
        AssertFirstMismatchCode(result, ComparisonMismatches<byte>.MismatchDetectedCode);
    }

    [Fact]
    public void Compare_NullableByteArrays_WithNullElement_ShouldReturnError()
    {
        var builder = CreateBuilder();

        byte?[] left = [1, null, 3];
        byte?[] right = [1, 2, 3];

        var result = builder.Compare(left, right);
        LogResult(result);

        result.WasSuccessful.ShouldBeFalse();
        result.ErrorCount.ShouldBe(1);
        AssertFirstErrorCode(result, ComparisonErrors.NullPassedAsArgumentCode);
    }

    [Fact]
    public void Compare_ParamsNullableByteArrays_WithNullArray_ShouldReturnMismatch()
    {
        var builder = CreateBuilder();

        byte?[][] arrays =
        [
            [1, 2],
            null
        ];

        var result = builder.Compare(arrays);
        LogResult(result);

        result.AllMatched.ShouldBeFalse();
        result.MismatchCount.ShouldBe(1);
        AssertFirstMismatchCode(result, ComparisonMismatches.NullPassedAsArgumentCode);
    }

    [Fact]
    public void Compare_ObjectOverload_NullableByteArrays_WithDifferentValues_ShouldReturnMismatch()
    {
        var builder = CreateBuilder();

        object left = new byte?[] { 1, 2, 3 };
        object right = new byte?[] { 1, 9, 3 };

        var result = builder.Compare(left, right);
        LogResult(result);

        result.AllMatched.ShouldBeFalse();
        result.MismatchCount.ShouldBe(1);
        AssertFirstMismatchCode(result, ComparisonMismatches<byte>.MismatchDetectedCode);
    }

    [Fact(Skip = "No support for type ?[][] yet, which is treated as an object. The actual content of the arrays are not checked. TODO: Decide how to handle that.")]
    public void Compare_ObjectOverload_NullableByteJaggedArrays_WithInnerNullArray_ShouldReturnMismatch()
    {
        var builder = CreateBuilder();

        object left = new byte?[][]
        {
            new byte?[] { 1, 2 },
            null
        };
        object right = new byte?[][]
        {
            new byte?[] { 1, 2 },
            new byte?[] { 1, 2 }
        };

        var result = builder.Compare(left, right);
        LogResult(result);

        result.AllMatched.ShouldBeFalse();
        result.MismatchCount.ShouldBe(1);
        AssertFirstMismatchCode(result, ComparisonMismatches.NullPassedAsArgumentCode);
    }
}
