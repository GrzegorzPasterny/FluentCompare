internal abstract class DoubleComparisonBase : ComparisonBase<double>
{
    public DoubleComparisonBase(ComparisonConfiguration configuration, ComparisonResult? comparisonResult = null)
        : base(configuration, comparisonResult)
    {
    }

    internal bool CompareWithRounding(double valueA, double valueB, ComparisonType comparisonType, int precision)
    {
        double roundedA = Math.Round(valueA, precision);
        double roundedB = Math.Round(valueB, precision);

        switch (comparisonType)
        {
            case ComparisonType.EqualTo:
                return roundedA == roundedB;
            case ComparisonType.NotEqualTo:
                return roundedA != roundedB;
            case ComparisonType.GreaterThan:
                return roundedA > roundedB;
            case ComparisonType.LessThan:
                return roundedA < roundedB;
            case ComparisonType.GreaterThanOrEqualTo:
                return roundedA >= roundedB;
            case ComparisonType.LessThanOrEqualTo:
                return roundedA <= roundedB;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    internal bool CompareWithEpsilon(double valueA, double valueB, ComparisonType comparisonType, double epsilon)
    {
        double diff = valueA - valueB;
        switch (comparisonType)
        {
            case ComparisonType.EqualTo:
                return Math.Abs(diff) < epsilon;
            case ComparisonType.NotEqualTo:
                return Math.Abs(diff) >= epsilon;
            case ComparisonType.GreaterThan:
                return diff > epsilon;
            case ComparisonType.LessThan:
                return diff < -epsilon;
            case ComparisonType.GreaterThanOrEqualTo:
                return diff > -epsilon;
            case ComparisonType.LessThanOrEqualTo:
                return diff < epsilon;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
