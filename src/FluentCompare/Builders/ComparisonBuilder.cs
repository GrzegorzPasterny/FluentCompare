using System.Runtime.CompilerServices;

// TODO: Add async interface to allow comparison cancellation (in case of big objects)
/// <summary>
/// Builder object to configure and perform comparisons.
/// </summary>
public partial class ComparisonBuilder : IComparisonBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComparisonBuilder"/> class with default settings.
    /// </summary>
    internal ComparisonBuilder() { }

    /// <summary>
    /// Configuration object for the comparison.
    /// </summary>
    public ComparisonConfiguration Configuration { get; private set; } = new();


    /// <summary>
    /// Starting point to begin configuring and performing comparisons.
    /// </summary>
    /// <returns>A new <see cref="ComparisonBuilder"/> instance.</returns>
    public static ComparisonBuilder Create() => new ComparisonBuilder();

    /// <summary>
    /// Executes the comparison between <paramref name="t1"/> and <paramref name="t2"/> using provided <paramref name="comparisonConfiguration"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    /// <param name="comparisonConfiguration">Configuration object. The default configuration will be used if this parameter is not provided.</param>
    /// <returns></returns>
    /// <remarks>
    /// Use <see cref="Create"/> method to enable the Fluent API experience if needed.
    /// </remarks>
    public static ComparisonResult Compare<T>(T t1, T t2, ComparisonConfiguration? comparisonConfiguration = null)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Builds and executes the comparison for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="t">The objects to compare.</param>
    /// <returns>A <see cref="ComparisonResult"/> representing the outcome of the comparison.</returns>
    /// <exception cref="NotImplementedException">Thrown if the type is not supported.</exception>
    public ComparisonResult Compare<T>(params T[]? t)
    {
        var result = new ComparisonResult();
        if (t is null)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(T)));
            return result;
        }
        if (typeof(T).Name == typeof(Nullable<>).Name)
        {
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(T)));
            return result;
        }

        if (typeof(T) == typeof(string))
            return new StringComparison(Configuration)
                .Compare((string[])(object)t, result);

        if (typeof(T) == typeof(string[]))
            return new StringComparison(Configuration)
                .Compare((string[][])(object)t, result);

        if (typeof(T) == typeof(bool))
            return new BoolComparison(Configuration)
                .Compare((bool[])(object)t, result);

        if (typeof(T) == typeof(bool[]))
            return new BoolComparison(Configuration)
                .Compare((bool[][])(object)t, result);

        if (typeof(T) == typeof(byte))
            return new ByteComparison(Configuration)
                .Compare((byte[])(object)t, result);

        if (typeof(T) == typeof(byte[]))
            return new ByteComparison(Configuration)
                .Compare((byte[][])(object)t, result);

        if (typeof(T) == typeof(byte?[]))
            return new ByteComparison(Configuration)
                .Compare((byte?[][])(object)t, result);

        if (typeof(T) == typeof(short))
            return new NumericComparison<short>(Configuration)
                .Compare((short[])(object)t, result);

        if (typeof(T) == typeof(short[]))
            return new NumericComparison<short>(Configuration)
                .Compare((short[][])(object)t, result);

        if (typeof(T) == typeof(int))
            return new NumericComparison<int>(Configuration)
                .Compare((int[])(object)t, result);

        if (typeof(T) == typeof(int[]))
            return new NumericComparison<int>(Configuration)
                .Compare((int[][])(object)t, result);

        if (typeof(T) == typeof(long))
            return new NumericComparison<long>(Configuration)
                .Compare((long[])(object)t, result);

        if (typeof(T) == typeof(long[]))
            return new NumericComparison<long>(Configuration)
                .Compare((long[][])(object)t, result);

        if (typeof(T) == typeof(float))
            return new FloatingPointComparison<float>(Configuration)
                .Compare((float[])(object)t, result);

        if (typeof(T) == typeof(float[]))
            return new FloatingPointComparison<float>(Configuration)
                .Compare((float[][])(object)t, result);

        if (typeof(T) == typeof(double))
            return new FloatingPointComparison<double>(Configuration)
                .Compare((double[])(object)t, result);

        if (typeof(T) == typeof(double[]))
            return new FloatingPointComparison<double>(Configuration)
                .Compare((double[][])(object)t, result);

        if (typeof(T) == typeof(object[]))
            return new ObjectComparison(Configuration)
                .Compare((object[][])(object)t, result);

        if (IsPrimitiveOrEnum(typeof(T)))
            throw new NotImplementedException(typeof(T).Name);

        return new ObjectComparison(Configuration)
            .Compare((object[])(object)t, result);
    }

    /// <summary>
    /// Builds and executes the comparison between <paramref name="t1"/> and <paramref name="t2"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="t1">First object for comparison.</param>
    /// <param name="t2">Second object for comparison.</param>
    /// <param name="t1Expr">Caller argument expression for t1.</param>
    /// <param name="t2Expr">Caller argument expression for t2.</param>
    /// <returns>A <see cref="ComparisonResult"/> representing the outcome of the comparison.</returns>
    /// <exception cref="NotImplementedException">Thrown if the type is not supported.</exception>
    public ComparisonResult Compare<T>(T t1, T t2,
        [CallerArgumentExpression(nameof(t1))] string? t1Expr = null,
        [CallerArgumentExpression(nameof(t2))] string? t2Expr = null)
    {
        var result = new ComparisonResult();


        HandleNullability(t1, t2, t1Expr, t2Expr, result);

        // Check basic nullability results first
        if (result.WasSuccessful == false)
        {
            return result;
        }
        if (result.AllMatched == false &&
            result.Mismatches.First().Code == ComparisonMismatches.NullPassedAsArgumentCode)
        {
            return result;
        }
        if (result.WarningCount > 0 && result.Warnings.Any(warning => warning.Code == ComparisonErrors.BothObjectsAreNullCode))
        {
            return result;
        }

        if (typeof(T) == typeof(string))
        {
            string s1 = Unsafe.As<T, string>(ref t1);
            string s2 = Unsafe.As<T, string>(ref t2);
            string t1ExprName = t1Expr ?? "StringOne";
            string t2ExprName = t2Expr ?? "StringTwo";

            return new StringComparison(Configuration)
                .Compare(s1, s2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(string[]))
        {
            string[] sArr1 = Unsafe.As<T, string[]>(ref t1);
            string[] sArr2 = Unsafe.As<T, string[]>(ref t2);
            string t1ExprName = t1Expr ?? "StringArrayOne";
            string t2ExprName = t2Expr ?? "StringArrayTwo";

            return new StringComparison(Configuration)
                .Compare(sArr1, sArr2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(bool))
        {
            bool s1 = Unsafe.As<T, bool>(ref t1);
            bool s2 = Unsafe.As<T, bool>(ref t2);
            string t1ExprName = t1Expr ?? "BoolOne";
            string t2ExprName = t2Expr ?? "BoolTwo";

            return new BoolComparison(Configuration)
                .Compare(s1, s2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(bool[]))
        {
            bool[] s1 = Unsafe.As<T, bool[]>(ref t1);
            bool[] s2 = Unsafe.As<T, bool[]>(ref t2);
            string t1ExprName = t1Expr ?? "BoolArrayOne";
            string t2ExprName = t2Expr ?? "BoolArrayTwo";

            return new BoolComparison(Configuration)
                .Compare(s1, s2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(byte))
        {
            byte s1 = Unsafe.As<T, byte>(ref t1);
            byte s2 = Unsafe.As<T, byte>(ref t2);
            string t1ExprName = t1Expr ?? "ByteOne";
            string t2ExprName = t2Expr ?? "ByteTwo";

            return new ByteComparison(Configuration)
                .Compare(s1, s2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(byte[]))
        {
            byte[] s1 = Unsafe.As<T, byte[]>(ref t1);
            byte[] s2 = Unsafe.As<T, byte[]>(ref t2);
            string t1ExprName = t1Expr ?? "ByteArrayOne";
            string t2ExprName = t2Expr ?? "ByteArrayTwo";

            return new ByteComparison(Configuration)
                .Compare(s1, s2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(byte?[]))
        {
            string t1ExprName = t1Expr ?? "NullableByteArrayOne";
            string t2ExprName = t2Expr ?? "NullableByteArrayTwo";

            return new ByteComparison(Configuration)
                .Compare((byte?[]?)(object?)t1, (byte?[]?)(object?)t2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(short))
        {
            short o1 = Unsafe.As<T, short>(ref t1);
            short o2 = Unsafe.As<T, short>(ref t2);
            string t1ExprName = t1Expr ?? "ShortOne";
            string t2ExprName = t2Expr ?? "ShortTwo";

            return new NumericComparison<short>(Configuration)
                .Compare(o1, o2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(short[]))
        {
            short[] oArr1 = Unsafe.As<T, short[]>(ref t1);
            short[] oArr2 = Unsafe.As<T, short[]>(ref t2);
            string t1ExprName = t1Expr ?? "ShortArrayOne";
            string t2ExprName = t2Expr ?? "ShortArrayTwo";

            return new NumericComparison<short>(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(int))
        {
            int o1 = Unsafe.As<T, int>(ref t1);
            int o2 = Unsafe.As<T, int>(ref t2);
            string t1ExprName = t1Expr ?? "IntOne";
            string t2ExprName = t2Expr ?? "IntTwo";

            return new NumericComparison<int>(Configuration)
                .Compare(o1, o2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(int[]))
        {
            int[] oArr1 = Unsafe.As<T, int[]>(ref t1);
            int[] oArr2 = Unsafe.As<T, int[]>(ref t2);
            string t1ExprName = t1Expr ?? "IntArrayOne";
            string t2ExprName = t2Expr ?? "IntArrayTwo";

            return new NumericComparison<int>(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(long))
        {
            long o1 = Unsafe.As<T, long>(ref t1);
            long o2 = Unsafe.As<T, long>(ref t2);
            string t1ExprName = t1Expr ?? "LongOne";
            string t2ExprName = t2Expr ?? "LongTwo";

            return new NumericComparison<long>(Configuration)
                .Compare(o1, o2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(long[]))
        {
            long[] oArr1 = Unsafe.As<T, long[]>(ref t1);
            long[] oArr2 = Unsafe.As<T, long[]>(ref t2);
            string t1ExprName = t1Expr ?? "LongArrayOne";
            string t2ExprName = t2Expr ?? "LongArrayTwo";

            return new NumericComparison<long>(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(float))
        {
            var o1 = Unsafe.As<T, float>(ref t1);
            var o2 = Unsafe.As<T, float>(ref t2);
            string t1ExprName = t1Expr ?? "FloatOne";
            string t2ExprName = t2Expr ?? "FloatTwo";

            return new FloatingPointComparison<float>(Configuration)
                .Compare(o1, o2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(float[]))
        {
            var oArr1 = Unsafe.As<T, float[]>(ref t1);
            var oArr2 = Unsafe.As<T, float[]>(ref t2);
            string t1ExprName = t1Expr ?? "FloatArrayOne";
            string t2ExprName = t2Expr ?? "FloatArrayTwo";

            return new FloatingPointComparison<float>(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(double))
        {
            var o1 = Unsafe.As<T, double>(ref t1);
            var o2 = Unsafe.As<T, double>(ref t2);
            string t1ExprName = t1Expr ?? "DoubleOne";
            string t2ExprName = t2Expr ?? "DoubleTwo";

            return new FloatingPointComparison<double>(Configuration)
                .Compare(o1, o2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(double[]))
        {
            var oArr1 = Unsafe.As<T, double[]>(ref t1);
            var oArr2 = Unsafe.As<T, double[]>(ref t2);
            string t1ExprName = t1Expr ?? "DoubleArrayOne";
            string t2ExprName = t2Expr ?? "DoubleArrayTwo";

            return new FloatingPointComparison<double>(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName, result);
        }
        if (typeof(T) == typeof(object[]))
        // TODO: Code not reached by unit tests - need to add tests for this case
        {
            object[] oArr1 = Unsafe.As<T, object[]>(ref t1);
            object[] oArr2 = Unsafe.As<T, object[]>(ref t2);
            string t1ExprName = t1Expr ?? "ArrayOne";
            string t2ExprName = t2Expr ?? "ArrayTwo";

            return new ObjectComparison(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName, result);
        }
        if (IsPrimitiveOrEnum(typeof(T)))
        {
            throw new NotImplementedException(typeof(T).Name);
        }
        else
        {
            string t1ExprName = t1Expr ?? "ObjectOne";
            string t2ExprName = t2Expr ?? "ObjectTwo";

            return new ObjectComparison(Configuration)
                .Compare(t1!, t2!, t1ExprName, t2ExprName, result);
        }
    }

    /// <summary>
    /// Builds and executes the comparison between two objects.
    /// </summary>
    /// <param name="o1">First object for comparison.</param>
    /// <param name="o2">Second object for comparison.</param>
    /// <param name="o1Expr">Caller argument expression for o1.</param>
    /// <param name="o2Expr">Caller argument expression for o2.</param>
    /// <returns>A <see cref="ComparisonResult"/> representing the outcome of the comparison.</returns>
    public ComparisonResult Compare(object? o1, object? o2,
        [CallerArgumentExpression(nameof(o1))] string? o1Expr = null,
        [CallerArgumentExpression(nameof(o2))] string? o2Expr = null)
    {
        var result = new ComparisonResult();
        HandleNullability(o1, o2, o1Expr, o2Expr, result);

        // Check basic nullability results first
        if (result.WasSuccessful == false || result.AllMatched == false)
        {
            return result;
        }
        if (result.WarningCount > 0 &&
            result.Warnings.Any(warning => warning.Code == ComparisonErrors.BothObjectsAreNullCode))
        {
            return result;
        }

        Type type = o1!.GetType();
        string t1ExprName = o1Expr ?? "ObjectOne";
        string t2ExprName = o2Expr ?? "ObjectTwo";

        // Primitives
        if (type == typeof(string))
            return new StringComparison(Configuration).Compare((string)o1, (string)o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(bool))
            return new BoolComparison(Configuration).Compare((bool)o1, (bool)o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(byte))
            return new ByteComparison(Configuration).Compare((byte)o1, (byte)o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(byte?))
            return new ByteComparison(Configuration).Compare((byte?)o1, (byte?)o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(short))
            return new NumericComparison<short>(Configuration).Compare((short)o1, (short)o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(int))
            return new NumericComparison<int>(Configuration).Compare((int)o1, (int)o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(long))
            return new NumericComparison<long>(Configuration).Compare((long)o1, (long)o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(float))
            return new FloatingPointComparison<float>(Configuration).Compare((float)o1, (float)o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(double))
            return new FloatingPointComparison<double>(Configuration).Compare((double)o1, (double)o2!, t1ExprName, t2ExprName, result);

        // Array types
        if (type == typeof(string[]))
            return new StringComparison(Configuration).Compare((string[])o1, (string[])o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(bool[]))
            return new BoolComparison(Configuration).Compare((bool[])o1, (bool[])o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(byte[]))
            return new ByteComparison(Configuration).Compare((byte[])o1, (byte[])o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(byte?[]))
            return new ByteComparison(Configuration).Compare((byte?[]?)o1, (byte?[]?)o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(short[]))
            // TODO: Code not reached by unit tests - need to add tests for this case
            return new NumericComparison<short>(Configuration).Compare((short[])o1, (short[])o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(int[]))
            // TODO: Code not reached by unit tests - need to add tests for this case
            return new NumericComparison<int>(Configuration).Compare((int[])o1, (int[])o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(long[]))
            return new NumericComparison<long>(Configuration).Compare((long[])o1, (long[])o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(float[]))
            return new FloatingPointComparison<float>(Configuration).Compare((float[])o1, (float[])o2!, t1ExprName, t2ExprName, result);

        if (type == typeof(double[]))
            return new FloatingPointComparison<double>(Configuration).Compare((double[])o1, (double[])o2!, t1ExprName, t2ExprName, result);


        if (type == typeof(object[]))
            return new ObjectComparison(Configuration)
                .Compare((object[])o1, (object[])o2!, t1ExprName, t2ExprName, result);


        if (IsPrimitiveOrEnum(type))
            throw new NotImplementedException(type.Name);


        return new ObjectComparison(Configuration)
            .Compare(o1, o2, t1ExprName, t2ExprName, result);
    }

    /// <summary>
    /// Builds and executes the comparison between two object arrays.
    /// </summary>
    /// <param name="o1Arr">First object array for comparison.</param>
    /// <param name="o2Arr">Second object array for comparison.</param>
    /// <param name="o1ArrExpr">Caller argument expression for o1Arr.</param>
    /// <param name="o2ArrExpr">Caller argument expression for o2Arr.</param>
    /// <returns>A <see cref="ComparisonResult"/> representing the outcome of the comparison.</returns>
    public ComparisonResult Compare(object[] o1Arr, object[] o2Arr,
        [CallerArgumentExpression(nameof(o1Arr))] string? o1ArrExpr = null,
        [CallerArgumentExpression(nameof(o2Arr))] string? o2ArrExpr = null)
    {
        var result = new ComparisonResult();
        HandleNullability(o1Arr, o2Arr, o1ArrExpr, o2ArrExpr, result);

        // Check basic nullability results first
        if (result.WasSuccessful == false)
        {
            // TODO: Code not reached by unit tests - need to add tests for this case
            return result;
        }
        if (result.AllMatched == false &&
            result.Mismatches.First().Code == ComparisonMismatches.NullPassedAsArgumentCode)
        {
            return result;
        }

        string t1ExprName = o1ArrExpr ?? "ObjectArrayOne";
        string t2ExprName = o2ArrExpr ?? "ObjectArrayTwo";

        return new ObjectComparison(Configuration)
            .Compare(o1Arr, o2Arr, t1ExprName, t2ExprName, result);
    }


    /// <summary>
    /// Handles nullability checks for object comparisons.
    /// </summary>
    /// <param name="o1">First object.</param>
    /// <param name="o2">Second object.</param>
    /// <param name="t1Expr">Caller argument expression for o1.</param>
    /// <param name="t2Expr">Caller argument expression for o2.</param>
    /// <returns>A <see cref="ComparisonResult"/> indicating nullability issues.</returns>
    private ComparisonResult HandleNullability(object? o1, object? o2, string? t1Expr, string? t2Expr, ComparisonResult result)
    {
        if (o1 is null || o2 is null)
        {
            if (o1 is null && o2 is null)
            {
                if (Configuration.AllowNullComparison)
                {
                    result.AddWarning(ComparisonErrors.BothObjectsAreNull());
                    return result;
                }
                else
                {
                    result.AddError(ComparisonErrors.BothObjectsAreNull());
                    return result;
                }
            }

            if (o1 is null)
            {
                string t1ExprName = t1Expr ?? $"{o2!.GetType().Name}Two";

                if (Configuration.AllowNullComparison)
                {
                    result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(1, o2!.GetType()));
                    return result;
                }
                else
                {
                    result.AddError(ComparisonMismatches.NullPassedAsArgument(t1ExprName, o2!.GetType()));
                    return result;
                }
            }

            if (o2 is null)
            {
                string t2ExprName = t2Expr ?? $"{o1.GetType().Name}One";
                if (Configuration.AllowNullComparison)
                {
                    result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(2, o1!.GetType()));
                    return result;
                }
                else
                {
                    result.AddError(ComparisonMismatches.NullPassedAsArgument(t2ExprName, o1!.GetType()));
                    return result;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Handles nullability checks for generic type comparisons.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="t1">First object.</param>
    /// <param name="t2">Second object.</param>
    /// <param name="t1Expr">Caller argument expression for t1.</param>
    /// <param name="t2Expr">Caller argument expression for t2.</param>
    /// <returns>A <see cref="ComparisonResult"/> indicating nullability issues.</returns>
    private ComparisonResult HandleNullability<T>(T t1, T t2, string? t1Expr, string? t2Expr, ComparisonResult result)
    {
        if (t1 is null || t2 is null)
        {
            if (t1 is null && t2 is null)
            {
                if (Configuration.AllowNullsInArguments && Configuration.AllowNullComparison)
                {
                    result.AddWarning(ComparisonErrors.BothObjectsAreNull());
                    return result;
                }

                result.AddError(ComparisonErrors.BothObjectsAreNull());
                return result;
            }

            string t1ExprName = t1Expr ?? $"{typeof(T).Name}One";
            string t2ExprName = t1Expr ?? $"{typeof(T).Name}Two";

            if (t1 is null || t2 is null)
            {
                if (Configuration.AllowNullsInArguments == false)
                {
                    // TODO: Code not reached by unit tests - need to add tests for this case
                    result.AddError(ComparisonErrors.NullPassedAsArgument(t1ExprName, typeof(T)));
                    return result;
                }

                if (Configuration.AllowNullComparison == false)
                {
                    result.AddError(ComparisonErrors.OneOfTheObjectsIsNull(t1ExprName, t2ExprName));
                }

                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(t1ExprName, t2ExprName, typeof(T)));
                return result;
            }
        }

        return result;
    }

    /// <summary>
    /// Determines if the specified type is a primitive or enum.
    /// </summary>
    /// <param name="type1">The type to check.</param>
    /// <returns><c>true</c> if the type is primitive or enum; otherwise, <c>false</c>.</returns>
    private static bool IsPrimitiveOrEnum(Type type1) => type1.IsPrimitive || type1.IsEnum;
}
