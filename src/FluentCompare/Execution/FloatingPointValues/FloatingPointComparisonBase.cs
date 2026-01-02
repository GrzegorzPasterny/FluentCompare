#if NET7_0_OR_GREATER

using System.Numerics;

namespace FluentCompare.Execution.FloatingPointValues;

internal abstract class FloatingPointComparisonBase<T>
    : ComparisonBase<T>
    where T : struct, IFloatingPoint<T>
{
    protected FloatingPointComparisonBase(
        ComparisonConfiguration configuration,
        ComparisonResult? comparisonResult = null)
        : base(configuration, comparisonResult)
    {
    }

    internal bool CompareWithRounding(
        T valueA,
        T valueB,
        ComparisonType comparisonType,
        int precision)
    {
        int maxPrecision = FloatingPointHelpers.MaxRoundingDigits<T>();
        precision = Math.Min(precision, maxPrecision);

        T roundedA = T.Round(valueA, precision);
        T roundedB = T.Round(valueB, precision);

        return comparisonType switch
        {
            ComparisonType.EqualTo => roundedA == roundedB,
            ComparisonType.NotEqualTo => roundedA != roundedB,
            ComparisonType.GreaterThan => roundedA > roundedB,
            ComparisonType.LessThan => roundedA < roundedB,
            ComparisonType.GreaterThanOrEqualTo => roundedA >= roundedB,
            ComparisonType.LessThanOrEqualTo => roundedA <= roundedB,
            _ => throw new ArgumentOutOfRangeException(nameof(comparisonType))
        };
    }

    internal bool CompareWithEpsilon(
        T valueA,
        T valueB,
        ComparisonType comparisonType,
        double epsilon)
    {
        T epsilonT = FloatingPointHelpers.ToEpsilon<T>(epsilon);

        T diff = valueA - valueB;
        T absDiff = T.Abs(diff);

        return comparisonType switch
        {
            ComparisonType.EqualTo => absDiff < epsilonT,
            ComparisonType.NotEqualTo => absDiff >= epsilonT,
            ComparisonType.GreaterThan => diff > epsilonT,
            ComparisonType.LessThan => diff < -epsilonT,
            ComparisonType.GreaterThanOrEqualTo => diff >= -epsilonT,
            ComparisonType.LessThanOrEqualTo => diff <= epsilonT,
            _ => throw new ArgumentOutOfRangeException(nameof(comparisonType))
        };
    }
}

#endif
