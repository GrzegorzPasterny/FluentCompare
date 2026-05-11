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
                // TODO: Code not reached by unit tests - need to add tests for this case
                return comparisonResult != 0;
            case ComparisonType.GreaterThan:
                return comparisonResult > 0;
            case ComparisonType.LessThan:
                return comparisonResult < 0;
            case ComparisonType.GreaterThanOrEqualTo:
                // TODO: Code not reached by unit tests - need to add tests for this case
                return comparisonResult >= 0;
            case ComparisonType.LessThanOrEqualTo:
                // TODO: Code not reached by unit tests - need to add tests for this case
                return comparisonResult <= 0;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
