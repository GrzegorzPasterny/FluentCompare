using Xunit.Abstractions;

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

    [Fact]
    public void Compare_BothNull_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare((byte[]?)null!);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.NullPassedAsArgumentCode);
    }

    [Fact]
    public void Compare_NotEnoughBytes_ShouldAddError()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare((byte)1);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe(ComparisonErrors.NotEnoughObjectsToCompareCode);
    }

    [Fact]
    public void Compare_EqualBytes_ShouldAllMatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare((byte)5, (byte)5);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
        result.Errors.ShouldBeEmpty();
        result.Warnings.ShouldBeEmpty();
        result.WasSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Compare_NotEqualBytes_ShouldAddMismatch()
    {
        // Arrange
        var builder = CreateBuilder();

        // Act
        var result = builder.Compare((byte)10, (byte)20);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.Mismatches.Count.ShouldBe(1);
        result.AllMatched.ShouldBeFalse();
        result.WasSuccessful.ShouldBeTrue();
    }

    [Theory]
    [InlineData(5, 10, ComparisonType.LessThan, true)]
    [InlineData(10, 5, ComparisonType.GreaterThan, true)]
    [InlineData(10, 10, ComparisonType.EqualTo, true)]
    [InlineData(10, 10, ComparisonType.NotEqualTo, false)]
    [InlineData(5, 10, ComparisonType.GreaterThan, false)]
    [InlineData(5, 10, ComparisonType.GreaterThanOrEqualTo, false)]
    [InlineData(10, 10, ComparisonType.GreaterThanOrEqualTo, true)]
    public void Compare_RespectsComparisonType(
        byte left, byte right, ComparisonType type, bool expectedMatch)
    {
        // Arrange
        var builder = CreateBuilder(type);

        // Act
        var result = builder.Compare(left, right);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        if (expectedMatch)
        {
            result.AllMatched.ShouldBeTrue();
            result.Mismatches.ShouldBeEmpty();
        }
        else
        {
            result.AllMatched.ShouldBeFalse();
            result.Mismatches.Count.ShouldBeGreaterThan(0);
        }
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
        result.Errors.Count.ShouldBe(1);
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
        result.Mismatches.Count.ShouldBe(1);
        result.AllMatched.ShouldBeFalse();
        result.WasSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Compare_WithBitwiseAndOperation_ShouldTransformAndMatch()
    {
        // Arrange
        var builder = CreateBuilder()
            .ApplyBitwiseOperation(BitwiseOperation.And, 0x0F);

        // Act
        var result = builder.Compare((byte)0x3F, (byte)0x1F); // 0x3F & 0x0F == 0x0F, 0x1F & 0x0F == 0x0F

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_WithBitwiseOrOperation_ShouldTransformAndMatch()
    {
        // Arrange
        var builder = CreateBuilder()
            .ApplyBitwiseOperation(BitwiseOperation.Or, 0xF0);

        // Act
        var result = builder.Compare((byte)0x0A, (byte)0x0B); // (0x0A | 0xF0) == 0xFA, (0x0B | 0xF0) == 0xFB

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeFalse();
        result.Mismatches.Count.ShouldBe(1);
    }

    [Fact]
    public void Compare_WithBitwiseXorOperation_ShouldTransformAndMatch()
    {
        // Arrange
        var builder = CreateBuilder()
            .ApplyBitwiseOperation(BitwiseOperation.Xor, 0x0F);

        // Act
        var result = builder.Compare((byte)0xF0, (byte)0xF0);

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_MultipleBitwiseOperations_ShouldApplyInOrder()
    {
        // Arrange
        var builder = CreateBuilder()
            .ApplyBitwiseOperation(BitwiseOperation.And, 0x0F)
            .ApplyBitwiseOperation(BitwiseOperation.Xor, 0x05);

        // Act
        var result = builder.Compare((byte)0x3F, (byte)0x2F);
        // (0x3F & 0x0F) ^ 0x05 == (0x2F & 0x0F) ^ 0x05 == 0x0A

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_MultipleBitwiseOperations_DifferentAfterTransform_ShouldMismatch()
    {
        // Arrange
        var builder = CreateBuilder()
            .ApplyBitwiseOperation(BitwiseOperation.And, 0x0F)
            .ApplyBitwiseOperation(BitwiseOperation.Xor, 0x05);

        // Act
        var result = builder.Compare((byte)0x3F, (byte)0x30);
        // (0x3F & 0x0F) ^ 0x05 == 0x0A
        // (0x30 & 0x0F) ^ 0x05 == 0x05
        // → Mismatch expected

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeFalse();
        result.Mismatches.Count.ShouldBe(1);
    }

    [Fact]
    public void Compare_ShiftLeftAndRight_ShouldBehaveCorrectly_AsBothOperandsTransformed()
    {
        // Arrange
        var shiftLeft = CreateBuilder().ApplyBitwiseOperation(BitwiseOperation.ShiftLeft, 1);
        var shiftRight = CreateBuilder().ApplyBitwiseOperation(BitwiseOperation.ShiftRight, 1);

        // Act
        var leftResult = shiftLeft.Compare((byte)0b_0000_0010, (byte)0b_0000_0100);  // 2<<1 == 4  ; 4<<1 == 8
        var rightResult = shiftRight.Compare((byte)0b_0000_0100, (byte)0b_0000_0010); // 4>>1 == 2  ; 2>>1 == 1

        // Assert — transformed results are different, so mismatches expected
        _testOutputHelper.WriteLine(leftResult.ToString());
        _testOutputHelper.WriteLine(rightResult.ToString());
        leftResult.AllMatched.ShouldBeFalse();
        leftResult.Mismatches.Count.ShouldBeGreaterThan(0);
        rightResult.AllMatched.ShouldBeFalse();
        rightResult.Mismatches.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void Compare_ShiftLeftAndRight_WithBigShift_ShouldBehaveCorrectly_AsBothOperandsTransformed()
    {
        // Arrange
        var shiftLeft = CreateBuilder().ApplyBitwiseOperation(BitwiseOperation.ShiftLeft, 100);
        var shiftRight = CreateBuilder().ApplyBitwiseOperation(BitwiseOperation.ShiftRight, 100);

        // Act
        var leftResult = shiftLeft.Compare((byte)0b_1111_0010, (byte)0b_0000_0100);
        var rightResult = shiftRight.Compare((byte)0b_0000_0100, (byte)0b_1111_0100);

        // Assert — transformed results are different, so mismatches expected
        _testOutputHelper.WriteLine(leftResult.ToString());
        _testOutputHelper.WriteLine(rightResult.ToString());
        leftResult.AllMatched.ShouldBeFalse();
        leftResult.Mismatches.Count.ShouldBeGreaterThan(0);
        rightResult.AllMatched.ShouldBeFalse();
        rightResult.Mismatches.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void Compare_ShiftLeft_ShiftingBothEqualInputs_ShouldMatch()
    {
        // Arrange
        var shiftLeft = CreateBuilder().ApplyBitwiseOperation(BitwiseOperation.ShiftLeft, 1);

        // Act
        var result = shiftLeft.Compare((byte)0b_0000_0010, (byte)0b_0000_0010);  // both 2 -> both 4 after <<1

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
    }

    [Fact]
    public void Compare_ShiftRight_ShiftingBothEqualInputs_ShouldMatch()
    {
        // Arrange
        var shiftRight = CreateBuilder().ApplyBitwiseOperation(BitwiseOperation.ShiftRight, 1);

        // Act
        var result = shiftRight.Compare((byte)0b_0000_0100, (byte)0b_0000_0100);  // both 4 -> both 2 after >>1

        // Assert
        _testOutputHelper.WriteLine(result.ToString());
        result.AllMatched.ShouldBeTrue();
        result.Mismatches.ShouldBeEmpty();
    }
}
