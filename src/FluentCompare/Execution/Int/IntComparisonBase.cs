namespace FluentCompare.Execution.Int
{
    public abstract class IntComparisonBase
    {
        internal bool Compare(int valueA, int valueB, ComparisonType comparisonType)
        {
            switch (comparisonType)
            {
                case ComparisonType.EqualTo:
                    return valueA == valueB;
                case ComparisonType.NotEqualTo:
                    return valueA != valueB;
                case ComparisonType.GreaterThan:
                    return valueA > valueB;
                case ComparisonType.LessThan:
                    return valueA < valueB;
                case ComparisonType.GreaterThanOrEqualTo:
                    return valueA >= valueB;
                case ComparisonType.LessThanOrEqualTo:
                    return valueA <= valueB;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
