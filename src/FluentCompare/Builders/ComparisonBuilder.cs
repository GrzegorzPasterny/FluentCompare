using System.Runtime.CompilerServices;

/// <summary>
/// Builder object to configure and perform comparisons.
/// </summary>
public class ComparisonBuilder
{
    private readonly int _currentDepth = 0;

    internal ComparisonBuilder() { }

    internal ComparisonBuilder(int currentDepth)
    {
        _currentDepth = currentDepth;
    }
    internal static ComparisonBuilder Create(int currentDepth) => new ComparisonBuilder(currentDepth);

    /// <summary>
    /// Configuration object for the comparison
    /// </summary>
    public ComparisonConfiguration Configuration { get; private set; } = new();

    /// <summary>
    /// Starting point to begin configuring and performing comparisons.
    /// </summary>
    /// <returns></returns>
    public static ComparisonBuilder Create() => new ComparisonBuilder();

    /// <summary>
    /// Builds and executes the comparison for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ComparisonResult Compare<T>(params T[] t)
    {
        if (typeof(T) == typeof(string))
            return new StringComparison(Configuration)
                .Compare((string[])(object)t);

        if (typeof(T) == typeof(string[]))
            return new StringComparison(Configuration)
                .Compare((string[][])(object)t);

        if (typeof(T) == typeof(bool))
            return new BoolComparison(Configuration)
                .Compare((bool[])(object)t);

        if (typeof(T) == typeof(bool[]))
            return new BoolComparison(Configuration)
                .Compare((bool[][])(object)t);

        if (typeof(T) == typeof(byte))
            return new ByteComparison(Configuration)
                .Compare((byte[])(object)t);

        if (typeof(T) == typeof(byte[]))
            return new ByteComparison(Configuration)
                .Compare((byte[][])(object)t);

        if (typeof(T) == typeof(short))
            return new NumericComparison<short>(Configuration)
                .Compare((short[])(object)t);

        if (typeof(T) == typeof(short[]))
            return new NumericComparison<short>(Configuration)
                .Compare((short[][])(object)t);

        if (typeof(T) == typeof(int))
            return new NumericComparison<int>(Configuration)
                .Compare((int[])(object)t);

        if (typeof(T) == typeof(int[]))
            return new NumericComparison<int>(Configuration)
                .Compare((int[][])(object)t);

        if (typeof(T) == typeof(long))
            return new NumericComparison<long>(Configuration)
                .Compare((long[])(object)t);

        if (typeof(T) == typeof(long[]))
            return new NumericComparison<long>(Configuration)
                .Compare((long[][])(object)t);

        if (typeof(T) == typeof(double))
            return new DoubleComparison(Configuration)
                .Compare((double[])(object)t);

        if (typeof(T) == typeof(double[]))
            return new DoubleComparison(Configuration)
                .Compare((double[][])(object)t);

        if (typeof(T) == typeof(object[]))
            return new ObjectArrayComparison(Configuration, _currentDepth)
                .Compare((object[][])(object)t);

        if (IsPrimitiveEnumOrString(typeof(T)))
            throw new NotImplementedException(typeof(T).Name);

        return new ObjectComparison(Configuration, _currentDepth)
            .Compare(t);
    }


    /// <summary>
    /// Builds and executes the comparison between <paramref name="t1"/> and <paramref name="t2"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t1">First object for comparison</param>
    /// <param name="t2">Second object for comparison</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ComparisonResult Compare<T>(T t1, T t2,
        [CallerArgumentExpression(nameof(t1))] string? t1Expr = null,
        [CallerArgumentExpression(nameof(t2))] string? t2Expr = null)
    {
        if (t1 is null || t2 is null)
        {
            ComparisonResult result = new();

            if (t1 is null && t2 is null)
            {
                result.AddWarning(ComparisonErrors.BothObjectsAreNull());
                return result;
            }

            if (t1 is null)
            {
                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(1, typeof(T)));
                return result;
            }

            if (t2 is null)
            {
                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(2, typeof(T)));
                return result;
            }
        }

        if (typeof(T) == typeof(string))
        {
            string s1 = Unsafe.As<T, string>(ref t1);
            string s2 = Unsafe.As<T, string>(ref t2);
            string t1ExprName = t1Expr ?? "StringOne";
            string t2ExprName = t2Expr ?? "StringTwo";

            return new StringComparison(Configuration)
                .Compare(s1, s2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(string[]))
        {
            string[] sArr1 = Unsafe.As<T, string[]>(ref t1);
            string[] sArr2 = Unsafe.As<T, string[]>(ref t2);
            string t1ExprName = t1Expr ?? "StringArrayOne";
            string t2ExprName = t2Expr ?? "StringArrayTwo";

            return new StringComparison(Configuration)
                .Compare(sArr1, sArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(bool))
        {
            bool s1 = Unsafe.As<T, bool>(ref t1);
            bool s2 = Unsafe.As<T, bool>(ref t2);
            string t1ExprName = t1Expr ?? "BoolOne";
            string t2ExprName = t2Expr ?? "BoolTwo";

            return new BoolComparison(Configuration)
                .Compare(s1, s2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(bool[]))
        {
            bool[] s1 = Unsafe.As<T, bool[]>(ref t1);
            bool[] s2 = Unsafe.As<T, bool[]>(ref t2);
            string t1ExprName = t1Expr ?? "BoolArrayOne";
            string t2ExprName = t2Expr ?? "BoolArrayTwo";

            return new BoolComparison(Configuration)
                .Compare(s1, s2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(byte))
        {
            byte s1 = Unsafe.As<T, byte>(ref t1);
            byte s2 = Unsafe.As<T, byte>(ref t2);
            string t1ExprName = t1Expr ?? "ByteOne";
            string t2ExprName = t2Expr ?? "ByteTwo";

            return new ByteComparison(Configuration)
                .Compare(s1, s2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(byte[]))
        {
            byte[] s1 = Unsafe.As<T, byte[]>(ref t1);
            byte[] s2 = Unsafe.As<T, byte[]>(ref t2);
            string t1ExprName = t1Expr ?? "ByteArrayOne";
            string t2ExprName = t2Expr ?? "ByteArrayTwo";

            return new ByteComparison(Configuration)
                .Compare(s1, s2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(short))
        {
            short o1 = Unsafe.As<T, short>(ref t1);
            short o2 = Unsafe.As<T, short>(ref t2);
            string t1ExprName = t1Expr ?? "ShortOne";
            string t2ExprName = t2Expr ?? "ShortTwo";

            return new NumericComparison<short>(Configuration)
                .Compare(o1, o2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(short[]))
        {
            short[] oArr1 = Unsafe.As<T, short[]>(ref t1);
            short[] oArr2 = Unsafe.As<T, short[]>(ref t2);
            string t1ExprName = t1Expr ?? "ShortArrayOne";
            string t2ExprName = t2Expr ?? "ShortArrayTwo";

            return new NumericComparison<short>(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(int))
        {
            int o1 = Unsafe.As<T, int>(ref t1);
            int o2 = Unsafe.As<T, int>(ref t2);
            string t1ExprName = t1Expr ?? "IntOne";
            string t2ExprName = t2Expr ?? "IntTwo";

            return new NumericComparison<int>(Configuration)
                .Compare(o1, o2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(int[]))
        {
            int[] oArr1 = Unsafe.As<T, int[]>(ref t1);
            int[] oArr2 = Unsafe.As<T, int[]>(ref t2);
            string t1ExprName = t1Expr ?? "IntArrayOne";
            string t2ExprName = t2Expr ?? "IntArrayTwo";

            return new NumericComparison<int>(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(long))
        {
            long o1 = Unsafe.As<T, long>(ref t1);
            long o2 = Unsafe.As<T, long>(ref t2);
            string t1ExprName = t1Expr ?? "LongOne";
            string t2ExprName = t2Expr ?? "LongTwo";

            return new NumericComparison<long>(Configuration)
                .Compare(o1, o2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(long[]))
        {
            long[] oArr1 = Unsafe.As<T, long[]>(ref t1);
            long[] oArr2 = Unsafe.As<T, long[]>(ref t2);
            string t1ExprName = t1Expr ?? "LongArrayOne";
            string t2ExprName = t2Expr ?? "LongArrayTwo";

            return new NumericComparison<long>(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(double))
        {
            var o1 = Unsafe.As<T, double>(ref t1);
            var o2 = Unsafe.As<T, double>(ref t2);
            string t1ExprName = t1Expr ?? "DoubleOne";
            string t2ExprName = t2Expr ?? "DoubleTwo";

            return new DoubleComparison(Configuration)
                .Compare(o1, o2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(double[]))
        {
            var oArr1 = Unsafe.As<T, double[]>(ref t1);
            var oArr2 = Unsafe.As<T, double[]>(ref t2);
            string t1ExprName = t1Expr ?? "DoubleArrayOne";
            string t2ExprName = t2Expr ?? "DoubleArrayTwo";

            return new DoubleComparison(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(object[]))
        {
            object[] oArr1 = Unsafe.As<T, object[]>(ref t1);
            object[] oArr2 = Unsafe.As<T, object[]>(ref t2);
            string t1ExprName = t1Expr ?? "ArrayOne";
            string t2ExprName = t2Expr ?? "ArrayTwo";

            return new ObjectArrayComparison(Configuration, _currentDepth)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (IsPrimitiveEnumOrString(typeof(T)))
        {
            throw new NotImplementedException(typeof(T).Name);
        }
        else // objects and non-primitive types
        {
            string t1ExprName = t1Expr ?? "ObjectOne";
            string t2ExprName = t2Expr ?? "ObjectTwo";

            return new ObjectComparison(Configuration, _currentDepth)
                .Compare(t1, t2, t1ExprName, t2ExprName);
        }
    }

    public ComparisonResult Compare(object o1, object o2,
    [CallerArgumentExpression(nameof(o1))] string? o1Expr = null,
    [CallerArgumentExpression(nameof(o2))] string? o2Expr = null)
    {
        ComparisonResult comparisonResult = HandleNullability(o1, o2);

        if (comparisonResult.WasSuccessful == false)
        {
            return comparisonResult;
        }

        Type type = o1.GetType();
        string t1ExprName = o1Expr ?? "ObjectOne";
        string t2ExprName = o2Expr ?? "ObjectTwo";

        // Primitives
        if (type == typeof(string))
            return new StringComparison(Configuration).Compare((string)o1, (string)o2, t1ExprName, t2ExprName);

        if (type == typeof(bool))
            return new BoolComparison(Configuration).Compare((bool)o1, (bool)o2, t1ExprName, t2ExprName);

        if (type == typeof(byte))
            return new ByteComparison(Configuration).Compare((byte)o1, (byte)o2, t1ExprName, t2ExprName);

        if (type == typeof(short))
            return new NumericComparison<short>(Configuration).Compare((short)o1, (short)o2, t1ExprName, t2ExprName);

        if (type == typeof(int))
            return new NumericComparison<int>(Configuration).Compare((int)o1, (int)o2, t1ExprName, t2ExprName);

        if (type == typeof(long))
            return new NumericComparison<long>(Configuration).Compare((long)o1, (long)o2, t1ExprName, t2ExprName);

        if (type == typeof(double))
            return new DoubleComparison(Configuration).Compare((double)o1, (double)o2, t1ExprName, t2ExprName);

        // Array types
        if (type == typeof(string[]))
            return new StringComparison(Configuration).Compare((string[])o1, (string[])o2, t1ExprName, t2ExprName);

        if (type == typeof(bool[]))
            return new BoolComparison(Configuration).Compare((bool[])o1, (bool[])o2, t1ExprName, t2ExprName);

        if (type == typeof(byte[]))
            return new ByteComparison(Configuration).Compare((byte[])o1, (byte[])o2, t1ExprName, t2ExprName);

        if (type == typeof(short[]))
            return new NumericComparison<short>(Configuration).Compare((short[])o1, (short[])o2, t1ExprName, t2ExprName);

        if (type == typeof(int[]))
            return new NumericComparison<int>(Configuration).Compare((int[])o1, (int[])o2, t1ExprName, t2ExprName);

        if (type == typeof(long[]))
            return new NumericComparison<long>(Configuration).Compare((long[])o1, (long[])o2, t1ExprName, t2ExprName);

        if (type == typeof(double[]))
            return new DoubleComparison(Configuration).Compare((double[])o1, (double[])o2, t1ExprName, t2ExprName);


        if (type == typeof(object[]))
            return new ObjectArrayComparison(Configuration, _currentDepth)
                .Compare((object[])o1, (object[])o2, t1ExprName, t2ExprName);


        if (IsPrimitiveEnumOrString(type))
            throw new NotImplementedException(type.Name);


        return new ObjectComparison(Configuration, _currentDepth)
            .Compare(o1, o2, t1ExprName, t2ExprName);
    }

    private static ComparisonResult HandleNullability(object o1, object o2)
    {
        if (o1 is null || o2 is null)
        {
            var result = new ComparisonResult();

            if (o1 is null && o2 is null)
            {
                result.AddWarning(ComparisonErrors.BothObjectsAreNull());
                return result;
            }

            if (o1 is null)
            {
                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(1, o2!.GetType()));
                return result;
            }

            if (o2 is null)
            {
                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(2, o1!.GetType()));
                return result;
            }
        }
    }

    public ComparisonResult Compare(object[] o1Arr, object[] o2Arr,
        [CallerArgumentExpression(nameof(o1Arr))] string? o1ArrExpr = null,
        [CallerArgumentExpression(nameof(o2Arr))] string? o2ArrExpr = null)
    {
        string t1ExprName = o1ArrExpr ?? "ObjectArrayOne";
        string t2ExprName = o2ArrExpr ?? "ObjectArrayTwo";

        return new ObjectArrayComparison(Configuration, _currentDepth)
            .Compare(o1Arr, o2Arr, t1ExprName, t2ExprName);
    }

    public ComparisonBuilder UseConfiguration(ComparisonConfiguration configuration)
    {
        Configuration = configuration;
        return this;
    }

    public DoubleComparisonBuilder ForDouble()
    {
        return new DoubleComparisonBuilder(Configuration);
    }

    public ComparisonBuilder Configure(Action<ComparisonConfiguration> configure)
    {
        configure(Configuration);
        return this;
    }

    // TODO: All of the configuration methods can be in a partial class
    /// <summary>
    /// Sets the <paramref name="comparisonType"/>. Default is <see cref="ComparisonType.EqualTo"/>.
    /// </summary>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    public ComparisonBuilder UseComparisonType(ComparisonType comparisonType)
    {
        Configuration.ComparisonType = comparisonType;
        return this;
    }

    /// <summary>
    /// Checks if objects are equivalent by comparing their properties. Default setting.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder UsePropertyEquality()
    {
        Configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.PropertyEquality;
        return this;
    }

    /// <summary>
    /// Checks if objects are equivalent by comparing their references.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder UseReferenceEquality()
    {
        Configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.ReferenceEquality;
        return this;
    }

    /// <summary>
    /// Sets the string comparison type. Default is <see cref="System.StringComparison.Ordinal"/>.
    /// </summary>
    /// <param name="stringComparison"></param>
    /// <returns></returns>
    public ComparisonBuilder UseStringComparisonType(System.StringComparison stringComparison)
    {
        Configuration.StringConfiguration.StringComparisonType = stringComparison;
        return this;
    }

    /// <summary>
    /// Sets the maximum depth for recursive comparison of complex types. Default is 5.
    /// </summary>
    /// <param name="depth">New comparison depth.</param>
    /// <returns></returns>
    public ComparisonBuilder SetComparisonDepth(int depth)
    {
        Configuration.MaximumComparisonDepth = depth;
        return this;
    }

    /// <summary>
    /// Allows comparing nulls. If both values are null, they are considered equal. Default behavior.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder AllowNullComparison()
    {
        Configuration.AllowNullComparison = true;
        return this;
    }

    /// <summary>
    /// Disallows comparing nulls. Comparing nulls will result in a mismatch.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder DisallowNullComparison()
    {
        Configuration.AllowNullComparison = false;
        return this;
    }

    /// <summary>
    /// Allows nulls in the arguments to be compared. Default behavior.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder AllowNullsInArguments()
    {
        Configuration.AllowNullsInArguments = true;
        return this;
    }

    /// <summary>
    /// Disallows nulls in the arguments to be compared. Passing nulls will result in an error.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder DisallowNullsInArguments()
    {
        Configuration.AllowNullsInArguments = false;
        return this;
    }

    private static bool IsPrimitiveEnumOrString(Type type1) => type1.IsPrimitive || type1.IsEnum || type1 == typeof(string);

}
