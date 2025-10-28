internal class ByteComparison : ByteComparisonBase, IExecuteComparison<byte>
{
    public ByteComparison(
        ComparisonConfiguration configuration, ComparisonResult? comparisonResult = null)
        : base(configuration, comparisonResult)
    { }

    public override ComparisonResult Compare(params byte[] bytes)
    {
        var result = new ComparisonResult();

        if (bytes == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(byte)));
            return result;
        }

        if (bytes.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectToCompare(bytes.Length, typeof(byte)));
            return result;
        }

        var first = bytes[0];
        for (byte i = 1; i <= bytes.Length; i++)
        {
            if (!Compare(first, bytes[i], _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches<byte>.MismatchDetected(first, bytes[i], i, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
        }

        return result;
    }

    public override ComparisonResult Compare(byte i1, byte i2, string t1ExprName, string t2ExprName)
    {
        var result = new ComparisonResult();

        if (!Compare(i1, i2, _comparisonConfiguration.ComparisonType))
        {
            result.AddMismatch(ComparisonMismatches<byte>.MismatchDetected(i1, i2, t1ExprName, t2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
        }

        return result;
    }

    public override ComparisonResult Compare(params byte[][] bytes)
    {
        var result = new ComparisonResult();

        if (bytes == null || bytes.Length < 2)
            return result;

        // All arrays are compared against the first one
        var first = bytes[0];

        for (byte i = 1; i < bytes.Length; i++)
        {
            var current = bytes[i];

            if (first == null)
            {
                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(0, typeof(byte[])));
                return result;
            }

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

            for (byte j = 0; j < first.Length; j++)
            {
                if (!Compare(first[j], current[j], _comparisonConfiguration.ComparisonType))
                {
                    result.AddMismatch(ComparisonMismatches<byte>.MismatchDetected(
                        first[j], current[j], j, 0, i, _comparisonConfiguration.ComparisonType, _toStringFunc));
                }
            }
        }

        return result;
    }

    public override ComparisonResult Compare(byte[] byteArr1, byte[] byteArr2, string byteArr1ExprName, string byteArr2ExprName)
    {
        var result = new ComparisonResult();

        if (byteArr1.Length != byteArr2.Length)
        {
            result.AddError(ComparisonErrors.InputArrayLengthsDiffer(
                byteArr1.Length, byteArr2.Length, byteArr1ExprName, byteArr2ExprName, typeof(byte[])));
            return result;
        }

        for (byte i = 0; i < byteArr1.Length; i++)
        {
            if (!Compare(byteArr1[i], byteArr2[i], _comparisonConfiguration.ComparisonType))
            {
                result.AddMismatch(ComparisonMismatches<byte>.MismatchDetected(
                    byteArr1[i], byteArr2[i], i, byteArr1ExprName, byteArr2ExprName, _comparisonConfiguration.ComparisonType, _toStringFunc));
            }
        }

        return result;
    }
}
