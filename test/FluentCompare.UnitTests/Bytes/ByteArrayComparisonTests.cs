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

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, byte[]?, byte[]?, int, int, string?> ByteArrayPairCases =>
        new()
        {
            { b => b, new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 3 }, 0, 0, null },
            { b => b, new byte[] { 1, 2, 3 }, new byte[] { 1, 9, 3 }, 1, 0, ComparisonMismatches<byte>.MismatchDetectedCode },
            { b => b, new byte[] { 1, 2 }, new byte[] { 1, 2, 3 }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.And, 0xAF, 1), new byte[] { 0xFB, 0xFC }, new byte[] { 0xAB, 0xAC }, 0, 0, null },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.Or, 0x02), new byte[] { 0x00 }, new byte[] { 0x01 }, 1, 0, ComparisonMismatches.Byte.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.Xor, 0x01), new byte[] { 0x00 }, new byte[] { 0x01 }, 1, 0, ComparisonMismatches.Byte.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.ShiftLeft, 1), new byte[] { 0x80 }, new byte[] { 0x00 }, 0, 0, null },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.ShiftRight, 1), new byte[] { 0x01 }, new byte[] { 0x00 }, 0, 0, null },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.Not, 0x00), new byte[] { 0b_0000_1111, 0b_1111_0000 }, new byte[] { 0b_0000_1111, 0b_1111_0000 }, 0, 0, null },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, byte[][]?, int, int, string?> ByteArrayParamsCases =>
        new()
        {
            { b => b, null, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { b => b, new byte[0][], 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new byte[1][] { new byte[] { 1 } }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new byte[2][] { new byte[] { 1, 2 }, new byte[] { 1, 2 } }, 0, 0, null },
            { b => b, new byte[2][] { new byte[] { 1, 2, 3 }, new byte[] { 1, 9, 3 } }, 1, 0, ComparisonMismatches<byte>.MismatchDetectedCode },
            { b => b, new byte[2][] { new byte[] { 1, 2 }, null! }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b, new byte[2][] { new byte[] { 1, 2 }, new byte[] { 1 } }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.And, 0x0F), new byte[2][] { new byte[] { 0xFF, 0xE1 }, new byte[] { 0x0F, 0xE0 } }, 1, 0, ComparisonMismatches.Byte.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.And, 0x0F), new byte[3][] { new byte[] { 0xFF, 0xE1 }, new byte[] { 0x0F, 0xE1 }, null! }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.And, 0x0F), new byte[2][] { new byte[] { 0xFF, 0xE1 }, new byte[] { 0x0F, 0xE1, 0x23 } }, 0, 1, ComparisonErrors.InputArrayLengthsDifferCode },
            { b => b.DisallowNullComparison(), new byte[2][] { new byte[] { 1, 2 }, null! }, 0, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
        };

    public static TheoryData<byte?[]?, byte?[]?, bool, int, int, string?> NullableByteArrayPairCases =>
        new()
        {
            { new byte?[] { 1, 2, 3 }, new byte?[] { 1, 9, 3 }, false, 1, 0, ComparisonMismatches<byte>.MismatchDetectedCode },
            { new byte?[] { 1, null, 3 }, new byte?[] { 1, 2, 3 }, false, 0, 1, ComparisonErrors.NullPassedAsArgumentCode },
            { new byte?[] { 1, 2, 3 }, new byte?[] { 1, 9, 3 }, true, 1, 0, ComparisonMismatches<byte>.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, byte?[][]?, int, int, string?> NullableByteArrayParamsCases =>
        new()
        {
            { b => b, new byte?[][] { new byte?[] { 1, 2 }, new byte?[] { 1, 2 } }, 0, 0, null },
            { b => b, new byte?[][] { new byte?[] { 1, 2 }, null! }, 1, 0, ComparisonMismatches.NullPassedAsArgumentCode },
            { b => b.DisallowNullComparison(), new byte?[][] { new byte?[] { 1, 2 }, null! }, 0, 1, ComparisonErrors.OneOfTheObjectsIsNullCode },
        };

    private void LogResult(params ComparisonResult[] results)
    {
        foreach (var result in results)
        {
            _testOutputHelper.WriteLine(result.ToString());

            foreach (var mismatch in result.Mismatches)
            {
                _testOutputHelper.WriteLine($"Mismatch [{mismatch.Code}]: {mismatch.Message}");
            }

            foreach (var error in result.Errors)
            {
                _testOutputHelper.WriteLine($"Error [{error.Code}]: {error.Message}");
            }

            foreach (var warning in result.Warnings)
            {
                _testOutputHelper.WriteLine($"Warning [{warning.Code}]: {warning.Message}");
            }
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
    [MemberData(nameof(ByteArrayPairCases))]
    public void Compare_ByteArrayPair_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        byte[]? first,
        byte[]? second,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = builder.Compare(first, second);

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
    [MemberData(nameof(ByteArrayParamsCases))]
    public void Compare_ByteArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        byte[][]? bytes,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = bytes is null
            ? builder.Compare((byte[][]?)null)
            : builder.Compare(bytes);

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
    [MemberData(nameof(NullableByteArrayPairCases))]
    public void Compare_NullableByteArrayPair_UsesExpectedOutcome(
        byte?[]? first,
        byte?[]? second,
        bool useObjectOverload,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();

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

    [Theory]
    [MemberData(nameof(NullableByteArrayParamsCases))]
    public void Compare_NullableByteArrayParams_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        byte?[][]? bytes,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = builder.Compare(bytes);

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
}
