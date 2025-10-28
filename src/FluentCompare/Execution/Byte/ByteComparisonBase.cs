internal abstract class ByteComparisonBase : ComparisonBase<byte>
{
    protected ByteComparisonBase(
        ComparisonConfiguration configuration, ComparisonResult? comparisonResult = null)
        : base(configuration, comparisonResult)
    {
    }

    internal bool Compare(byte valueA, byte valueB, ComparisonType comparisonType)
    {
        int cmp = valueA.CompareTo(valueB);
        return comparisonType switch
        {
            ComparisonType.EqualTo => cmp == 0,
            ComparisonType.NotEqualTo => cmp != 0,
            ComparisonType.GreaterThan => cmp > 0,
            ComparisonType.LessThan => cmp < 0,
            ComparisonType.GreaterThanOrEqualTo => cmp >= 0,
            ComparisonType.LessThanOrEqualTo => cmp <= 0,
            _ => throw new ArgumentOutOfRangeException(nameof(comparisonType), comparisonType, null)
        };
    }

    internal byte ApplyBitwiseOperations(byte value, int valueIndex, List<BitwiseOperationModel> bitwiseOperationModels)
    {
        byte result = value;

        foreach (var bitwiseOperationModel in bitwiseOperationModels)
        {
            if (bitwiseOperationModel.ComparisonObjectIndexesToExclude.Contains(valueIndex))
            {
                continue;
            }

            switch (bitwiseOperationModel.Operation)
            {
                case BitwiseOperation.And:
                    result &= bitwiseOperationModel.Value;
                    break;
                case BitwiseOperation.Or:
                    result |= bitwiseOperationModel.Value;
                    break;
                case BitwiseOperation.Xor:
                    result ^= bitwiseOperationModel.Value;
                    break;
                case BitwiseOperation.Not:
                    result = (byte)~result;
                    break;
                case BitwiseOperation.ShiftLeft:
                    result = (byte)(result << bitwiseOperationModel.Value);
                    break;
                case BitwiseOperation.ShiftRight:
                    result = (byte)(result >> bitwiseOperationModel.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return result;
    }

    internal byte[] ApplyBitwiseOperations(byte[] value, int valueIndex, List<BitwiseOperationModel> bitwiseOperationModels)
    {
        byte[] result = value;

        foreach (var bitwiseOperationModel in bitwiseOperationModels)
        {
            if (bitwiseOperationModel.ComparisonObjectIndexesToExclude.Contains(valueIndex))
            {
                continue;
            }

            for (int i = 0; i < value.Length; i++)
            {
                switch (bitwiseOperationModel.Operation)
                {
                    case BitwiseOperation.And:
                        result[i] &= bitwiseOperationModel.Value;
                        break;
                    case BitwiseOperation.Or:
                        result[i] |= bitwiseOperationModel.Value;
                        break;
                    case BitwiseOperation.Xor:
                        result[i] ^= bitwiseOperationModel.Value;
                        break;
                    case BitwiseOperation.Not:
                        result[i] = (byte)~result[i];
                        break;
                    case BitwiseOperation.ShiftLeft:
                        result[i] = (byte)(result[i] << bitwiseOperationModel.Value);
                        break;
                    case BitwiseOperation.ShiftRight:
                        result[i] = (byte)(result[i] >> bitwiseOperationModel.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        return result;
    }
}
