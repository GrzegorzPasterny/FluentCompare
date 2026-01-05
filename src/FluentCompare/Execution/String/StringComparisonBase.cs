internal abstract class StringComparisonBase : ComparisonBase<string>
{
    protected StringComparisonBase(
        ComparisonConfiguration configuration)
        : base(configuration) { }

    internal bool Compare(
        string stringA, string stringB,
        ComparisonType comparisonType,
        System.StringComparison stringComparison)
    {
        int comparisonResult = string.Compare(stringA, stringB, stringComparison);

        switch (comparisonType)
        {
            case ComparisonType.EqualTo:
                return comparisonResult == 0;
            case ComparisonType.NotEqualTo:
                return comparisonResult != 0;
            case ComparisonType.GreaterThan:
                return comparisonResult > 0;
            case ComparisonType.LessThan:
                return comparisonResult < 0;
            case ComparisonType.GreaterThanOrEqualTo:
                return comparisonResult >= 0;
            case ComparisonType.LessThanOrEqualTo:
                return comparisonResult <= 0;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
