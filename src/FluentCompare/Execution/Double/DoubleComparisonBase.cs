public class DoubleComparisonBase
{
    private bool Compare(double valueA, double valueB, ComparisonType comparisonType, int precision)
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
}
