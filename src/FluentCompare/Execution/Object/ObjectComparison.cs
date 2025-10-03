using FluentCompare.Configuration;

public class ObjectComparison : IExecuteComparison<object>
{
    private readonly ComparisonConfiguration _comparisonConfiguration;

    internal ObjectComparison(
        ComparisonConfiguration comparisonConfiguration)
    {
        _comparisonConfiguration = comparisonConfiguration;
    }

    public ComparisonResult Compare(params object[] objects)
    {
        throw new NotImplementedException();
    }

    public ComparisonResult Compare(
        object obj1, object obj2,
        string obj1Name, string obj2Name)
    {
        throw new NotImplementedException();
    }
}
