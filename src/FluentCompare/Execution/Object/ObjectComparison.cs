internal class ObjectComparison : ObjectComparisonBase
{
    private readonly int _currentDepth;
    private int _localDepth = 0;

    internal ObjectComparison(
        ComparisonConfiguration comparisonConfiguration, int currentDepth, ComparisonResult? comparisonResult = null)
        : base(comparisonConfiguration, comparisonResult)
    {
        _currentDepth = currentDepth;
    }

    public override ComparisonResult Compare(params object[] objects)
    {
        var result = new ComparisonResult();

        if (_currentDepth + _localDepth >= _comparisonConfiguration.MaximumComparisonDepth)
        {
            result.AddWarning(ComparisonErrors.DepthLimitReached(_currentDepth));
            return result;
        }

        _localDepth++;

        if (objects == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(object[])));
            return result;
        }

        if (objects.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(objects.Length, typeof(object[])));
            return result;
        }

        object? firstObj = objects[0];

        for (int i = 1; i < objects.Length; i++)
        {
            object? currentObj = objects[i];

            // Use the configured comparison mode for complex types
            switch (_comparisonConfiguration.ComplexTypesComparisonMode)
            {
                case ComplexTypesComparisonMode.ReferenceEquality:
                    CompareObjectsByReference(firstObj, currentObj, 0, i, result);
                    break;

                case ComplexTypesComparisonMode.PropertyEquality:
                    if (ReferenceEquals(firstObj, currentObj))
                    {
                        return result;
                    }
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
                    CompareObjectsPropertiesRecursively(firstObj, currentObj, result, type1);
                    break;
            }
        }

        return result;
    }

    public override ComparisonResult Compare(object? t1, object? t2, string t1ExprName, string t2ExprName)
    {
        var result = new ComparisonResult();

        if (_currentDepth + _localDepth >= _comparisonConfiguration.MaximumComparisonDepth)
        {
            result.AddWarning(ComparisonErrors.DepthLimitReached(_currentDepth, t1ExprName, t2ExprName));
            return result;
        }

        _localDepth++;

        if (ReferenceEquals(t1, t2))
        {
            return result;
        }
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

        switch (_comparisonConfiguration.ComplexTypesComparisonMode)
        {
            case ComplexTypesComparisonMode.ReferenceEquality:
                CompareObjectsByReference(t1, t2, t1ExprName, t2ExprName, result);
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

                if (IsPrimitiveEnumOrString(type1))
                {
                    CompareObjectsWhenPrimitive(t1, t2, t1ExprName, t2ExprName, result, type1);
                    break;
                }

                CompareObjectsPropertiesRecursively(t1, t2, result, type1, t1ExprName, t2ExprName);
                break;
        }
        return result;
    }

    public override ComparisonResult Compare(params object[][] objects)
    {
        var result = new ComparisonResult();
        if (objects == null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(object[])));
            return result;
        }

        if (objects.Length < 2)
        {
            result.AddError(ComparisonErrors.NotEnoughObjectsToCompare(objects.Length, typeof(object[])));
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

    public override ComparisonResult Compare(object[] t1, object[] t2, string t1ExprName, string t2ExprName)
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
            switch (_comparisonConfiguration.ComplexTypesComparisonMode)
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

    private void CompareObjectsPropertiesRecursively(
        object t1, object t2,
        ComparisonResult result, Type type1,
        string? t1ExprName = null, string? t2ExprName = null)
    {
        var properties = type1.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach (var prop in properties)
        {
            var val1 = prop.GetValue(t1);
            var val2 = prop.GetValue(t2);
            ComparisonResult propResult;

            if (t1ExprName is null || t2ExprName is null)
            {
                propResult = Compare(val1, val2, prop.Name, prop.Name);
            }
            else
            {
                propResult = Compare(
                    val1, val2,
                    $"{t1ExprName}.{prop.Name}", $"{t2ExprName}.{prop.Name}");
            }

            if (!propResult.AllMatched)
            {
                result.AddComparisonResult(propResult);
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

    private void CompareObjectsWhenPrimitive(object t1, object t2, string t1ExprName, string t2ExprName, ComparisonResult result, Type type1)
    {
        var compareMethod = typeof(ComparisonBuilder)
                                .GetMethods()
                                .Where(m => m.Name == nameof(Compare))
                                .Where(m => m.IsGenericMethodDefinition)
                                .First(m =>
                                {
                                    var p = m.GetParameters();
                                    return p.Length >= 2 && p[0].ParameterType.IsGenericParameter && p[1].ParameterType.IsGenericParameter;
                                });

        // Dynamically call the generic Compare<T> with the real type
        var method = compareMethod.MakeGenericMethod(type1);

        var subResult = (ComparisonResult)method.Invoke(
            new ComparisonBuilder(_currentDepth + 1),
            new object?[] { t1, t2, t1ExprName, t2ExprName }
        )!;

        result.AddComparisonResult(subResult);
    }

    private static bool IsPrimitiveEnumOrString(Type type1) => type1.IsPrimitive || type1.IsEnum || type1 == typeof(string);

    private static void CompareObjectsByReference(object t1, object t2, string t1ExprName, string t2ExprName, ComparisonResult result)
    {
        if (!ReferenceEquals(t1, t2))
        {
            result.AddMismatch(ComparisonMismatches.Object.MismatchDetectedByReference(
                t1, t2, t1ExprName, t2ExprName));
        }
    }

    private void CompareObjectsByReference(object? firstObj, object? currentObj, int v, int i, ComparisonResult result)
    {
        if (!ReferenceEquals(firstObj, currentObj))
        {
            result.AddMismatch(ComparisonMismatches.Object.MismatchDetectedByReference(
                firstObj, currentObj, 0, i));
        }
    }
}
