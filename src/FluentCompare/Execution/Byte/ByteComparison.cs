

internal class ByteComparison : ByteComparisonBase, IExecuteComparison<byte>, IExecuteNullableComparison<byte>
{
    public ByteComparison(
        ComparisonConfiguration configuration)
        : base(configuration)
    { }

    public override ComparisonResult Compare(byte[] bytes, ComparisonResult result)
    {
        if (bytes == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(byte)));
            return result;
        }

        if (bytes.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(bytes.Length, typeof(byte)));
            return result;
        }

        var first = bytes[0];
        byte transformedFirst = ApplyBitwiseOperations(first, 0, _comparisonConfiguration.ByteConfiguration.BitwiseOperations);

        for (byte i = 1; i < bytes.Length; i++)
        {
            byte transformedCurrent = ApplyBitwiseOperations(bytes[i], i, _comparisonConfiguration.ByteConfiguration.BitwiseOperations);
            if (!Compare(transformedFirst, transformedCurrent, _comparisonConfiguration.ComparisonType))
            {
                if (_comparisonConfiguration.ByteConfiguration.BitwiseOperations.Count > 0)
                {
                    result.AddMismatch(
                        ComparisonMismatches.Byte.MismatchDetected(
                            first, bytes[i], transformedFirst, transformedCurrent, i, _comparisonConfiguration.ComparisonType, _toStringFunc));
                }
                else
                {
                    result.AddMismatch(
                        ComparisonMismatches<byte>.MismatchDetected(
                            first, bytes[i], i, _comparisonConfiguration.ComparisonType, _toStringFunc));
                }
            }
        }

        return result;
    }

    public ComparisonResult Compare(byte? b1, byte? b2, string t1ExprName, string t2ExprName, ComparisonResult result)
    {
        if (b1 is null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t1ExprName, typeof(byte?)));
            return result;
        }

        if (b2 is null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t2ExprName, typeof(byte?)));
            return result;
        }

        return Compare(b1.Value, b2.Value, t1ExprName, t2ExprName, result);
    }

    public override ComparisonResult Compare(byte b1, byte b2, string t1ExprName, string t2ExprName, ComparisonResult result)
    {
        byte b1Transformed = ApplyBitwiseOperations(b1, 0, _comparisonConfiguration.ByteConfiguration.BitwiseOperations);
        byte b2Transformed = ApplyBitwiseOperations(b2, 1, _comparisonConfiguration.ByteConfiguration.BitwiseOperations);

        if (!Compare(b1Transformed, b2Transformed, _comparisonConfiguration.ComparisonType))
        {
            if (_comparisonConfiguration.ByteConfiguration.BitwiseOperations.Count > 0)
            {
                result.AddMismatch(ComparisonMismatches.Byte.MismatchDetected(b1, b2, b1Transformed, b2Transformed, t1ExprName, t2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
            else
            {
                result.AddMismatch(ComparisonMismatches<byte>.MismatchDetected(b1, b2, t1ExprName, t2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
        }

        return result;
    }

    public override ComparisonResult Compare(byte[][] bytes, ComparisonResult result)
    {
        if (bytes == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(byte[])));
            return result;
        }

        if (bytes.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(bytes.Length, typeof(byte[])));
            return result;
        }

        // All arrays are compared against the first one
        var first = bytes[0];

        if (first == null)
        {
            result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(0, typeof(byte[])));
            return result;
        }

        var firstTransformed = ApplyBitwiseOperations(first, 0, _comparisonConfiguration.ByteConfiguration.BitwiseOperations);

        for (byte i = 1; i < bytes.Length; i++)
        {
            var current = bytes[i];

            if (current == null)
            {
                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(i, typeof(byte[])));
                return result;
            }

            if (first.Length != current.Length)
            {
                result.AddError(ComparisonErrors.InputArrayLengthsDiffer(first.Length, current.Length, 0, i, typeof(byte[])));
                return result;
            }

            var currentTransformed = ApplyBitwiseOperations(current, i, _comparisonConfiguration.ByteConfiguration.BitwiseOperations);

            for (byte j = 0; j < first.Length; j++)
            {
                if (!Compare(firstTransformed[j], currentTransformed[j], _comparisonConfiguration.ComparisonType))
                {
                    if (_comparisonConfiguration.ByteConfiguration.BitwiseOperations.Count > 0)
                    {
                        result.AddMismatch(ComparisonMismatches.Byte.MismatchDetected(
                            first[j], current[j], firstTransformed[j], currentTransformed[j], j, 0, i, _comparisonConfiguration.ComparisonType, _toStringFunc));
                    }
                    else
                    {
                        result.AddMismatch(ComparisonMismatches<byte>.MismatchDetected(
                            first[j], current[j], j, 0, i, _comparisonConfiguration.ComparisonType, _toStringFunc));
                    }
                }
            }
        }

        return result;
    }

    public override ComparisonResult Compare(byte[] byteArr1, byte[] byteArr2, string byteArr1ExprName, string byteArr2ExprName, ComparisonResult result)
    {
        if (byteArr1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(byteArr1ExprName, typeof(byte[])));
            return result;
        }

        if (byteArr2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(byteArr2ExprName, typeof(byte[])));
            return result;
        }

        if (byteArr1.Length != byteArr2.Length)
        {
            result.AddError(ComparisonErrors.InputArrayLengthsDiffer(
                byteArr1.Length, byteArr2.Length, byteArr1ExprName, byteArr2ExprName, typeof(byte[])));
            return result;
        }

        byte[] byteArr1Transformed = ApplyBitwiseOperations(byteArr1, 0, _comparisonConfiguration.ByteConfiguration.BitwiseOperations);
        byte[] byteArr2Transformed = ApplyBitwiseOperations(byteArr2, 1, _comparisonConfiguration.ByteConfiguration.BitwiseOperations);

        for (byte i = 0; i < byteArr1.Length; i++)
        {
            if (!Compare(byteArr1Transformed[i], byteArr2Transformed[i], _comparisonConfiguration.ComparisonType))
            {
                if (_comparisonConfiguration.ByteConfiguration.BitwiseOperations.Count > 0)
                {
                    result.AddMismatch(ComparisonMismatches.Byte.MismatchDetected(
                        byteArr1[i], byteArr2[i], byteArr1Transformed[i], byteArr2Transformed[i], i, byteArr1ExprName, byteArr2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
                }
                else
                {
                    result.AddMismatch(ComparisonMismatches<byte>.MismatchDetected(
                        byteArr1[i], byteArr2[i], i, byteArr1ExprName, byteArr2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
                }
            }
        }

        return result;
    }

    public ComparisonResult Compare(byte?[]? byteArr1, byte?[]? byteArr2, string byteArr1ExprName, string byteArr2ExprName, ComparisonResult result)
    {
        if (byteArr1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(byteArr1ExprName, typeof(byte?[])));
            return result;
        }

        if (byteArr2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(byteArr2ExprName, typeof(byte?[])));
            return result;
        }

        if (byteArr1.Length != byteArr2.Length)
        {
            result.AddError(ComparisonErrors.InputArrayLengthsDiffer(
                byteArr1.Length, byteArr2.Length, byteArr1ExprName, byteArr2ExprName, typeof(byte?[])));
            return result;
        }

        for (var i = 0; i < byteArr1.Length; i++)
        {
            if (byteArr1[i] is null)
            {
                result.AddError(ComparisonErrors.NullPassedAsArgument($"{byteArr1ExprName}[{i}]", typeof(byte?)));
                return result;
            }

            if (byteArr2[i] is null)
            {
                result.AddError(ComparisonErrors.NullPassedAsArgument($"{byteArr2ExprName}[{i}]", typeof(byte?)));
                return result;
            }
        }

        var normalizedByteArr1 = byteArr1.Select(value => value!.Value).ToArray();
        var normalizedByteArr2 = byteArr2.Select(value => value!.Value).ToArray();

        return Compare(normalizedByteArr1, normalizedByteArr2, byteArr1ExprName, byteArr2ExprName, result);
    }

    public ComparisonResult Compare(byte?[][]? bytes, ComparisonResult result)
    {
        if (bytes == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(byte?[])));
            return result;
        }

        if (bytes.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(bytes.Length, typeof(byte?[])));
            return result;
        }

        var normalized = new byte[bytes.Length][];

        for (var i = 0; i < bytes.Length; i++)
        {
            var current = bytes[i];
            if (current == null)
            {
                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(i, typeof(byte?[])));
                return result;
            }

            normalized[i] = new byte[current.Length];
            for (var j = 0; j < current.Length; j++)
            {
                if (current[j] is null)
                {
                    result.AddError(ComparisonErrors.NullPassedAsArgument($"Array[{i}][{j}]", typeof(byte?)));
                    return result;
                }

                normalized[i][j] = current[j]!.Value;
            }
        }

        return Compare(normalized, result);
    }
}
