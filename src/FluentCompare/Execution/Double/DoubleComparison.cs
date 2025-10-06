public class DoubleComparison : DoubleComparisonBase, IExecuteComparison<double>
{
    private ComparisonConfiguration _configuration;

    public DoubleComparison(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ComparisonResult Compare(params double[] objects)
    {
        var result = new ComparisonResult();

        if (objects == null || objects.Length < 2)
        {
            result.AddError(new ComparisonError("At least two double values are required for comparison."));
            return result;
        }

        var comparisonType = _configuration.ComparisonType;

        if (_configuration.DoubleConfiguration is null)
        {
            result.AddError(new ComparisonError("Double comparison configuration is missing"));
            return result;
        }

        var precision = _configuration.DoubleConfiguration.Precision;

        for (int i = 0; i < objects.Length - 1; i++)
        {
            bool matched = Compare(objects[i], objects[i + 1], comparisonType, precision);
            if (!matched)
            {
                result.AddMismatch(new ComparisonMismatch
                {
                    Message = $"Comparison failed between objects[{i}] ({objects[i]}) and objects[{i + 1}] ({objects[i + 1]}) with type {comparisonType} and precision {precision}."
                });
            }
        }

        return result;
    }

    public ComparisonResult Compare(double t1, double t2, string t1ExprName, string t2ExprName)
    {
        var result = new ComparisonResult();
        var comparisonType = _configuration.ComparisonType;

        if (_configuration.DoubleConfiguration is null)
        {
            result.AddError(new ComparisonError("Double comparison configuration is missing"));
            return result;
        }

        var precision = _configuration.DoubleConfiguration.Precision;

        bool matched = Compare(t1, t2, comparisonType, precision);
        if (!matched)
        {
            result.AddMismatch(new ComparisonMismatch
            {
                Message = $"Comparison failed between {t1ExprName} ({t1}) and {t2ExprName} ({t2}) with type {comparisonType} and precision {precision}."
            });
        }

        return result;
    }
}
