namespace FluentCompare.UnitTests.Bytes;

public class ByteComparisonTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ByteComparisonTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private ComparisonBuilder CreateBuilder(ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        return ComparisonBuilder.Create()
            .UseComparisonType(comparisonType);
    }

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, byte, byte, int, int, string?> SingleByteCases =>
        new()
        {
            { b => b, 5, 5, 0, 0, null },
            { b => b, 10, 20, 1, 0, ComparisonMismatches<byte>.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.Or, 0x0F, 0), 0xFF, 0x0F, 1, 0, ComparisonMismatches<byte>.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.And, 0x0F), 0x3F, 0x1F, 0, 0, null },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.Or, 0xF0), 0x0A, 0x0B, 1, 0, ComparisonMismatches.Byte.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.Xor, 0x0F), 0xF0, 0xF0, 0, 0, null },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.Not, 0x00), 0b_1010_1010, 0b_1010_1010, 0, 0, null },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.ShiftLeft, 1), 0b_0000_0010, 0b_0000_0010, 0, 0, null },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.ShiftRight, 1), 0b_0000_0100, 0b_0000_0100, 0, 0, null },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.And, 0x0F).ApplyBitwiseOperation(BitwiseOperation.Xor, 0x05), 0x3F, 0x2F, 0, 0, null },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.And, 0x0F).ApplyBitwiseOperation(BitwiseOperation.Xor, 0x05), 0x3F, 0x30, 1, 0, ComparisonMismatches.Byte.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.ShiftLeft, 1), 0b_0000_0010, 0b_0000_0100, 1, 0, ComparisonMismatches.Byte.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.ShiftRight, 1), 0b_0000_0100, 0b_0000_0010, 1, 0, ComparisonMismatches.Byte.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.ShiftLeft, 100), 0b_1111_0010, 0b_0000_0100, 1, 0, ComparisonMismatches.Byte.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.ShiftRight, 100), 0b_0000_0100, 0b_1111_0100, 1, 0, ComparisonMismatches.Byte.MismatchDetectedCode },
        };

    public static TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, byte[], int, int, string?> ParamsByteCases =>
        new()
        {
            { b => b, new byte[] { 1 }, 0, 1, ComparisonErrors.NotEnoughObjectsToCompareCode },
            { b => b, new byte[] { 1, 2 }, 1, 0, ComparisonMismatches<byte>.MismatchDetectedCode },
            { b => b, new byte[] { 1, 1, 2 }, 1, 0, ComparisonMismatches<byte>.MismatchDetectedCode },
            { b => b.ApplyBitwiseOperation(BitwiseOperation.Xor, 0x01), new byte[] { 0x01, 0x01, 0x03 }, 1, 0, ComparisonMismatches.Byte.MismatchDetectedCode },
        };

    public static TheoryData<byte?, byte?, int, int, string?> NullableByteObjectCases =>
        new()
        {
            { 1, 2, 1, 0, ComparisonMismatches<byte>.MismatchDetectedCode },
            { 7, 7, 0, 0, null },
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
    [MemberData(nameof(SingleByteCases))]
    public void Compare_Byte_WithConfigurations_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        byte left,
        byte right,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = configure(CreateBuilder());
        var result = builder.Compare(left, right);

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
    [MemberData(nameof(ParamsByteCases))]
    public void Compare_ParamsByte_UsesExpectedOutcome(
        Func<ComparisonBuilder, ComparisonBuilder> configure,
        byte[] bytes,
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

    [Theory]
    [MemberData(nameof(NullableByteObjectCases))]
    public void Compare_ObjectOverload_NullableByte_UsesExpectedOutcome(
        byte? left,
        byte? right,
        int expectedMismatches,
        int expectedErrors,
        string? expectedCode)
    {
        var builder = CreateBuilder();
        object? leftObject = left;
        object? rightObject = right;

        var result = builder.Compare(leftObject, rightObject);

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
