
internal class ObjectArrayComparison : IExecuteComparison<object[]>
{
    private ComparisonConfiguration _configuration;
    private ObjectComparison _objectComparison;

    public ObjectArrayComparison(ComparisonConfiguration configuration)
    {
        _configuration = configuration;
        _objectComparison = new ObjectComparison(configuration);
    }

    public ComparisonResult Compare(params object[][] objects)
    {
        var result = new ComparisonResult();
        if (objects == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(object[])));
            return result;
        }

        if (objects.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectToCompare(objects.Length, typeof(object[])));
            return result;
        }

        var first = objects[0];
        for (int i = 1; i < objects.Length; i++)
        {
            var comparisonResult = Compare(first, objects[i], $"objects[0]", $"objects[{i}]");
            result.AddComparisonResult(comparisonResult);
        }

        return result;
    }

    public ComparisonResult Compare(object[] t1, object[] t2, string t1ExprName, string t2ExprName)
    {
        var result = new ComparisonResult();

        if (ReferenceEquals(t1, t2))
        {
            return result;
        }
        if (t1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t1ExprName, typeof(object[])));
            return result;
        }

        if (t2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t2ExprName, typeof(object[])));
            return result;
        }

        if (t1.Length != t2.Length)
        {
            result.AddError(ComparisonErrors.InputArrayLengthsDiffer(
                t1.Length, t2.Length, t1ExprName, t2ExprName, typeof(object[])));
            return result;
        }

        for (int i = 0; i < t1.Length; i++)
        {
            var obj1 = t1[i];
            var obj2 = t2[i];
            // Use the configured comparison mode for complex types
            switch (_configuration.ComplexTypesComparisonMode)
            {
                case ComplexTypesComparisonMode.ReferenceEquality:
                    CompareObjectsByReference(obj1, obj2, i, result);
                    break;
                case ComplexTypesComparisonMode.PropertyEquality:

                    if (obj1 == null && obj2 == null)
                    {
                        result.AddWarning(ComparisonErrors.Object.BothObjectsAreNull(obj1, obj2, i, i));
                    }

                    if (obj1 == null || obj2 == null)
                    {
                        if (obj1 != obj2)
                        {
                            result.AddMismatch(ComparisonMismatches.Object.MismatchDetectedByNull(
                                obj1, obj2, i, i));
                        }
                        break;
                    }

                    var type1 = obj1.GetType();
                    var type2 = obj2.GetType();

                    if (type1 != type2)
                    {
                        result.AddMismatch(ComparisonMismatches.Object.MismatchDetectedByType(
                            obj1, obj2, i, i, type1, type2));
                        break;
                    }

                    CompareObjectsPropertiesRecursively(obj1, obj2, result, type1);
                    break;
            }
        }
        return result;
    }

    private void CompareObjectsPropertiesRecursively(object t1, object t2, ComparisonResult result, Type type1, string? t1ExprName = null, string? t2ExprName = null)
    {
        var properties = type1.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach (var prop in properties)
        {
            var val1 = prop.GetValue(t1);
            var val2 = prop.GetValue(t2);
            ComparisonResult propResult;

            if (t1ExprName is null || t2ExprName is null)
            {
                propResult = _objectComparison.Compare(val1, val2, prop.Name, prop.Name);
            }
            else
            {
                propResult = _objectComparison.Compare(val1, val2, $"{t1ExprName}.{prop.Name}", $"{t2ExprName}.{prop.Name}");
            }

            if (!propResult.AllMatched)
            {
                result.AddMismatches(propResult.Mismatches);
                result.AddErrors(propResult.Errors);
            }
        }
    }

    private void CompareObjectsByReference(object obj1, object obj2, int i, ComparisonResult result)
    {
        if (!ReferenceEquals(obj1, obj2))
        {
            result.AddMismatch(ComparisonMismatches.Object.MismatchDetectedByReference(
                obj1, obj2, i));
        }
    }
}
