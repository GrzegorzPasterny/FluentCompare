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
        var result = new ComparisonResult();

        if (objects == null || objects.Length < 2)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(object[])));
            return result;
        }

        object firstObj = objects[0];

        for (int i = 1; i < objects.Length; i++)
        {
            object currentObj = objects[i];

            // Use the configured comparison mode for complex types
            switch (_comparisonConfiguration.ComplexTypesComparisonMode)
            {
                case ComplexTypesComparisonMode.ReferenceEquality:
                    if (!ReferenceEquals(firstObj, currentObj))
                    {
                        result.AddMismatch(ComparisonMismatches.Object.MismatchDetectedByReference(
                            firstObj, currentObj, 0, i));
                    }
                    break;

                case ComplexTypesComparisonMode.PropertyEquality:
                    if (firstObj == null && currentObj == null)
                    {
                        result.AddWarning(ComparisonErrors.Object.BothObjectsAreNull(firstObj, currentObj, 0, i));
                    }

                    if (firstObj == null || currentObj == null)
                    {
                        if (firstObj != currentObj)
                        {
                            result.AddMismatch(ComparisonMismatches.Object.MismatchDetectedByNull(
                                firstObj, currentObj, 0, i));
                        }
                        break;
                    }

                    var type1 = firstObj.GetType();
                    var type2 = currentObj.GetType();

                    if (type1 != type2)
                    {
                        result.AddMismatch(ComparisonMismatches.Object.MismatchDetectedByType(
                            firstObj, currentObj, 0, i, type1, type2));
                        break;
                    }

                    // For complex types, compare properties recursively
                    var properties = type1.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    foreach (var prop in properties)
                    {
                        var val1 = prop.GetValue(firstObj);
                        var val2 = prop.GetValue(currentObj);

                        // Recursively compare property values
                        var propResult = Compare(val1, val2, prop.Name, prop.Name);
                        if (!propResult.AllMatched)
                        {
                            result.AddMismatches(propResult.Mismatches);
                            result.AddErrors(propResult.Errors);
                        }
                    }
                    break;
            }
        }

        return result;
    }

    public ComparisonResult Compare(object t1, object t2, string t1ExprName, string t2ExprName)
    {
        var result = new ComparisonResult();
        if (t1 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t1ExprName, typeof(object)));
            return result;
        }
        if (t2 == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(t2ExprName, typeof(object)));
            return result;
        }

        // Use the configured comparison mode for complex types
        switch (_comparisonConfiguration.ComplexTypesComparisonMode)
        {
            case ComplexTypesComparisonMode.ReferenceEquality:
                if (!ReferenceEquals(t1, t2))
                {
                    result.AddMismatch(ComparisonMismatches.Object.MismatchDetectedByReference(
                        t1, t2, t1ExprName, t2ExprName));
                }
                break;
            case ComplexTypesComparisonMode.PropertyEquality:
                var type1 = t1.GetType();
                var type2 = t2.GetType();
                if (type1 != type2)
                {
                    result.AddMismatch(ComparisonMismatches.Object.MismatchDetectedByType(
                        t1, t2, t1ExprName, t2ExprName, type1, type2));
                    break;
                }

                // Check for primitive, enum, or string types
                if (type1.IsPrimitive)
                {
                    var results = new ComparisonBuilder(_comparisonConfiguration)
                        .Compare(t1, t2, t1ExprName, t2ExprName);

                    result.AddComparisonResult(results);
                    break;
                }

                // For complex types, compare properties recursively
                var properties = type1.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                foreach (var prop in properties)
                {
                    var val1 = prop.GetValue(t1);
                    var val2 = prop.GetValue(t2);
                    // Recursively compare property values
                    var propResult = Compare(val1, val2, $"{t1ExprName}.{prop.Name}", $"{t2ExprName}.{prop.Name}");
                    if (!propResult.AllMatched)
                    {
                        result.AddMismatches(propResult.Mismatches);
                        result.AddErrors(propResult.Errors);
                    }
                }
                break;
        }
        return result;
    }
}
